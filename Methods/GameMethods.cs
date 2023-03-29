using HabaneroCodeTest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HabaneroCodeTest.Methods
{
    public class GameMethods
    {
        //HttpClient client = new HttpClient();

        private const string APIKeyGames = "A50C2BDA-4ACF-42E8-AE22-971A9A9F47C7";
        private const string BrandIdGames = "0d51cf4c-1d02-e711-80d9-000d3a802d1d";
        private const string URLGetGames = "https://ws-test.insvr.com/jsonapi/GetGames";
        private string HostName = HttpContext.Current.Request.Url.Authority;
        public JObject JsonObject;
        public string ImgEndpoint = "https://app-test.insvr.com/img/";
        public string GameEndpoint = "https://app-test.insvr.com/games/";
        public List<Game> Games { get; set; }
        public HttpClient client = new HttpClient();
        public class GamesResponse
        {
            public List<Game> Games{ get; set; }
        }

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
                var GameData = new Game
                {
                    BrandGameId = g.BrandGameId,
                    Name = g.Name,
                    KeyName = g.KeyName,
                    TranslatedNames = g.TranslatedNames,
                    GameTypeName = g.GameTypeName,
                    DtAdded = g.DtAdded,
                    Logos = new Logo
                    {
                        CircleFlat = $"{ImgEndpoint}circleflat/400/{g.KeyName}.png",
                        Rectangle = $"{ImgEndpoint}rectangle/400/{g.KeyName}.png",
                        Square = $"{ImgEndpoint}square/400/{g.KeyName}.png",
                        OvalFlat = $"{ImgEndpoint}ovalflat/400/{g.KeyName}.png"
                    },
                    PlayLink = $"{GameEndpoint}?brandid={BrandIdGames}&keyname={g.KeyName}&mode=fun&lobbyurl={HostName}",
                };
                createListOfGames.Add(
                    GameData
                );
                //TODO
            }


            return createListOfGames;
        }
    }
}