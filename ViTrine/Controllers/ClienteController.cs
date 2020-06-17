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
                || l.CidadeLoja.Contains(searchString)
                || l.EnderecoLoja.Contains(searchString));
            }

            int pageSize = 12;

            return View(await PaginatedList<Loja>.CreateAsync(lojas.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Produto(Guid Id, string searchString, int? pageNumber, string currentFilter)
        {
            ViewData["CurrentFilter"] = searchString;

            var produtos = from p in ctx.Produtos select p;

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
                produtos = produtos.Where(p => p.NomeProduto.Contains(searchString)
                || p.CategoriaProduto.Contains(searchString)
                || p.DescricaoProduto.Contains(searchString));
            }

            ViewBag.LojaId = Id;

            int pageSize = 12;

            return View(await PaginatedList<Produto>.CreateAsync(produtos.Where((c => c.LojaId == Id)).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Sobre(Guid Id)
        {
            Loja l = await ctx.Lojas.FirstOrDefaultAsync(c => c.LojaId == Id);
            return View(l);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Chat()
        {
            var chats = await ctx.Chats.Where(c => c.UserId == User.Identity.Name).ToListAsync();

            return View(chats);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Chat c, Guid Id)
        {
            if (ModelState.IsValid)
            {
                c.UserId = User.Identity.Name;

                ctx.Chats.Add(c);
                await ctx.SaveChangesAsync();
                return RedirectToAction("Send", new { id = c.ChatId });
            }
            return View(c);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Mensagem(Guid Id)
        {
            var mensagens = await ctx.Mensagens.Where(c => c.ChatId == Id).ToListAsync();

            ViewBag.ChatId = Id;

            return View(mensagens);
        }

                [HttpGet]
        [Authorize]
        public IActionResult Send(Guid Id)
        {
            return View(new Mensagem() { ChatId = Id });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(Mensagem m, Guid Id)
        {
            if (ModelState.IsValid)
            {
                m.DataMensagem = DateTime.Now;

                ctx.Mensagens.Add(m);
                await ctx.SaveChangesAsync();
                return RedirectToAction("Mensagem", new { id = Id });
            }
            return View(m);
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid Id)
        {
            Chat c = await ctx.Chats.FirstOrDefaultAsync(c => c.ChatId == Id);
            ctx.Chats.Remove(c);
            await ctx.SaveChangesAsync();

            return RedirectToAction("Chat");
        }
    }
}
