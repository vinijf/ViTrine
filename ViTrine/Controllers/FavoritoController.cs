using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViTrine.Data;
using ViTrine.Models;

namespace ViTrine.Controllers
{
    public class FavoritoController : Controller
    {
        private readonly ApplicationDbContext ctx;

        public FavoritoController(ApplicationDbContext context)
        {
            ctx = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string Id = User.Identity.Name;

            var favoritos = await ctx.Favoritos.Where(c => c.UserId == Id).ToListAsync();

            return View(favoritos);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Favorito f, Guid Id)
        {
            if (ModelState.IsValid)
            {
                f.UserId = User.Identity.Name;

                ctx.Favoritos.Add(f);
                await ctx.SaveChangesAsync();
            }
            return View(f);
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid Id)
        {
            Favorito f = await ctx.Favoritos.FirstOrDefaultAsync(c => c.FavoritoId == Id);
            ctx.Favoritos.Remove(f);
            await ctx.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
