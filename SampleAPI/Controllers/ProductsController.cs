using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleAPI.Data;
using SampleAPI.Models;
using SQLitePCL;

namespace SampleAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;
        public ProductsController(DataContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet("verilericek")]
        public async Task<ActionResult> VerileriCek()
        {
            
            var model = await new ApiService<MovieApiModel>().GetRequest();
            List<Film> films = new List<Film>();
            if (model != null && model.MovieModels.Count > 0)
            {
                foreach (var movieModel in model.MovieModels)
                {
                    films.Add(new Film() { Dil = movieModel.Language, FilmAdi = movieModel.Title, Konu = movieModel.Overview });
                }

                await _context.Films.AddRangeAsync(films);
                int id = await _context.SaveChangesAsync(); 
            }
            return Ok();
        } 
        
    }
}
