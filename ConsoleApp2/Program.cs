using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task = UpdateSwitchState();
            task.Wait();
        }

        public static async Task UpdateSwitchState()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://localhost:8080/testDB/UpdateSwitchState.php?state=5");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

            }
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
        }
    }



}
