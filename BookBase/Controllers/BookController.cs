using BookBase.DAL;
using BookBase.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookBase.Controllers
{
    public class BookController : Controller
    {
        BookBaseDB db = new BookBaseDB();

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
                return PartialView("_BookListPartial", db.Books.ToList());
            else
                return View(db.Books.ToList());
        }

        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
                return PartialView("_BookCreatePartial");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                Book newBook = new Book();
                newBook.Title = book.Title;
                newBook.DateOfRelease = book.DateOfRelease;
                newBook.ISBN = book.ISBN;

                // Searching if entered author already exist in database. If not - creating new Author.
                Author author = db.Authors.Where(b => b.Name.ToLower() == book.Author.Name.ToLower()).FirstOrDefault();
                if (author == null)
                {
                    Author newAuthor = new Author() { Name = book.Author.Name };
                    db.Authors.Add(newAuthor);
                    db.SaveChanges();
                }

                author = db.Authors.Where(b => b.Name.ToLower() == book.Author.Name.ToLower()).FirstOrDefault();
                newBook.AuthorId = author.Id;
                db.Books.Add(newBook);
                db.SaveChanges();

                if (Request.IsAjaxRequest())
                    return JavaScript("BookCreateSuccesful()");
                else
                    return RedirectToAction("Index");
            }
            else
            {
                if (!Request.IsAjaxRequest())
                    return View(book);
                else
                    return JavaScript("BookCreateFailed()");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                throw new HttpException(400, "Bad Request");
            Book book = db.Books.Find(id);

            if (book == null)
                throw new HttpException(404, "Page not found");

            if (Request.IsAjaxRequest())
                return PartialView("_BookEditPartial", book);
            else
                return View(book);
        }


        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                Book editBook = db.Books.Find(book.Id);
                if (editBook != null)
                {
                    editBook.Title = book.Title;
                    editBook.DateOfRelease = book.DateOfRelease;
                    editBook.ISBN = book.ISBN;
                    Author author = db.Authors.Where(b => b.Name.ToLower() == book.Author.Name.ToLower()).FirstOrDefault();
                    if (author == null)
                    {
                        Author newAuthor = new Author() { Name = book.Author.Name };
                        db.Authors.Add(newAuthor);
                        db.SaveChanges();
                    }
                    author = db.Authors.Where(b => b.Name.ToLower() == book.Author.Name.ToLower()).FirstOrDefault();
                    editBook.AuthorId = author.Id;
                }
                db.SaveChanges();
            }
            else
                if (!Request.IsAjaxRequest())
                return View(book);

            if (Request.IsAjaxRequest())
                return JavaScript("BookUpdateSuccesful()");
            else
                return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                throw new HttpException(400, "Bad Request");
            Book book = db.Books.Find(id);
            if (book == null)
                throw new HttpException(404, "Page not found");
            if (Request.IsAjaxRequest())
                return PartialView("_BookDetailsPartial", book);
            else
                return View(book);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                throw new HttpException(400, "Bad Request");

            Book book = db.Books.Find(id);
            if (book == null)
                throw new HttpException(404, "Page not found");

            if (Request.IsAjaxRequest())
                return PartialView("_BookDeletePartial", book);
            else
                return View(book);
        }

        [HttpPost]
        public ActionResult Delete(Book book)
        {
            Book bookToRemove = db.Books.Find(book.Id);
            if (bookToRemove != null)
                db.Books.Remove(bookToRemove);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
                return JavaScript("BookDeleteSuccesful()");
            else
                return RedirectToAction("Index");
        }
    }
}