using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SampleAPI.Models
{
    public class MovieApiModel
    {
        [JsonProperty("results")]
        public List<MovieModel> MovieModels { get; set; }
    }

    public class MovieModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("original_language")]
        public string Language { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("overview")]
        public string Overview { get; set; }

    }
}
