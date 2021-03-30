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
        /// API呼叫Get cookie版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseUrl">cookieDomain</param>
        /// <param name="cookieDic">cookie</param>
        /// <param name="url"></param>
        /// <returns></returns>
        public T Get<T>(string url, string baseUrl, Dictionary<string, string> cookieDic)
        {
            var cookieContainer = this.GenerateCookieContainer(baseUrl, cookieDic);
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// API呼叫POST Form特規版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="ContentDic">FormContent</param>
        /// <returns></returns>
        public T PostForm<T>(string url, Dictionary<string, string> ContentDic)
        {
            HttpClient httpClient = new HttpClient();
            var form = GenerateMultipartFormDataContent(ContentDic);
            HttpResponseMessage response = httpClient.PostAsync(url, form).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// API呼叫POST Form特規版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="ContentDic">FormContent</param>
        /// <param name="fileDic">UploadFile</param>
        /// <returns></returns>
        public T PostForm<T>(string url, Dictionary<string, string> ContentDic, Dictionary<string, HttpPostedFileBase> fileDic)
        {
            HttpClient httpClient = new HttpClient();
            var form = GenerateMultipartFormDataContent(ContentDic, fileDic);
            HttpResponseMessage response = httpClient.PostAsync(url, form).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// API呼叫POST Form特規版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="baseUrl">cookieDomain</param>
        /// <param name="cookieDic">cookie</param>
        /// <param name="ContentDic">FormContent</param>
        /// <param name="fileDic">UploadFile</param>
        /// <returns></returns>
        public T PostForm<T>(string url, string baseUrl, Dictionary<string, string> cookieDic, Dictionary<string, string> ContentDic, Dictionary<string, HttpPostedFileBase> fileDic)
        {
            var cookieContainer = this.GenerateCookieContainer(baseUrl, cookieDic);
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient httpClient = new HttpClient(handler);
            var form = GenerateMultipartFormDataContent(ContentDic, fileDic);
            HttpResponseMessage response = httpClient.PostAsync(url, form).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
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
        /// 產生特規FormData，參數有增加特別處理不一定適用所有API
        /// </summary>
        /// <param name="ContentDic"></param>
        /// <returns></returns>
        private MultipartFormDataContent GenerateMultipartFormDataContent(Dictionary<string, string> ContentDic)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            foreach (var prop in ContentDic)
            {
                var content = this.GenerateStringContent(prop.Value);
                var name = this.FormatContentName(prop.Key);
                form.Add(content, name);
            }
            return form;
        }

        /// <summary>
        /// 產生特規FormData含檔案，參數有增加特別處理不一定適用所有API
        /// </summary>
        /// <param name="jsonObj">文字資料</param>
        /// <param name="fileDic">檔案資料</param>
        /// <returns></returns>
        private MultipartFormDataContent GenerateMultipartFormDataContent(Dictionary<string, string> ContentDic, Dictionary<string, HttpPostedFileBase> fileDic)
        {
            var form = this.GenerateMultipartFormDataContent(ContentDic);
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

        #endregion Other

    }
}
