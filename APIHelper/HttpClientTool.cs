using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APIHelper
{
    //備註: RestSharp 套件(非原生 WebApi 呼叫)
    public class HttpClientTool
    {
        #region 泛型版本

        public async Task<T> Delete<T>(string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            HttpResponseMessage response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task<T> Get<T>(string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task<T> Post<T>(string url, string json)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task<T> Put<T>(string url, string json)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        #endregion 泛型版本

        #region 回傳字串版本

        public async Task<string> Delete(string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            HttpResponseMessage response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task<string> Get(string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task<string> Post(string url, string json)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task<string> Put(string url, string json)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        #endregion 回傳字串版本

        #region 特殊規格版本(呼叫某些老舊或嚴謹的API可以使用)

        /// <summary>
        /// Post MultipartForm 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public T PostForm<T>(string url, object jsonObj)
        {
            HttpClient httpClient = new HttpClient();
            var form = GenerateMultipartFormDataContent(jsonObj);
            HttpResponseMessage response = httpClient.PostAsync(url, form).Result;
            response.EnsureSuccessStatusCode();
            //httpClient.Dispose();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// 產生特規FormData
        /// </summary>
        /// <param name="jsonObj"></param>
        private MultipartFormDataContent GenerateMultipartFormDataContent(object jsonObj)
        {
            //REF: https://dotblogs.com.tw/rainmaker/2016/08/16/093026
            string quote = "\"";
            MultipartFormDataContent form = new MultipartFormDataContent();
            Type t = jsonObj.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach (var prop in props)
            {
                form.Add(new StringContent(prop.GetValue(jsonObj).ToString(), Encoding.UTF8), $"{quote}{prop.Name}{quote}");
            }
            return form;
        }

        #endregion 特殊規格版本(呼叫某些老舊或嚴謹的API可以使用)

        #region API 帶cookie版本

        /// <summary>
        /// API呼叫Get 帶cookie版本特規版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseUrl"></param>
        /// <param name="cookieDic"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public T GetHaveCookie<T>(string baseUrl, Dictionary<string, string> cookieDic, string url)
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

        #endregion API 帶cookie版本

        public string ObjectToJson(object data)
        {
            string json = JsonConvert.SerializeObject(data);
            return json;
        }
    }
}