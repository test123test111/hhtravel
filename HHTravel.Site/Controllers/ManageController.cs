using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EFCachingProvider.Caching;
using HHTravel.Infrastructure.Caching;
using HHTravel.Site.Filter;
using HHTravel.Site.Models;
using HHTravel.Infrastructure;

namespace HHTravel.Site.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Manage/

        public ActionResult Index()
        {
            return View();
        }

        [NoCache]
        public ActionResult jSecondLevelCache(string key, string column, string row)
        {
            if (key == "undefined") key = "";
            var query = from item in EFCache.Instance.Find(key)
                        select item;

            if (!string.IsNullOrEmpty(column))
            {
                query = from item in query
                        where item.Value is DbQueryResults
                            && ((DbQueryResults)item.Value).ColumnNames.Any(c => c.Contains(column))
                        select item;
            }

            if (!string.IsNullOrEmpty(row))
            {
                query = from item in query
                        where item.Value is DbQueryResults
                            && ((DbQueryResults)item.Value).Rows.Any(arr => arr.Any(c => c.ToString().Contains(row)))
                        select item;
            }

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SecondLevelCache()
        {
            ViewBag.TopNavModel = new TopNavModel(false);

            var model = new SecondLevelCacheModel(
                EFCache.Instance.CacheHits,
                EFCache.Instance.CacheMisses,
                EFCache.Instance.CacheItemAdds);

            return View(model);
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            StringBuilder responseText = new StringBuilder();
            bool security = false;
            if (this.Request.Form["hidPwd"] == "hhtravel123!$()")
            {
                security = true;
            }
            else
            {
                responseText.Append("验证码不正确!");
            }

            if (security == true)
            {
                if (Request.Files.Count > 0)
                {
                    foreach (string key in Request.Files.AllKeys)
                    {
                        HttpPostedFileBase file = Request.Files[key];
                        if (file.ContentLength > 0)
                        {
                            string ef = Path.GetExtension(file.FileName).TrimStart('.');
                            if (string.Equals(ef, "swf", StringComparison.OrdinalIgnoreCase))
                            {
                                string path = Server.MapPath(@"..\swf\");
                                string fileName = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
                                string phyFile = string.Format(@"{0}{1}", path, fileName);
                                file.SaveAs(phyFile);
                                responseText.AppendFormat("{0}|", string.Format("swf/{0}", fileName));
                            }
                            else if (string.Equals(ef, "zip", StringComparison.OrdinalIgnoreCase))
                            {
                                string path = Server.MapPath(@"..\event\");
                                string fileName = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
                                string phyFile = string.Format(@"{0}{1}", path, fileName);
                                file.SaveAs(phyFile);
                                ZipHelper.UnZip(phyFile, path);
                                System.IO.File.Delete(phyFile);
                                responseText.AppendFormat("{0}|", string.Format("event/{0}", fileName));
                            }
                        }
                    }
                }
            }
            return Content(responseText.ToString().TrimEnd('|'));
        }

        [HttpGet]
        public ActionResult UploadFileTest()
        {
            return View();
        }
    }
}