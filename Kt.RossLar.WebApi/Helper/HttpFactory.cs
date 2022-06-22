using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace HelperTools
{
    public class HttpFactoryOwner : IHttpFactoryOwner
    {

        public  async Task<string> PostFactory(List<KeyValuePair<string, string>> Json, string requestUri)
        {                       
            string responseString = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    /*using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri))
                    {
                        var PostBody = new MultipartFormDataContent();
                        for (int x = 0; x < Json.Count; x++)
                        {
                            PostBody.Add(new StringContent(Json[x].Value.ToString()), Json[x].Key.ToString());
                        }
                        httpRequest.Content = PostBody;
                        using (var httpResponse = await client.PostAsync(requestUri,httpRequest.Content))
                        {
                            //httpResponse.EnsureSuccessStatusCode();
                            LogHelper.WriteLog($"通行记录上传 httpResponse==>{JsonConvert.SerializeObject(httpResponse)}");                            
                        }
                    }*/
                    /*var PostBody = new MultipartFormDataContent();
                    for (int x = 0; x < Json.Count; x++)
                    {
                        PostBody.Add(new StringContent(Json[x].Value), Json[x].Key);
                    }*/
                    var content = new FormUrlEncodedContent(Json);                    
                    var response =  client.PostAsync(requestUri, content).Result;
                    responseString = await response.Content.ReadAsStringAsync();
                    LogHelper.WriteLog($"完成 content==>{responseString}");
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("PostFactory Err", ex);
            }
            return responseString;
        }        
        // <summary>
        // Get请求数据
        // <para>最终以url参数的方式提交</para>
        // </summary>
        // <param name="parameters">参数字典,可为空</param>
        // <param name="requestUri">例如/api/Files/UploadFile</param>
        // <returns></returns> 
        public  async Task<string> GetFactory(Dictionary<string, string> parameters, string requestUri)
        {
            string responseString = string.Empty;
            //拼接地址
            try
            {
                if (parameters != null)
                {
                    var strParam = string.Join("&", parameters.Select(o => o.Key + "=" + o.Value));
                    requestUri = string.Concat(requestUri, '?', strParam);
                }
                using (var client = new HttpClient())
                {
                    responseString = client.GetStringAsync($"{requestUri}").Result;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Post Err",ex);
            }
            return  responseString;
        }
        public  async Task<RestResponse> HttpWebRequestPost(string url, Dictionary<string,object> param)
        {
            RestApi Re = new RestApi();
            var Req = Re.CreateRequest(url, Method.POST);
            Req.AddHeader("Content-Type", "multipart/form-data");
            foreach (var item in param)
            {
                Req.AddParameter(item.Key, item.Value);
            }
            LogHelper.InfoLog($"{JsonConvert.SerializeObject(Req)}");
            var result = await Re.SendAsync(Req);
            return result;
        }
    }
}
