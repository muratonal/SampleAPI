using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Dto
{
    public class BaseApiModel
    {
        public object Data { get; set; }
        public bool Sonuc { get; set; }
    }

    public class FilmApiModel
    {
        public int Id { get; set; }
        public string FilmAdi { get; set; }
        public string Konu { get; set; }
        public string Dil { get; set; }
    }

    public class FilmYorumModel
    {
        public string Yorum { get; set; }
        [Range(1, 10)]
        public int Puan { get; set; }
        public int FilmId { get; set; }
    }

    public class FilmInfoModel
    {
        public int Id { get; set; }
        public string FilmAdi { get; set; }
        public string Konu { get; set; }
        public string Dil { get; set; }
        public List<FilmInfoYorumModel> YorumModels { get; set; }
        public decimal OrtalamaPuan { get; set; }
    }

    public class FilmInfoYorumModel
    {
        public string Yorum { get; set; }
        public int Puan { get; set; }
    }

    public class FilmOneriModel
    {
        public int FilmId { get; set; }
        public string Email { get; set; }
    }
}
