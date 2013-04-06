using System.Runtime.Serialization;

namespace HHTravel.Infrastructure.Crosscutting
{
    [DataContract]
    public class ImageInfo
    {
        /// <summary>
        /// alt
        /// </summary>
        [DataMember]
        public string Alt { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Url { get; set; }
    }
}