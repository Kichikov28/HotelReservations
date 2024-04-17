using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.ViewModels.Reservations;
using HotelReservations.Services.Contracts;
using HotelReservations.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HotelReservations.ViewModels.Shared;
using Castle.Core.Resource;

namespace HotelReservations.Web.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IReservationsService service;

        public ReservationsController(IReservationsService service, ApplicationDbContext context)
        {
            this.service = service;
            this.context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index(IndexReservationsViewModel model)
        {
            model = await service.GetReservationsAsync(model);
            return View(model);
        }

        public async Task<IActionResult> Create(string roomId)
        {
            CreateReservationViewModel model = new CreateReservationViewModel();
            await ConfigureCreateVM(model, roomId);
            if (!model.Rooms.Any())
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel() { ErrorMessage = "No free rooms at his time" });
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel model)
        {
            //Gets current user's id
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Remove temporary empty Client objects
            model.Clients = model.Clients.Where(x => x.FirstName != null && x.LastName != null && x.Number != null).ToList();

            // Check if user submitted a roomid
            if (model.RoomId == null)
            {
                ModelState.AddModelError(nameof(model.RoomId), "Please select and submit a room");
                await ConfigureCreateVM(model, model.RoomId);
                return View(model);
            }
            //Checks Accommodation and Leave date if they are sensible
            if (CheckDurationOfDates(model.LeaveDate, model.AccommodationDate))
            {
                ModelState.AddModelError(nameof(model.LeaveDate), "Leave date can't be before Accommodation Date");
                ModelState.AddModelError(nameof(model.AccommodationDate), "Accommodation Date can't be after Leave Date");
                await ConfigureCreateVM(model, model.RoomId);
                return View(model);
            }
            //Check if nubmer of people is more than room capacity
            if (await service.GetRoomCapacityAsync(model.RoomId) < model.Clients.Count)
            {
                ModelState.AddModelError(nameof(model.Clients), "Number of people exceeds Room Capacity");
                await ConfigureCreateVM(model, model.RoomId);
                return View(model);
            }
            //check if user inputed at least 1 Client
            if (!model.Clients.Any())
            {
                ModelState.AddModelError(nameof(model.Clients), "Add at least 1 person");
                await ConfigureCreateVM(model, model.RoomId);
                return View(model);
            }
            List<Client> inputClients = model.Clients.Select(x => new Client()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Number = x.Number,
            }).ToList();
            //chek every inputted User if he exists in database and if he already has a reservation
            foreach (var cust in inputClients)
            {
                Client Client = await service.FindClientAsync(cust);
                if (Client == null)
                {
                    ModelState.AddModelError(nameof(model.Clients), $"{cust.FirstName} {cust.LastName} isn't found in the database. You have to first add him/her");
                    await ConfigureCreateVM(model, model.RoomId);
                    return View(model);
                }
                if (Client.Reservation != null)
                {
                    ModelState.AddModelError(nameof(model.Clients), $"{cust.FirstName} {cust.LastName} has already been asigned to a Reservation");
                    await ConfigureCreateVM(model, model.RoomId);
                    return View(model);
                }
            }
            ModelState.MarkFieldValid("Reservations");
            await service.CreateReservationAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            DetailsReservationViewModel model = await service.GetReservationDetailsAsync(id);
            await service.DeleteReservationAsync(await service.GetReservationToDeleteAsync(id));
            return View(model);
        }
        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,RoomId,UserId,AccommodationDate,LeaveDate,HasBreakfast,HasAllInclusive,Price")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(reservation);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            DetailsReservationViewModel model = await service.GetReservationToDeleteAsync(id);
            return View(model);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DetailsReservationViewModel model)
        {
            await service.DeleteReservationAsync(model);
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(string id)
        {
            return context.Reservations.Any(x => x.Id == id);
        }
        private async Task ConfigureCreateVM(CreateReservationViewModel model, string roomId)
        {
            model.Rooms = new SelectList(await service.GetFreeRoomsSelectListAsync(), "Id", "Number");
            if (!string.IsNullOrWhiteSpace(roomId) && await service.GetRoomCapacityAsync(roomId) > 0)
            {
                model.RoomId = roomId;
                model.RoomCapacity = await service.GetRoomCapacityAsync(roomId);
            }
            for (int i = 0; i < model.RoomCapacity; i++)
            {
                model.Clients.Add(new Client());
            }
        }
        private static bool CheckDurationOfDates(DateTime LeaveDate, DateTime AccommodationDate)
        {
            return LeaveDate < AccommodationDate;
        }
    }
}
