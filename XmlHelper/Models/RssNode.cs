using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlHelper.Models
{
    public class RssNode
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime? PubDate { get; set; }
        public string Copyright { get; set; }
        public string Language { get; set; }
        public string Ttl { get; set; }

        public IEnumerable<Item> Items { get; set; }
    }

    public class Item
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Guid { get; set; }
    }
}
