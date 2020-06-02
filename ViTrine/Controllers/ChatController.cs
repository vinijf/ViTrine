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
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext ctx;

        public ChatController(ApplicationDbContext context)
        {
            ctx = context;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(Guid Id)
        {
            var chats = await ctx.Chats.Where(c => c.LojaId == Id).ToListAsync();

            ViewBag.LojaId = Id;

            return View(chats);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Read(Guid Id)
        {
            var mensagens = await ctx.Mensagens.Where(c => c.ChatId == Id).ToListAsync();

            ViewBag.ChatId = Id;

            return View(mensagens);
        }

        [HttpGet]
        [Authorize]
        public IActionResult New(Guid Id)
        {
            return View(new Chat() { LojaId = Id });
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
                return RedirectToAction("Read", new { Id });
            }
            return View(m);
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid Id)
        {
            Chat c = await ctx.Chats.FirstOrDefaultAsync(c => c.ChatId == Id);
            ctx.Chats.Remove(c);
            await ctx.SaveChangesAsync();

            return RedirectToAction("Index", new { c.LojaId });
        }
    }
}
