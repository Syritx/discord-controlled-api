using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Text.Json;

namespace api.src.disc.http {

    class Https {

        public enum HttpType {
            PostRequest,
            GetRequest
        }

        static HttpClient client = new HttpClient();

        static string IP, PORT, POST_EXTENSION, GET_EXTENSION, ADDR;

        public static void Start() {
            string JSON = File.ReadAllText("src/api/api.json");
            IP = JsonDocument.Parse(JSON).RootElement.GetProperty("addr").ToString();
            PORT = JsonDocument.Parse(JSON).RootElement.GetProperty("port").ToString();

            POST_EXTENSION = JsonDocument.Parse(JSON).RootElement.GetProperty("posturl").ToString();
            GET_EXTENSION = JsonDocument.Parse(JSON).RootElement.GetProperty("statusurl").ToString();

            ADDR = "http://"+IP+":"+PORT+"/";
            Console.WriteLine(ADDR);
        }

        public static async Task<string> PreformHttpRequest(HttpType type, string cmd) {

            if (type == HttpType.PostRequest) {

                var req = new Dictionary<string, string> {
                    {"cmd", cmd}
                };

                var content = new FormUrlEncodedContent(req);
                var response = await client.PostAsync(ADDR+POST_EXTENSION, content);
                Console.WriteLine(response);
                return "Successful POST request";
            }

            else {
                var response = await client.GetAsync(ADDR+GET_EXTENSION);
                return await response.Content.ReadAsStringAsync();
            }

            return "Unsuccessful HTTP request";
        }
    }
}