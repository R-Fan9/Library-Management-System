using Library.Models.Patron;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class PatronController : Controller
    {
        private IPatron _patron;
        public PatronController(IPatron patron)
        {
            _patron = patron;
        }
        public IActionResult Index()
        {
            var allPatrons = _patron.GetAll();

            var patronModel = allPatrons.Select(p => new PatronDetailModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                LibraryCardId = p.LibraryCard.Id,
                OverdueFees = p.LibraryCard.Fees,
                HomeLibraryBranch = p.HomeLibraryBranch.Name
            }).ToList();

            var model = new PatronIndexModel()
            {
                Patrons = patronModel
            };

            return View(model);
        }

        public IActionResult Detail(int Id)
        {
            var patron = _patron.Get(Id);

            var model = new PatronDetailModel
            {
                LastName = patron.LastName,
                FirstName = patron.FirstName,
                Address = patron.Address,
                HomeLibraryBranch = patron.HomeLibraryBranch.Name,
                MemberSince = patron.LibraryCard.Created,
                OverdueFees = patron.LibraryCard.Fees,
                LibraryCardId = patron.LibraryCard.Id,
                Telephone = patron.TelephoneNumber,
                AssetCheckedOut = _patron.GetCheckouts(Id).ToList() ?? new List<Checkout>(),
                CheckoutHistory = _patron.GetCheckoutHistories(Id),
                Holds = _patron.GetHolds(Id)
            };
            return View(model);
        }
    }
}
