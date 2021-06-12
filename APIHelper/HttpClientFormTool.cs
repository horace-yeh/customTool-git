using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace APIHelper
{
    public class HttpClientFormTool
    {
        /// <summary>
        /// API呼叫GET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">API Url</param>
        /// <returns></returns>
        public T Get<T>(string url)
        {
            var client = this.GenerateHttpClinet();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            return this.HttpClientGet<T>(client, url);
        }

        /// <summary>
        /// API呼叫Get (Cookie)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseUrl">cookieDomain</param>
        /// <param name="cookieDic">cookie</param>
        /// <param name="url">API Url</param>
        /// <returns></returns>
        public T Get<T>(string url, string baseUrl, Dictionary<string, string> cookieDic)
        {
            var client = this.GenerateHttpClinet(baseUrl, cookieDic);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            return this.HttpClientGet<T>(client, url);
        }

        /// <summary>
        /// API呼叫POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public T Post<T>(string url, object jsonObj)
        {
            var client = this.GenerateHttpClinet();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var content = new StringContent(JsonConvert.SerializeObject(jsonObj), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// API呼叫POST FormData(文字)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">API Url</param>
        /// <param name="contentDic">FormContent</param>
        /// <returns></returns>
        public T PostForm<T>(string url, Dictionary<string, string> contentDic)
        {
            var client = this.GenerateHttpClinet();
            var form = this.GenerateMultipartFormDataContent(contentDic);
            return this.PostMultipartForm<T>(client, form, url);
        }

        /// <summary>
        /// API呼叫POST FormData(文字、檔案)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">API Url</param>
        /// <param name="contentDic">FormContent</param>
        /// <param name="fileDic">UploadFile</param>
        /// <returns></returns>
        public T PostForm<T>(string url, Dictionary<string, string> contentDic, Dictionary<string, HttpPostedFileBase> fileDic)
        {
            var client = this.GenerateHttpClinet();
            var form = this.GenerateMultipartFormDataContent(contentDic, fileDic);
            return this.PostMultipartForm<T>(client, form, url);
        }

        /// <summary>
        /// API呼叫POST FormData(Cookie、文字)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">API Url</param>
        /// <param name="baseUrl">cookieDomain</param>
        /// <param name="cookieDic">cookie</param>
        /// <param name="contentDic">FormContent</param>
        /// <returns></returns>
        public T PostForm<T>(string url, string baseUrl, Dictionary<string, string> cookieDic, Dictionary<string, string> contentDic)
        {
            var client = this.GenerateHttpClinet(baseUrl, cookieDic);
            var form = this.GenerateMultipartFormDataContent(contentDic);
            return this.PostMultipartForm<T>(client, form, url);
        }

        /// <summary>
        /// API呼叫POST FormData(Cookie、文字、檔案)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">API Url</param>
        /// <param name="baseUrl">cookieDomain</param>
        /// <param name="cookieDic">cookie</param>
        /// <param name="contentDic">FormContent</param>
        /// <param name="fileDic">UploadFile</param>
        /// <returns></returns>
        public T PostForm<T>(string url, string baseUrl, Dictionary<string, string> cookieDic, Dictionary<string, string> contentDic, Dictionary<string, HttpPostedFileBase> fileDic)
        {
            var client = this.GenerateHttpClinet(baseUrl, cookieDic);
            var form = GenerateMultipartFormDataContent(contentDic, fileDic);
            return this.PostMultipartForm<T>(client, form, url);
        }

        #region Other

        /// <summary>
        /// 產生CookieContainer
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="cookieDic"></param>
        /// <returns></returns>
        private CookieContainer GenerateCookieContainer(string baseUrl, Dictionary<string, string> cookieDic)
        {
            var cookieContainer = new CookieContainer();
            foreach (var item in cookieDic)
            {
                cookieContainer.Add(new Uri(baseUrl), new Cookie(item.Key, item.Value));
            }
            return cookieContainer;
        }

        /// <summary>
        /// 產生特規FormData(文字)
        /// </summary>
        /// <param name="contentDic"></param>
        /// <returns></returns>
        private MultipartFormDataContent GenerateMultipartFormDataContent(Dictionary<string, string> contentDic)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            foreach (var prop in contentDic)
            {
                var content = this.GenerateStringContent(prop.Value);
                var name = this.FormatContentName(prop.Key);
                form.Add(content, name);
            }
            return form;
        }

        /// <summary>
        /// 產生特規FormData(文字、檔案)
        /// </summary>
        /// <param name="contentDic"></param>
        /// <param name="fileDic"></param>
        /// <returns></returns>
        private MultipartFormDataContent GenerateMultipartFormDataContent(Dictionary<string, string> contentDic, Dictionary<string, HttpPostedFileBase> fileDic)
        {
            var form = this.GenerateMultipartFormDataContent(contentDic);
            foreach (var file in fileDic)
            {
                if (file.Value == null)
                {
                    continue;
                }
                form.Add(this.GenerateStreamContent(file.Value, file.Key));
            }
            return form;
        }

        /// <summary>
        /// 格式化Content名稱
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string FormatContentName(string name)
        {
            //REF: https://dotblogs.com.tw/rainmaker/2016/08/16/093026
            string quote = "\"";
            var temp = $"{quote}{name}{quote}";
            return temp;
        }

        /// <summary>
        /// 產生檔案上傳用StringContent
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private StringContent GenerateStringContent(string content)
        {
            var tempContent = string.IsNullOrWhiteSpace(content) ? "" : content;
            var temp = new StringContent(tempContent, Encoding.UTF8);
            return temp;
        }

        /// <summary>
        /// 產生檔案上傳用StreamContent
        /// </summary>
        /// <param name="file"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private StreamContent GenerateStreamContent(HttpPostedFileBase file, string key)
        {
            var fileContent = new StreamContent(file.InputStream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = this.FormatContentName(key),
                FileName = this.FormatContentName(file.FileName)
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            return fileContent;
        }

        /// <summary>
        /// HttpClient 工廠
        /// </summary>
        /// <returns></returns>
        private HttpClient GenerateHttpClinet()
        {
            return new HttpClient();
        }

        /// <summary>
        /// HttpClient 工廠
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="cookieDic"></param>
        /// <returns></returns>
        private HttpClient GenerateHttpClinet(string baseUrl, Dictionary<string, string> cookieDic)
        {
            var cookieContainer = this.GenerateCookieContainer(baseUrl, cookieDic);
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient httpClient = new HttpClient(handler);
            return httpClient;
        }

        /// <summary>
        /// Post FormData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="formContent"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private T PostMultipartForm<T>(HttpClient client, MultipartFormDataContent formContent, string url)
        {
            HttpResponseMessage response = client.PostAsync(url, formContent).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// HttpClient Call Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private T HttpClientGet<T>(HttpClient client, string url)
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        #endregion Other

    }
}
