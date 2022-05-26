using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using XmlHelper.Models;

namespace XmlHelper
{
    public class XmlTool
    {
        private readonly Dictionary<XmlNamespaceEnum, XNamespace> XNamespaceDict;

        public XmlTool()
        {
            this.XNamespaceDict = this.GenerateXNamespace();
        }

        public void SaveSitemapDocument(IEnumerable<SitemapNode> sitemapNodes, string FileName)
        {
            XDocument document = this.SitemapDocumentGenerate(sitemapNodes);
            document.Save(FileName);
        }

        public void SaveRssDocument(RssNode rssNode, string FileName)
        {
            //REF: https://demo.tc/post/%E7%94%A2%E7%94%9F%20RSS%20feed

            XmlTextWriter xmlTw = new XmlTextWriter(FileName, Encoding.UTF8);
            xmlTw.Formatting = Formatting.Indented;
            xmlTw.WriteStartDocument();
            xmlTw.WriteStartElement("rss");
            xmlTw.WriteAttributeString("version", "2.0");
            xmlTw.WriteStartElement("channel");
            xmlTw.WriteElementString("title", rssNode.Title);
            xmlTw.WriteElementString("link", rssNode.Link);
            xmlTw.WriteElementString("description", rssNode.Description);
            xmlTw.WriteElementString("pubDate", rssNode.PubDate.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"));
            xmlTw.WriteElementString("copyright", rssNode.Copyright);
            xmlTw.WriteElementString("language", rssNode.Language);
            xmlTw.WriteElementString("ttl", rssNode.Ttl);

            foreach (var item in rssNode.Items)
            {
                xmlTw.WriteStartElement("item");
                xmlTw.WriteElementString("title", item.Title);
                xmlTw.WriteElementString("link", item.Link);
                xmlTw.WriteElementString("description", item.Description);

                xmlTw.WriteStartElement("guid");
                xmlTw.WriteAttributeString("isPermaLink", "false");
                xmlTw.WriteString(item.Guid);
                xmlTw.WriteEndElement();

                xmlTw.WriteEndElement();
            }
            xmlTw.WriteEndElement();
            xmlTw.WriteEndElement();
            xmlTw.WriteEndDocument();
            xmlTw.Flush();
            xmlTw.Close();
        }

        public StringBuilder RssDocumentGenerate(RssNode rssNode)
        {
            //REF: https://demo.tc/post/%E7%94%A2%E7%94%9F%20RSS%20feed

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            StringBuilder builder = new StringBuilder();
            using (XmlWriter xmlTw = XmlWriter.Create(builder, settings))
            {
                xmlTw.WriteStartDocument();
                xmlTw.WriteStartElement("rss");
                xmlTw.WriteAttributeString("version", "2.0");
                xmlTw.WriteStartElement("channel");
                xmlTw.WriteElementString("title", rssNode.Title);
                xmlTw.WriteElementString("link", rssNode.Link);
                xmlTw.WriteElementString("description", rssNode.Description);
                xmlTw.WriteElementString("pubDate", rssNode.PubDate.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"));
                xmlTw.WriteElementString("copyright", rssNode.Copyright);
                xmlTw.WriteElementString("language", rssNode.Language);
                xmlTw.WriteElementString("ttl", rssNode.Ttl);

                foreach (var item in rssNode.Items)
                {
                    xmlTw.WriteStartElement("item");
                    xmlTw.WriteElementString("title", item.Title);
                    xmlTw.WriteElementString("link", item.Link);
                    xmlTw.WriteElementString("description", item.Description);

                    xmlTw.WriteStartElement("guid");
                    xmlTw.WriteAttributeString("isPermaLink", "false");
                    xmlTw.WriteString(item.Guid);
                    xmlTw.WriteEndElement();

                    xmlTw.WriteEndElement();
                }
                xmlTw.WriteEndElement();
                xmlTw.WriteEndElement();
                xmlTw.WriteEndDocument();
                xmlTw.Flush();
            }
            return builder;
        }

        public XDocument SitemapDocumentGenerate(IEnumerable<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = this.XNamespaceDict[XmlNamespaceEnum.Xmlns];
            XNamespace xmlnsNews = this.XNamespaceDict[XmlNamespaceEnum.News];
            XNamespace xmlnsImage = this.XNamespaceDict[XmlNamespaceEnum.Image];
            XNamespace xmlnsXsi = this.XNamespaceDict[XmlNamespaceEnum.Xsi];
            XNamespace xsiSchemaLocation = this.XNamespaceDict[XmlNamespaceEnum.SchemaLocation];

            XElement root = new XElement(xmlns + "urlset", new XAttribute(XNamespace.Xmlns + "news", xmlnsNews),
                            new XAttribute(XNamespace.Xmlns + "image", xmlnsImage), new XAttribute(XNamespace.Xmlns + "xsi", xmlnsXsi),
                            new XAttribute(xmlnsXsi + "schemaLocation", xsiSchemaLocation));

            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(xmlns + "url", new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                    sitemapNode.Frequency == null ? null : new XElement(xmlns + "changefreq", sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                    sitemapNode.News == null ? null : this.GenerateNewsNode(sitemapNode.News),
                    sitemapNode.Image == null ? null : this.GenerateImageNode(sitemapNode.Image));
                root.Add(urlElement);
            }
            XDocument document = new XDocument(root);
            return document;
        }

        public XDocument SitemapIndexDocumentGenerate(IEnumerable<string> urls)
        {
            XNamespace xmlns = this.XNamespaceDict[XmlNamespaceEnum.Xmlns];
            XElement root = new XElement(xmlns + "sitemapindex");

            foreach (var item in urls)
            {
                XElement urlElement = new XElement(xmlns + "sitemap", new XElement(xmlns + "loc", Uri.EscapeUriString($"{item}")));
                root.Add(urlElement);
            }
            XDocument document = new XDocument(root);
            return document;
        }

        private XNamespace GetXNamespace(string url)
        {
            return XNamespace.Get(url);
        }

        private Dictionary<XmlNamespaceEnum, XNamespace> GenerateXNamespace()
        {
            var temp = new Dictionary<XmlNamespaceEnum, XNamespace>
            {
                { XmlNamespaceEnum.Xmlns, GetXNamespace("http://www.sitemaps.org/schemas/sitemap/0.9")},
                { XmlNamespaceEnum.News, GetXNamespace("http://www.google.com/schemas/sitemap-news/0.9")},
                { XmlNamespaceEnum.Image, GetXNamespace("http://www.google.com/schemas/sitemap-image/1.1")},
                { XmlNamespaceEnum.Xsi, GetXNamespace("http://www.w3.org/2001/XMLSchema-instance")},
                { XmlNamespaceEnum.SchemaLocation, GetXNamespace("http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd")},
            };
            return temp;
        }

        private XElement GenerateNewsNode(NewsNode newsNode)
        {
            var xmlnsNews = this.XNamespaceDict[XmlNamespaceEnum.News];
            var temp = new XElement(xmlnsNews + "news",
                    new XElement(xmlnsNews + "publication",
                        new XElement(xmlnsNews + "name", newsNode.Name),
                               new XElement(xmlnsNews + "language", newsNode.Language)),
                           new XElement(xmlnsNews + "publication_date", newsNode.PublicationDate.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                           new XElement(xmlnsNews + "title", newsNode.Title));
            return temp;
        }

        private XElement GenerateImageNode(ImageNode imageNode)
        {
            var xmlnsImage = this.XNamespaceDict[XmlNamespaceEnum.Image];
            var temp = new XElement(xmlnsImage + "image",
                    new XElement(xmlnsImage + "loc", imageNode.Url),
                           new XElement(xmlnsImage + "title", imageNode.Title));
            return temp;
        }
    }
}
