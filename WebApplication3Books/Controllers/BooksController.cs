using Microsoft.AspNetCore.Mvc;
using WebApplication3Books.Entities;
using WebApplication3Books.Models;

namespace WebApplication3Books.Controllers
{

    public class BooksController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Random()
        {
            var book = new Book() { Title = "Под игото" };
            return View(book);
        }

        public ActionResult Content()
        {
            var book = new Book() { Title = "Под игото" };
            return Content("Hello Niki");
        }

        public ActionResult RedirectToHome()
        {

            return RedirectToAction("Index", "Home", new { pages = 1, sortBy = "name" });
        }

        public ActionResult Edit(int id)
        {

            return Content("id=" + id);
        }

        private BookEntity _db = new BookEntity();
        /* public async ActionResult Index()
         {
             var books = await _context.Books.include(b => b.author).Select(b => new Book()).toListAsync();


             return View(books);
         }*/

    }

}
