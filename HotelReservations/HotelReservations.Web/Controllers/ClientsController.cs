using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.ViewModels.Clients;
using HotelReservations.Services.Contracts;

namespace HotelReservations.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClientsService service;

        public ClientsController(ApplicationDbContext context, IClientsService service)
        {
            _context = context;
            this.service = service;
        }

        // GET: Clients
        public async Task<IActionResult> Index(ClientsIndexViewModel model)
        {
            model = await service.GetClientsAsync(model);

            return View(model);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string id, ClientDetailsViewModel model)
        {
            model = await service.GetClientDetailsByIdAsync(id);
            return View(model);

        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                await service.CreateClientAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ClientEditViewModel model = await service.EditCustomerByIdAsync(id);
            return View(model);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                await service.UpdateCustomerAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.Reservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
