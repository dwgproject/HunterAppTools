using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Src.Tools
{
    public static class WinApiConnector
    {
        private static HttpClient client = new HttpClient();

        public async static Task<ConnectorResult<TResult>> RequestPost<TData, TResult>(TData data) where TResult : new(){
            string output = JsonConvert.SerializeObject(data);
            var content = new StringContent(output, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(Parameters.AddRoleUri, content);
                if (response.IsSuccessStatusCode){

                }else{

                }
            }
            catch(ArgumentNullException ex)
            {
                
            }
            catch(HttpRequestException ex)
            {

            }
            var t = new TResult();
            return new ConnectorResult();
        }

        // public static ConnectorResult<TResult> RequestPut<TData, TResult>(TData data){

        // }

        // public static ConnectorResult<TResult> RequestDelete<TData, TResult>(TData data){

        // }

        public async static Task<ConnectorResult<TResult>> RequestGet<TData, TResult>(TData data){
            try
            {
                HttpResponseMessage response = await client.GetAsync("/Api/Roles");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<TResult>(content);
                    return result;
                }
            }
            catch(ArgumentNullException ex)
            {
                
            }
            catch(HttpRequestException ex)
            {

            }
            return 
        }
    }
}