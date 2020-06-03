using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViTrine.Data;
using ViTrine.Models;

namespace ViTrine.Controllers
{
    public class LojaController : Controller
    {
        private readonly ApplicationDbContext ctx;
        private readonly IWebHostEnvironment webHostEnvironment;

        public LojaController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            ctx = context;
            webHostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string searchString, int? pageNumber, string currentFilter)
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

            return View(await PaginatedList<Loja>.CreateAsync(lojas.Where((c => c.UserId == User.Identity.Name)).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View(new LojaView());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LojaView c)
        {
            if (ModelState.IsValid)
            {
                string UrlImagem = Upload(c);

                Loja l = new Loja
                {
                    UrlFotoLoja = UrlImagem,
                    NomeLoja = c.NomeLoja,
                    CategoriaLoja = c.CategoriaLoja,
                    TelefoneLoja = c.TelefoneLoja,
                    CidadeLoja = c.CidadeLoja,
                    EnderecoLoja = c.EnderecoLoja,
                    UserId = User.Identity.Name
                };

                ctx.Add(l);
                await ctx.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(c);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Read(Guid Id)
        {
            Loja l = await ctx.Lojas.FirstOrDefaultAsync(c => c.LojaId == Id);
            return View(l);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(Guid Id)
        {
            Loja l = await ctx.Lojas.FirstOrDefaultAsync(c => c.LojaId == Id);
            return View(l);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Loja u, Guid Id)
        {
            if (ModelState.IsValid)
            {

                Loja l = await ctx.Lojas.FirstOrDefaultAsync(c => c.LojaId == Id);

                l.NomeLoja = u.NomeLoja;
                l.CategoriaLoja = u.CategoriaLoja;
                l.TelefoneLoja = u.TelefoneLoja;
                l.CidadeLoja = u.CidadeLoja;
                l.EnderecoLoja = u.EnderecoLoja;
                l.UserId = User.Identity.Name;

                ctx.Update(l);
                await ctx.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(u);
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid Id)
        {
            Loja l = await ctx.Lojas.FirstOrDefaultAsync(c => c.LojaId == Id);
            ctx.Lojas.Remove(l);
            await ctx.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        private string Upload(LojaView l)
        {
            string UrlImagem = null;

            if (l.UrlFotoLoja != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                UrlImagem = Guid.NewGuid().ToString() + "_" + l.UrlFotoLoja.FileName;
                string filePath = Path.Combine(uploadsFolder, UrlImagem);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    l.UrlFotoLoja.CopyTo(fileStream);
                }
            }
            return UrlImagem;
        }
    }
}
