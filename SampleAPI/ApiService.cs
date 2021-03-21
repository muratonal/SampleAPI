using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SampleAPI
{
    public class ApiService<T> where T : class, new()
    {
        private const int page = 1;
        private const int pageMax = 1000;
        public async Task<T> GetRequest()
        {
            int pageIndex = new Random().Next(page,pageMax);
            string url = "https://api.themoviedb.org/3/movie/popular?api_key=a7b10ac01a7ede7e712848abb3440675&language=tr-TR&page=" + pageIndex;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<T>(content);
                    Type type = typeof(T);
                    PropertyInfo prop = type.GetProperty("Success");
                    if (prop != null)
                    {
                        prop.SetValue(model, true);
                    }

                    return model;
                }


                return default;

            }
        }
    }
}
