using HabaneroCodeTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HabaneroCodeTest.Methods
{
    public class GameMethods
    {
        //HttpClient client = new HttpClient();

        ObjectCache cache = MemoryCache.Default;
        private string APIKeyGames = ConfigurationManager.AppSettings["APIKeyGames"];
        private string BrandIdGames = ConfigurationManager.AppSettings["BrandIdGames"];
        private string URLGetGames = ConfigurationManager.AppSettings["URLGetGames"];
        private string ImgEndpoint = ConfigurationManager.AppSettings["ImgEndpoint"];
        private string GameEndpoint = ConfigurationManager.AppSettings["GameEndpoint"];
        private string HostName = HttpContext.Current.Request.Url.Authority;
        public List<Game> Games { get; set; }
        public class GamesResponse
        {
            public List<Game> Games { get; set; }
        }

        public class RequestHabaAPI
        {
            public string BrandId { get; set; }
            public string APIKey { get; set; }
        }

        public async Task<string> CallHaba(string url, string brandId, string apiKey)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var httpCall = new HttpClient();
            var reqModel = new RequestHabaAPI()
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

        public List<Game> ListOfGames(int? clearcache)
        {
            
            string cacheKey = "listOfGames";
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60.0)
            }; 
            if (clearcache == 1)
            {
                cache.Remove(cacheKey);
            }
            var itemsData = cache.Get(cacheKey);

            if (itemsData == null) 
            { 
                var callHaba = CallHaba(URLGetGames, BrandIdGames, APIKeyGames);
                itemsData = callHaba.Result;
                cache.Add(cacheKey, itemsData, cacheItemPolicy);
            }

            var items = JsonConvert.DeserializeObject<GamesResponse>(itemsData.ToString());
            var createListOfGames = new List<Game>();

            foreach(var g in items.Games)
            {
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
                    PlayLink = $"{GameEndpoint}?brandid={BrandIdGames}&keyname={g.KeyName}&mode=fun&lobbyurl=http%3a%2f%2f{HostName}%2f",
                };
                createListOfGames.Add(
                    GameData
                );
            }


            return createListOfGames;
        }
    }
}