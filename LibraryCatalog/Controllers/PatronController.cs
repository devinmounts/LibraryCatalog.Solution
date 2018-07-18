using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;

namespace LibraryCatalog.Controllers
{
    public class PatronController : Controller
    {
        [HttpGet("/add/patron")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("/add/patron")]
        public ActionResult CreatePost(string name)
        {
            Patron newPatron = new Patron(name);
            newPatron.Save();

            return RedirectToAction("Patrons");
        }

        [HttpGet("/view/patrons")]
        public ActionResult Patrons()
        {
            return View(Patron.GetAll());
        }

        [HttpGet("/view/patrons/{id}")]
        public ActionResult Details(int id)
        {
            Patron existingPatron = Patron.Find(id);
            return View(existingPatron);
        }

        [HttpGet("/view/patrons/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Patron existingPatron = Patron.Find(id);
            return View(existingPatron);
        }

        [HttpPost("/view/patrons/{id}/update")]
        public ActionResult Update(int id, string name)
        {
            Patron existingPatron = Patron.Find(id);
            existingPatron.Edit(name);

            return RedirectToAction("Details", id);
        }

        [HttpPost("/view/patrons/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Patron existingPatron = Patron.Find(id);
            existingPatron.Delete();
            return RedirectToAction("Patrons");
        }

        [HttpGet("/view/patrons/{id}/checkout")]
        public ActionResult CheckoutForm(int id)
        {
            Patron existingPatron = Patron.Find(id);
            CheckoutViewModel viewModel = new CheckoutViewModel(existingPatron);
            return View(viewModel);
        }

        [HttpPost("/view/patrons/{id}/checkout")]
        public ActionResult CheckoutCopy(int bookId, int id, string date)
        {
            DateTime newDate = Convert.ToDateTime(date);
            Copy newCopy = new Copy(newDate);
            newCopy.Save();

            Patron existingPatron = Patron.Find(id);
            existingPatron.AddCopy(newCopy);
            Book existingBook = Book.Find(bookId);
            existingBook.AddCopy(newCopy);

            return RedirectToAction("Details", id);
        }
    }
}