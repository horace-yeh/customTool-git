using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace APIHelper
{
    public class HttpClientFormAsyncTool
    {
        /// <summary>
        /// API呼叫POST FormData(文字)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">API Url</param>
        /// <param name="contentDic">FormContent</param>
        /// <returns></returns>
        public async Task<T> PostFormAsync<T>(string url, Dictionary<string, string> contentDic)
        {
            var client = this.GenerateHttpClinet();
            var form = this.GenerateMultipartFormDataContent(contentDic);
            return await this.PostMultipartFormAsync<T>(client, form, url);
        }

        /// <summary>
        /// API呼叫POST FormData(文字、檔案)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">API Url</param>
        /// <param name="contentDic">FormContent</param>
        /// <param name="fileDic">UploadFile</param>
        /// <returns></returns>
        public async Task<T> PostFormAsync<T>(string url, Dictionary<string, string> contentDic, Dictionary<string, HttpPostedFileBase> fileDic)
        {
            var client = this.GenerateHttpClinet();
            var form = this.GenerateMultipartFormDataContent(contentDic, fileDic);
            return await this.PostMultipartFormAsync<T>(client, form, url);
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
        public async Task<T> PostFormAsync<T>(string url, string baseUrl, Dictionary<string, string> cookieDic, Dictionary<string, string> contentDic)
        {
            var client = this.GenerateHttpClinet(baseUrl, cookieDic);
            var form = this.GenerateMultipartFormDataContent(contentDic);
            return await this.PostMultipartFormAsync<T>(client, form, url);
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
        public async Task<T> PostFormAsync<T>(string url, string baseUrl, Dictionary<string, string> cookieDic, Dictionary<string, string> contentDic, Dictionary<string, HttpPostedFileBase> fileDic)
        {
            var client = this.GenerateHttpClinet(baseUrl, cookieDic);
            var form = GenerateMultipartFormDataContent(contentDic, fileDic);
            return await this.PostMultipartFormAsync<T>(client, form, url);
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
        private async Task<T> PostMultipartFormAsync<T>(HttpClient client, MultipartFormDataContent formContent, string url)
        {
            HttpResponseMessage response = await client.PostAsync(url, formContent);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        #endregion Other

    }
}
