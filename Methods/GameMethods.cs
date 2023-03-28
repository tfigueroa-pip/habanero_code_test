using HabaneroCodeTest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace HabaneroCodeTest.Methods
{
    public class GameMethods
    {
        //HttpClient client = new HttpClient();

        private const string APIKeyGames = "A50C2BDA-4ACF-42E8-AE22-971A9A9F47C7";
        private const string BrandIdGames = "0d51cf4c-1d02-e711-80d9-000d3a802d1d";
        private const string URLGetGames = "https://ws-test.insvr.com/jsonapi/GetGames";
        private string HostName = Dns.GetHostName();
        public JObject JsonObject;
        public string ImgEndpoint = "https://app-test.insvr.com/img/";
        public string GameEndpoint = "https://app-test.insvr.com/games/";
        public List<Game> Games { get; set; }
        public HttpClient client = new HttpClient();
        public class GamesResponse
        {
            public List<Game> Games{ get; set; }
        }

        //public void LoadJson() {

        //    // read JSON directly from a file
        //    using (StreamReader file = File.OpenText(@"C:\TIM\dev\HabaneroCodeTest\Content\response.json"))
        //    using (JsonTextReader reader = new JsonTextReader(file))
        //    {
        //        JObject o = (JObject)JToken.ReadFrom(reader);
        //        JsonObject = o;
        //    }
        //    var CreateListOfGames = new List<Game> { };
        //    foreach (JObject g in JsonObject["Games"])
        //    {
        //        CreateListOfGames.Add(
        //            new Game
        //            {
        //                Id = g["BrandGameId"].ToString(),
        //                KeyName = g["KeyName"].ToString(), 
        //                Name = g["Name"].ToString(),
        //                NameCN = g["TranslatedNames"][2]["Translation"].ToString(),
        //                Logo = $"{ImgEndpoint}circle/400/{g["KeyName"]}.png",
        //                PlayLink = $"{GameEndpoint}?brandid={BrandIdGames}&keyname={g["KeyName"]}&mode=fun&lobbyurl={HostName}",
        //            }
        //        );
        //    }
        //    Games = CreateListOfGames;

        //}

        public class RequestHaba
        {
            public string BrandId { get; set; }
            public string APIKey { get; set; }
        }

        public async Task<string> CallHaba(string url, string brandId, string apiKey)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var httpCall = new HttpClient();
            var reqModel = new RequestHaba()
            {
                BrandId = brandId,
                APIKey = apiKey
            };

            var content = JsonConvert.SerializeObject(reqModel);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = httpCall.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        public List<Game> ListOfGames()
        {
            var callHaba = CallHaba(URLGetGames, BrandIdGames, APIKeyGames);
            var items = JsonConvert.DeserializeObject<GamesResponse>(callHaba.Result);

            var createListOfGames = new List<Game>();

            foreach(var g in items.Games)
            {
                var r = "";
                createListOfGames.Add(
                    new Game
                    {
                        BrandGameId = g.BrandGameId,
                        Name = g.Name,
                        KeyName = g.KeyName,
                        TranslatedNames = g.TranslatedNames,
                        Logo = $"{ImgEndpoint}circle/400/{g.KeyName}.png",
                        PlayLink = $"{GameEndpoint}?brandid={BrandIdGames}&keyname={g.KeyName}&mode=fun&lobbyurl={HostName}",
                    }
                );
                //TODO
            }


            return createListOfGames;
        }
    }
}