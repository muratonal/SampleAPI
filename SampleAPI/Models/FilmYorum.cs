using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Models
{
    public class FilmYorum
    {
        [Key]
        public int Id { get; set; }
        public string Yorum { get; set; }
        public int Puan { get; set; }
        public int FilmId { get; set; }
        [ForeignKey("FilmId")]
        public virtual Film Film { get; set; }

        public int KullaniciId { get; set; }
    }
}
