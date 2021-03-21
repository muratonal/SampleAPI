using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SampleAPI.Data;
using SampleAPI.Dto;
using SampleAPI.Models;

namespace SampleAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly DataContext _context;

        public FilmController(DataContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet("getfilms")]
        public async Task<ActionResult> GetFilms(int pageIndex, int pageSize)
        {

            var films = await _context.Films.ToListAsync();
            films = films.OrderBy(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            List<FilmApiModel> apiModels = new List<FilmApiModel>();
            foreach (var film in films)
            {
                apiModels.Add(new FilmApiModel()
                {
                    FilmAdi = film.FilmAdi,
                    Konu = film.Konu,
                    Dil = film.Dil,
                    Id = film.Id
                });
            }

            return Ok(new BaseApiModel() { Data = apiModels, Sonuc = true });
        }
        [Authorize]
        [HttpPost("yorumekle")]
        public async Task<ActionResult> YorumEkle(FilmYorumModel model)
        {
            if (ModelState.IsValid)
            {
                var film = _context.Films.FirstOrDefault(x => x.Id == model.FilmId);
                if (film != null)
                {
                    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    FilmYorum yorum = new FilmYorum()
                    {
                        FilmId = film.Id,
                        Puan = model.Puan,
                        Yorum = model.Yorum,
                        KullaniciId = Convert.ToInt32(userId)
                    };
                    await _context.FilmYorums.AddAsync(yorum);
                    int id = await _context.SaveChangesAsync();
                    if (id > 0)
                    {
                        return Ok(new BaseApiModel() { Sonuc = true, Data = "Film yorum ekleme başarılı." });
                    }
                    return BadRequest(new BaseApiModel() { Sonuc = false, Data = "Veritabanı hatası alındı." });
                }
                return BadRequest(new BaseApiModel() { Sonuc = false, Data = "Film bulunamadı." });
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpGet("getfilm")]
        public async Task<ActionResult> GetFilm(int filmId)
        {
            var film = _context.Films.FirstOrDefault(x => x.Id == filmId);
            if (film == null)
            {
                return BadRequest(new BaseApiModel() { Sonuc = false, Data = "Film bulunamadı." });
            }
            string _userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(_userId);
            var yorums = _context.FilmYorums.Where(x => x.FilmId == filmId).ToList();
            FilmInfoModel model = new FilmInfoModel()
            {
                Konu = film.Konu,
                FilmAdi = film.FilmAdi,
                Dil = film.Dil,
                Id = filmId,
                OrtalamaPuan = yorums.Count > 0 ? yorums.Sum(x => x.Puan) / yorums.Count : 0
            };
            var userYorums = yorums.Where(x => x.KullaniciId == userId).ToList();
            model.YorumModels = userYorums.Select(x => new FilmInfoYorumModel() { Puan = x.Puan, Yorum = x.Yorum }).ToList();
            return Ok(model);
        }

        [Authorize]
        [HttpPost("filmoneri")]
        public async Task<ActionResult> FilmOner(FilmOneriModel model)
        {
            var film = _context.Films.FirstOrDefault(x => x.Id == model.FilmId);
            if (film == null)
            {
                return BadRequest(new BaseApiModel() { Sonuc = false, Data = "Film bulunamadı." });
            }
            string _userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(_userId);
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            string konu = string.Format("Merhaba {0} sizin için bu filmi öneriyor.", user.Name);
            string ileti = string.Format("Film Adı : {0}\nKonu : {1}\nDil : {2}", film.FilmAdi, film.Konu, film.Dil);
            if (MailSend(model.Email,konu,ileti))
            {
                return Ok(new BaseApiModel() {Sonuc = true, Data = "Mail gönderimi başarılı"});
            }

            return BadRequest(new BaseApiModel() {Sonuc = false, Data = "Gönderim başarısız kod içerisindeki mail ayarlarını girmeyi deneyiniz."});
        }

        private bool MailSend(string mail, string konu, string ileti)
        {
            string from = "";
            string host = "";
            string pass = "";
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from);

            msg.To.Add(mail);
            msg.Subject = konu;
            msg.Body = ileti;

            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = host;
            sc.EnableSsl = false;
            sc.Credentials = new NetworkCredential(from, pass);
            try
            {
                sc.Send(msg);
            }
            catch (Exception ex)
            { return false; }
            return true;
        }
    }
}
