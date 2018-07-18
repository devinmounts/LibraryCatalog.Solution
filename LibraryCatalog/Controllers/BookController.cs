using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryCatalog.Models;

namespace LibraryCatalog.Controllers
{
    public class BookController : Controller
    {
        [HttpGet("/add/book")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("/add/book")]
        public ActionResult CreatePost(string name)
        {
            Book newBook = new Book(name);
            newBook.Save();

            return RedirectToAction("Books");
        }

        [HttpGet("/view/books")]
        public ActionResult Books()
        {
            return View(Book.GetAll());
        }

        [HttpGet("/view/book/{id}")]
        public ActionResult Details(int id)
        {
            Book existingBook = Book.Find(id);
            return View(existingBook);
        }

        [HttpGet("/view/book/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Book existingBook = Book.Find(id);
            return View(existingBook);
        }

        [HttpPost("/view/book/{id}/update")]
        public ActionResult Update(int id, string name)
        {
            Book existingBook = Book.Find(id);
            existingBook.Edit(name);

            return RedirectToAction("Details", id);
        }

        [HttpPost("/view/book/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Book existingBook = Book.Find(id);
            existingBook.Delete();
            return RedirectToAction("Books");
        }
    }
}