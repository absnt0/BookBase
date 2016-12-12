using BookBase.DAL;
using BookBase.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookBase.Controllers
{
    public class AuthorController : Controller
    {
        BookBaseDB db = new BookBaseDB();

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
                return PartialView("_AuthorListPartial", db.Authors);
            else
                return View(db.Authors);
        }

        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
                return PartialView("_AuthorCreatePartial");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(Author author)
        {
            //Adding ModelState Error when author already exist in database. This is needed when javascript is disabled in the browser.
            Author checkAuthor = new Author();
            checkAuthor = db.Authors.Where(b => b.Name.ToLower() == author.Name.ToLower()).FirstOrDefault();
            if (checkAuthor != null)
                ModelState.AddModelError("Name", "Autor już istnieje!");

            if (ModelState.IsValid)
            {
                Author newAuthor = new Author();
                newAuthor.Name = author.Name;
                db.Authors.Add(newAuthor);
                db.SaveChanges();

                if (Request.IsAjaxRequest())
                    return JavaScript("AuthorCreateSuccesful()");
                else
                    return RedirectToAction("Index");
            }
            else
            {
                if (!Request.IsAjaxRequest())
                    return View(author);
                else
                    return JavaScript("AuthorCreateFailed()");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                throw new HttpException(400, "Bad Request");
            var author = db.Authors.Find(id);

            if (author == null)
                throw new HttpException(404, "Page not found");
            if (Request.IsAjaxRequest())
                return PartialView("_AuthorEditPartial", author);
            else
                return View(author);
        }

        [HttpPost]
        public ActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                // Checking if new author name already exist in database. If exist changing Author of all books to the new one.
                var editExistingAuthor = db.Authors.Where(b => b.Name.ToLower() == author.Name.ToLower()).FirstOrDefault();
                if (editExistingAuthor != null && author.Id != editExistingAuthor.Id)
                {
                    List<Book> booksToChange = db.Books.Where(b => b.AuthorId == author.Id).ToList();
                    foreach (var book in booksToChange)
                        book.AuthorId = editExistingAuthor.Id;
                    Author authorToRemove = db.Authors.Where(b => b.Id == author.Id).FirstOrDefault();
                    db.Authors.Remove(authorToRemove);
                }
                else
                {
                    Author editNewAuthor = db.Authors.Find(author.Id);
                    editNewAuthor.Name = author.Name;
                }
                db.SaveChanges();

                if (Request.IsAjaxRequest())
                    return JavaScript("AuthorUpdateSuccesful()");
                else
                    return RedirectToAction("Index");
            }
            else
            {
                if (!Request.IsAjaxRequest())
                    return View(author);
                else
                    return JavaScript("AuthorCreateFailed()");
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                throw new HttpException(400, "Bad Request");

            Author author = db.Authors.Find(id);
            if (author == null)
                throw new HttpException(404, "Page not found");

            if (Request.IsAjaxRequest())
                return PartialView("_AuthorDetailsPartial", author);
            else
                return View(author);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                throw new HttpException(400, "Bad Request");

            Author author = db.Authors.Find(id);
            if (author == null)
                throw new HttpException(404, "Page not found");

            if (Request.IsAjaxRequest())
                return PartialView("_AuthorDeletePartial", author);
            else
                return View(author);
        }

        [HttpPost]
        public ActionResult Delete(Author author)
        {
            Author authorToRemove = db.Authors.Find(author.Id);
            if (authorToRemove != null)
                db.Authors.Remove(authorToRemove);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
                return JavaScript("AuthorDeleteSuccesful()");
            else
                return RedirectToAction("Index");

        }

        // Method used with autocomplete functionality. Returning JSON with authors that contain entered text.
        public ActionResult AuthorSuggestions(string text)
        {
            var authors = from item in db.Authors
                          where item.Name.Contains(text)
                          select new { item.Name, item.Id };

            return Json(authors, JsonRequestBehavior.AllowGet);
        }


        // Method used with remote validator. Checking if author exist in database.
        public JsonResult DoesAuthorExist(string Name)
        {
            bool anyAuthors = !db.Authors.Any(m => m.Name == Name);
            return Json(!db.Authors.Any(m => m.Name == Name), JsonRequestBehavior.AllowGet);
        }
    }
}