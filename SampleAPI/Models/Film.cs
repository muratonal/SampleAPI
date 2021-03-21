using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Models
{
    public class Film
    {
        public Film()
        {
            FilmYorums = new HashSet<FilmYorum>();
        }
        [Key]
        public int Id { get; set; }
        public string FilmAdi { get; set; }
        public string Konu { get; set; }
        public string Dil { get; set; }
        public virtual ICollection<FilmYorum> FilmYorums { get; set; }
    }
}
