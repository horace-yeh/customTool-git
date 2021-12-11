using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIHelper
{
    public class HttpClientAsyncTool
    {
        /// <summary>
        /// API呼叫GET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">API Url</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string url)
        {
            var client = this.GenerateHttpClinet();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            return await this.HttpClientGetAsync<T>(client, url);
        }

        /// <summary>
        /// API呼叫Get (Cookie)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseUrl">cookieDomain</param>
        /// <param name="cookieDic">cookie</param>
        /// <param name="url">API Url</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string url, string baseUrl, Dictionary<string, string> cookieDic)
        {
            var client = this.GenerateHttpClinet(baseUrl, cookieDic);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            return await this.HttpClientGetAsync<T>(client, url);
        }

        /// <summary>
        /// 取得Url檔案
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<byte[]> GetFileAsync(string url)
        {
            var client = this.GenerateHttpClinet();
            byte[] fileBytes = await client.GetByteArrayAsync(url);
            return fileBytes;
        }

        /// <summary>
        /// API呼叫POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string url, object jsonObj)
        {
            var client = this.GenerateHttpClinet();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var content = new StringContent(JsonConvert.SerializeObject(jsonObj), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
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
        /// HttpClient Call Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<T> HttpClientGetAsync<T>(HttpClient client, string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        #endregion Other

    }
}
