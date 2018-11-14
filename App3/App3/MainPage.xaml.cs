using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Task<SQLObject> task = UpdateSwitchState(1);
            task.Wait();
            if (task != null)
            {
                string exitCode = task.Result.exitCode;
                string sqlResponse = task.Result.sqlResponse;
            }
        }

        public static async Task<SQLObject> UpdateSwitchState(int state)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://172.20.10.5:8080/testDB/UpdateSwitchState.php?state=" + state);
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
           
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();
                    SQLObject _sqlObject = JsonDeserializer(json);
                    string exitCode = _sqlObject.exitCode;
                    string sqlResponse = _sqlObject.sqlResponse;
                    return _sqlObject;
                }
                else
                {
                    return null;//Handle if cannot access update switch php site
                }
            
          
        }

        public static SQLObject JsonDeserializer(string json)
        {
            SQLObject results = JsonConvert.DeserializeObject<SQLObject>(json);
            return results;
        }
    }
}
