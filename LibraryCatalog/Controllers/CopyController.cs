using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;

namespace LibraryCatalog.Controllers
{
    public class CopyController : Controller
    {
        [HttpGet("/add/copy")]
        public ActionResult Create(int id)
        {
            return View();
        }

        [HttpPost("/add/copy")]
        public ActionResult CheckoutCopy(int bookId, int patronId, string dueDate)
        {
            return new EmptyResult();
        }

        [HttpGet("/view/copies")]
        public ActionResult Copies()
        {
            return View(Copy.GetAll());
        }

        [HttpGet("/view/copy/{id}")]
        public ActionResult Details(int id)
        {
            Copy existingCopy = Copy.Find(id);
            return View(existingCopy);
        }

        [HttpPost("/view/copy/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Copy existingCopy = Copy.Find(id);
            existingCopy.Delete();
            return RedirectToAction("Copies");
        }
    }
}