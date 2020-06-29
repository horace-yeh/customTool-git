using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIHelper
{
    public class WebClientTool
    {
        #region 回傳字串版本

        public string Delete(string url)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //client.Headers.Add("authorization", "token {apitoken}");
            var responseBody = client.UploadString(url, "DELETE", "");
            return responseBody;
        }

        public string Get(string url)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //client.Headers.Add("authorization", "token {apitoken}");
            var responseBody = client.DownloadString(url);
            return responseBody;
        }

        public string Post(string url, string json)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //client.Headers.Add("authorization", "token {apitoken}");
            var response = client.UploadData(url, "POST", Encoding.UTF8.GetBytes(json));
            var responseBody = Encoding.UTF8.GetString(response);
            return responseBody;
        }

        public string Put(string url, string json)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //client.Headers.Add("authorization", "token {apitoken}");
            var response = client.UploadData(url, "PUT", Encoding.UTF8.GetBytes(json));
            var responseBody = Encoding.UTF8.GetString(response);
            return responseBody;
        }

        #endregion 回傳字串版本

        #region 泛型版本

        public T Delete<T>(string url)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //client.Headers.Add("authorization", "token {apitoken}");
            var responseBody = client.UploadString(url, "DELETE", "");
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public T Get<T>(string url)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //client.Headers.Add("authorization", "token {apitoken}");
            var responseBody = client.DownloadString(url);
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public T Post<T>(string url, string json)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //client.Headers.Add("authorization", "token {apitoken}");
            var response = client.UploadData(url, "POST", Encoding.UTF8.GetBytes(json));
            var responseBody = Encoding.UTF8.GetString(response);
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public T Put<T>(string url, string json)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //client.Headers.Add("authorization", "token {apitoken}");
            var response = client.UploadData(url, "PUT", Encoding.UTF8.GetBytes(json));
            var responseBody = Encoding.UTF8.GetString(response);
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        #endregion 泛型版本

        public string ObjectToJson(object data)
        {
            string json = JsonConvert.SerializeObject(data);
            return json;
        }
    }
}