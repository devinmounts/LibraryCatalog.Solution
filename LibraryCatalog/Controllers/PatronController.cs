using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryCatalog.Models;

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

        [HttpGet("/view/books")]
        public ActionResult Patrons()
        {
            return View(Patron.GetAll());
        }

        [HttpGet("/view/book/{id}")]
        public ActionResult Details(int id)
        {
            Patron existingPatron = Patron.Find(id);
            return View(existingPatron);
        }

        [HttpGet("/view/book/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Patron existingPatron = Patron.Find(id);
            return View(existingPatron);
        }

        [HttpPost("/view/book/{id}/update")]
        public ActionResult Update(int id, string name)
        {
            Patron existingPatron = Patron.Find(id);
            existingPatron.Edit(name);

            return RedirectToAction("Details", id);
        }

        [HttpPost("/view/book/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Patron existingPatron = Patron.Find(id);
            existingPatron.Delete();
            return RedirectToAction("Patrons");
        }
    }
}