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
    public class ProdutoController : Controller
    {
        private readonly ApplicationDbContext ctx;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProdutoController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            ctx = context;
            webHostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(Guid Id)
        {
            var produtos = await ctx.Produtos.Where(c => c.LojaId == Id).ToListAsync();

            ViewBag.LojaId = Id;

            return View(produtos);
        }

        [HttpGet]
        public IActionResult Create(Guid Id)
        {
            return View(new ProdutoView() { LojaId = Id } );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoView c, Guid Id)
        {
            if (ModelState.IsValid)
            {
                string UrlImagem = Upload(c);

                Produto p = new Produto
                {
                    UrlFotoProduto = UrlImagem,
                    LojaId = c.LojaId,
                    NomeProduto = c.NomeProduto,
                    CategoriaProduto = c.CategoriaProduto,
                    DescricaoProduto = c.DescricaoProduto,
                    PrecoProduto = c.PrecoProduto,
                    PromocaoProduto = c.PromocaoProduto
                };

                ctx.Add(p);
                await ctx.SaveChangesAsync();
                return RedirectToAction("Index", new { Id });
            }
            return View(c);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Read(Guid Id)
        {
            Produto p = await ctx.Produtos.FirstOrDefaultAsync(c => c.ProdutoId == Id);
            return View(p);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(Guid Id)
        {
            Produto p = await ctx.Produtos.FirstOrDefaultAsync(c => c.ProdutoId == Id);
            return View(p);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Produto u, Guid Id)
        {
            if (ModelState.IsValid)
            {
                Produto p = await ctx.Produtos.FirstOrDefaultAsync(c => c.ProdutoId == Id);

                p.NomeProduto = u.NomeProduto;
                p.CategoriaProduto = u.CategoriaProduto;
                p.DescricaoProduto = u.DescricaoProduto;
                p.PrecoProduto = u.PrecoProduto;
                p.PromocaoProduto = u.PromocaoProduto;

                ctx.Update(p);
                await ctx.SaveChangesAsync();
                return RedirectToAction("Index", new { p.LojaId });
            }
            return View(u);
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid Id)
        {
            Produto p = await ctx.Produtos.FirstOrDefaultAsync(c => c.ProdutoId == Id);
            ctx.Produtos.Remove(p);
            await ctx.SaveChangesAsync();

            return RedirectToAction("Index", new { p.LojaId });
        }

        [Authorize]
        private string Upload(ProdutoView l)
        {
            string UrlImagem = null;

            if (l.UrlFotoProduto != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Imagens");
                UrlImagem = Guid.NewGuid().ToString() + "_" + l.UrlFotoProduto.FileName;
                string filePath = Path.Combine(uploadsFolder, UrlImagem);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    l.UrlFotoProduto.CopyTo(fileStream);
                }
            }
            return UrlImagem;
        }
    }
}
