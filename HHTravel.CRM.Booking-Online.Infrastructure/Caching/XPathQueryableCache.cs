using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Caching
{
    /// <summary>
    /// SAF.Cache is an object caching service that
    /// present the cached object in an hierarchical structure.
    /// It uses a pluggable object storage mechnism or cache strategy
    /// to storage the objects.
    /// </summary>
    public class XPathQueryableCache : IQueryableCache
    {
        private static IQueryableCache cacheInstance;
        private static object lockObject = new object();
        private HttpContext httpContext;
        private XmlElement objectXmlMap;

        //private static XPathQueryableCache cache;
        private XmlDocument rootXml = new XmlDocument();

        /// <summary>
        /// Private construtor, required for singleton design pattern.
        /// </summary>
        private XPathQueryableCache(HttpContext httpContext)
        {
            this.httpContext = httpContext;

            //create an Xml used as a map between  xml expression and object cached in the
            //physical storage.
            objectXmlMap = rootXml.CreateElement("Cache");

            //build the internal xml document.
            rootXml.AppendChild(objectXmlMap);
        }

        public static IQueryableCache Instance
        {
            get
            {
                if (cacheInstance == null)
                {
                    lock (lockObject)
                    {
                        if (cacheInstance == null)
                            cacheInstance = new XPathQueryableCache(null);
                    }
                }

                return cacheInstance;
            }
        }

        private Cache HttpCache
        {
            get
            {
                if (this.httpContext != null)
                {
                    return this.httpContext.Cache;
                }

                var context = HttpContext.Current;
                if (context == null)
                {
                    throw new InvalidOperationException("Unable to determine HTTP context.");
                }

                return context.Cache;
            }
        }

        IDictionary<string, object> IQueryableCache.GetObjectList(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve the cached object using its hierarchical location
        /// </summary>
        /// <param name="xpath">hierarchical location of the object in xml document</param>
        /// <returns>cached object </returns>
        public virtual object Get(string xpath)
        {
            object o = null;
            XmlNode node = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));

            //if the hierarchical location existed in the xml, retrieve the object
            //otherwise, return the object as null
            if (node != null)
            {
                string objectId = node.Attributes["objectId"].Value;

                //retrieve the object through cache strategy
                o = this.HttpCache.Get(objectId);
            }
            return o;
        }

        /// <summary>
        /// Retrive a list of object under a hierarchical location
        /// </summary>
        /// <param name="xpath">hierarchical location</param>
        /// <returns>an array of objects</returns>
        public virtual object[] GetObjectList(string xpath)
        {
            XmlNode group = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
            XmlNodeList results = group.SelectNodes(PrepareXpath(xpath) + "/*[@objectId]");
            ArrayList objects = new ArrayList();
            string objectId = null;

            //loop through each node and link the object in object[]
            //to objects stored via cache strategy
            foreach (XmlNode result in results)
            {
                objectId = result.Attributes["objectId"].Value;
                objects.Add(this.HttpCache.Get(objectId));
            }

            //convert the ArrayList to object[]
            return (object[])objects.ToArray(typeof(System.Object));
        }

        /// <summary>
        /// Add the object to the underlying storage and Xml mapping document
        /// </summary>
        /// <param name="xpath">the hierarchical location of the object in Xml document </param>
        /// <param name="o">the object to be cached</param>
        public virtual void Insert(string xpath, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            //clear up the xpath expression
            string newXpath = PrepareXpath(xpath);
            int separator = newXpath.LastIndexOf("/");

            //find the group name
            string group = newXpath.Substring(0, separator);

            //find the item name
            string element = newXpath.Substring(separator + 1);

            XmlNode groupNode = objectXmlMap.SelectSingleNode(group);

            //determin if group is already exist?, if not, create one.
            if (groupNode == null)
            {
                lock (this)
                {
                    //build the xml tree
                    groupNode = CreateNode(group);
                }
            }

            //get a unique key to identity of object, it is used to map
            //between xml and object key used in the cache strategy
            string objectId = System.Guid.NewGuid().ToString();

            //create an new element and new attribute for this perticular object
            XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
            XmlAttribute objectAttribute = objectXmlMap.OwnerDocument.CreateAttribute("objectId");
            objectAttribute.Value = objectId;
            objectElement.Attributes.Append(objectAttribute);

            //Add the object element to the Xml document
            groupNode.AppendChild(objectElement);

            //add the object to the underlying storage through cache strategy
            this.HttpCache.Insert(objectId, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }

        /// <summary>
        /// Remove the object from the storage and clear the Xml assocated with
        /// the object
        /// </summary>
        /// <param name="xpath">hierarchical locatioin of the object</param>
        public virtual void Remove(string xpath)
        {
            XmlNode result = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));

            //check if the xpath refers to a group(container) or
            //actual element for cached object
            if (result.HasChildNodes)
            {
                //remove all the cached object in the hastable
                //and remove all the child nodes
                XmlNodeList objects = result.SelectNodes("*[@objectId]");
                string objectId = "";
                foreach (XmlNode node in objects)
                {
                    objectId = node.Attributes["objectId"].Value;
                    node.ParentNode.RemoveChild(node);

                    //use cache strategy to remove the objects from the
                    //underlying storage
                    this.HttpCache.Remove(objectId);
                }
            }
            else
            {
                //just remove the element node and the object associate with it
                string objectId = result.Attributes["objectId"].Value;
                result.ParentNode.RemoveChild(result);
                this.HttpCache.Remove(objectId);
            }
        }

        /// <summary>
        /// CreateNode is responsible for creating the xml tree that is
        /// specificed in the hierarchical location of the object.
        /// </summary>
        /// <param name="xpath">hierarchical location</param>
        /// <returns></returns>
        private XmlNode CreateNode(string xpath)
        {
            string[] xpathArray = xpath.Split('/');
            string root = "";
            XmlNode parentNode = (XmlNode)objectXmlMap;

            //loop through the array of levels and create the corresponding node for each level
            //skip the root node.
            for (int i = 1; i < xpathArray.Length; i++)
            {
                XmlNode node = objectXmlMap.SelectSingleNode(root + "/" + xpathArray[i]);

                // if the current location doesn't exist, build one
                //otherwise set the current locaiton to the it child location
                if (node == null)
                {
                    XmlElement newElement = objectXmlMap.OwnerDocument.CreateElement(xpathArray[i]);
                    parentNode.AppendChild(newElement);
                }

                //set the new location to one level lower
                root = root + "/" + xpathArray[i];
                parentNode = objectXmlMap.SelectSingleNode(root);
            }
            return parentNode;
        }

        /// <summary>
        /// clean up the xpath so that extra '/' is removed
        /// </summary>
        /// <param name="xpath">hierarchical location</param>
        /// <returns></returns>
        private string PrepareXpath(string xpath)
        {
            string[] xpathArray = xpath.Split('/');
            xpath = "/Cache";
            foreach (string s in xpathArray)
            {
                if (s != "")
                {
                    xpath = xpath + "/" + s;
                }
            }
            return xpath;
        }
    }
}