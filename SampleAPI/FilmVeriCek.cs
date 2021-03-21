using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Quartz;
using SampleAPI.Data;
using SampleAPI.Models;

namespace SampleAPI
{
    public class FilmVeriCek : IJob
    {
        
        public async Task Execute(IJobExecutionContext context)
        {

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:5001/api/products/verilericek");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                }
            }


        }
    }
}
