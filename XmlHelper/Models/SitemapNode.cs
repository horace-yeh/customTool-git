using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlHelper.Models
{
    public enum SitemapFrequency
    {
        Never,
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Always
    }

    public enum XmlNamespaceEnum
    {
        Xmlns,
        News,
        Image,
        Xsi,
        SchemaLocation
    }

    public class SitemapNodeBase
    {
        public SitemapFrequency? Frequency { get; set; }
        public string Url { get; set; }
    }

    public class SitemapNode : SitemapNodeBase
    {
        public NewsNode News { get; set; }
        public ImageNode Image { get; set; }
    }

    public class NewsNode
    {
        public string Name { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
    }

    public class ImageNode
    {
        public string Url { get; set; }
        public string Title { get; set; }
    }
}
