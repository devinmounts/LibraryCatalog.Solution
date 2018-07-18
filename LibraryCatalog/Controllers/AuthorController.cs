using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;

namespace LibraryCatalog.Controllers
{
    public class AuthorController : Controller
    {
        [HttpGet("/add/author")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("/add/author")]
        public ActionResult CreatePost(string name)
        {
            Author newAuthor = new Author(name);
            newAuthor.Save();

            return RedirectToAction("Authors");
        }

        [HttpGet("/view/authors")]
        public ActionResult Authors()
        {
            return View(Author.GetAll());
        }

        [HttpGet("/view/authors/{id}")]
        public ActionResult Details(int id)
        {
            Author existingAuthor = Author.Find(id);
            return View(existingAuthor);
        }

        [HttpGet("/view/authors/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Author existingAuthor = Author.Find(id);
            return View(existingAuthor);
        }

        [HttpPost("/view/authors/{id}/update")]
        public ActionResult Update(int id, string name)
        {
            Author existingAuthor = Author.Find(id);
            existingAuthor.Edit(name);

            return RedirectToAction("Details", id);
        }

        [HttpPost("/view/authors/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Author existingAuthor = Author.Find(id);
            existingAuthor.Delete();
            return RedirectToAction("Authors");
        }
    }
}

