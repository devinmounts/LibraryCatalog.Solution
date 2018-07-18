using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryCatalog.Models;

namespace LibraryCatalog.Controllers
{
    public class CopyController : Controller
    {
        [HttpGet("/add/copy")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("/add/copy")]
        public ActionResult CreatePost(string dueDate)
        {
            DateTime newDate = Convert.ToDateTime(dueDate);
            Copy newCopy = new Copy(newDate);
            newCopy.Save();

            return RedirectToAction("Copies");
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