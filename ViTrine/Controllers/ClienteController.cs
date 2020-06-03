using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViTrine.Data;
using ViTrine.Models;

namespace ViTrine.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext ctx;

        public ClienteController(ApplicationDbContext context)
        {
            ctx = context;
        }

        [HttpGet]
        public async Task<IActionResult> Home(string searchString, int? pageNumber, string currentFilter)
        {
            ViewData["CurrentFilter"] = searchString;

            var lojas = from p in ctx.Lojas select p;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                lojas = lojas.Where(l => l.NomeLoja.Contains(searchString)
                || l.CategoriaLoja.Contains(searchString)
                || l.CidadeLoja.Contains(searchString));
            }

            int pageSize = 10;

            return View(await PaginatedList<Loja>.CreateAsync(lojas.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}
