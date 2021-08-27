using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Collections.Generic;
using LibraryManagement5.Context;

namespace LibraryManagement5.Controllers
{
    public class StudentController : Controller
    {
        #region Database Initialization 
        LibraryEntities15 dbobj = new LibraryEntities15();
        Book dbbook = new Book();
        Borrow dbborrrow = new Borrow();
        User dbuser = new User();
        #endregion

        #region Action: Show Books
        [Authorize]
        public ActionResult Books()
        {
            ViewData["username"] = User.Identity.Name;
            var username = ViewData["username"]; 
            var res = dbobj.Users.Where(x => x.username == username.ToString()).First();
            dbbook.UsernameID = res.Id;
            
            return View(dbbook);
        }
        #endregion

        #region Action: Add Books
        [Authorize]
        [HttpPost]
        public ActionResult AddBook(Book model)
        {
            ViewData["username"] = User.Identity.Name;
            var username = ViewData["username"];
            var res = dbobj.Users.Where(x => x.username == username.ToString()).First();
            dbbook.UsernameID = res.Id;

            if (ModelState.IsValid)
            {
                dbbook.id = model.id;
                dbbook.BookName = model.BookName;
                dbbook.Author = model.Author;
                dbbook.Quantity =  model.Quantity;
                dbbook.UsernameID = model.UsernameID;
                dbbook.BookStatus = model.BookStatus;
                dbbook.creationTime = DateTime.Now;

                if(model.id == 0)
                {
                    dbobj.Books.Add(dbbook);
                    Response.Write("<script LANGUAGE='JavaScript'>alert('Successfully Inserted!')</script>");
                }
                else
                {
                    dbobj.Entry(dbbook).State = EntityState.Modified;
                    Response.Write("<script LANGUAGE='JavaScript'>alert('Successfully Updated!')</script>");
                }
                dbobj.SaveChanges(); 
                ModelState.Clear();
            }
            return View("Books");
        }
        #endregion

        #region Action: Show Books with Sorting
        [Authorize]
        public ActionResult BookList(int? page, string searchBy, string search, string sortBy)
        {
            ViewData["username"] = User.Identity.Name;
            var username = ViewData["username"];
            var res = dbobj.Users.Where(x => x.username == username.ToString()).First();
            ViewData["userId"] = res.Id;

            ViewBag.SortByBookName = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SortByAuthor = sortBy == "Author" ? "Author desc" : "Author";
            ViewBag.SortByQuantity = sortBy == "Quantity" ? "Quantity desc" : "Quantity";

            var books = dbobj.Books.AsQueryable();
            switch (searchBy){
                case "Author":
                    books = books.Where(x => x.Author.StartsWith(search) || search == null);
                    break;
                case "Available Books":
                    books = books.Where(x => !x.Quantity.Contains("0") || search == null);
                    break;
                default:
                    books = books.Where(x => x.BookName.StartsWith(search) || search == null);
                    break;
                }

            switch (sortBy)
            {
                case "Name desc":
                    books = books.OrderByDescending(x => x.BookName);
                    break;
                case "Author desc":
                    books = books.OrderByDescending(x => x.Author);
                    break;
                case "Author":
                    books = books.OrderBy(x => x.Author);
                    break;
                case "Quantity desc":
                    books = books.OrderByDescending(x => x.Quantity);
                    break;
                case "Quantity":
                    books = books.OrderBy(x => x.Quantity);
                    break;
                default:
                    books = books.OrderBy(x => x.BookName);
                    break;
            }

            return View(books.ToPagedList(page ?? 1, 3));
        }
        #endregion

        #region Action: Show User Specific Books
        [Authorize]
        public ActionResult MyBooks(int? page, string searchBy, string search, string sortBy)
        {
            ViewData["username"] = User.Identity.Name;
            var username = ViewData["username"];
            var res = dbobj.Users.Where(x => x.username == username.ToString()).First();
            int userid = res.Id;
            var res2 = dbobj.Books.Where(x => x.UsernameID == userid).ToList().ToPagedList(page ?? 1,3);

            ViewBag.SortByBookName = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SortByAuthor = sortBy == "Author" ? "Author desc" : "Author";
            ViewBag.SortByQuantity = sortBy == "Quantity" ? "Quantity desc" : "Quantity";
            ViewBag.BorrowedBooks = BorrowedBooks(userid, page);
            ViewData["userId"] = res.Id;

            var books = dbobj.Books.AsQueryable();
            books = books.Where(x => x.UsernameID == userid);

            switch (searchBy)
            {
                case "Author":
                    books = books.Where(x => x.Author.StartsWith(search) || search == null);
                    break;
                case "Available Books":
                    books = books.Where(x => !x.Quantity.Contains("0") || search == null);
                    break;
                default:
                    books = books.Where(x => x.BookName.StartsWith(search) || search == null);
                    break;
            }

            switch (sortBy)
            {
                case "Name desc":
                    books = books.OrderByDescending(x => x.BookName);
                    break;
                case "Author desc":
                    books = books.OrderByDescending(x => x.Author);
                    break;
                case "Author":
                    books = books.OrderBy(x => x.Author);
                    break;
                case "Quantity desc":
                    books = books.OrderByDescending(x => x.Quantity);
                    break;
                case "Quantity":
                    books = books.OrderBy(x => x.Quantity);
                    break;
                default:
                    books = books.OrderBy(x => x.BookName);
                    break;
            }
            return View(books.ToPagedList(page ?? 1, 3));
        }
        #endregion

        #region Action: Delete Books
        [Authorize]
        public ActionResult Delete(int id, int? page)
        {
            ViewData["username"] = User.Identity.Name;
            var username = ViewData["username"];
            var res2 = dbobj.Users.Where(x => x.username == username.ToString()).First();
            ViewData["userId"] = res2.Id;

            var res = dbobj.Books.Where(x => x.id == id).First();
            dbobj.Books.Remove(res);
            try
            {
                dbobj.SaveChanges();
                Response.Write("<script LANGUAGE='JavaScript'>alert('Successfully Deleted!')</script>");
            }
            catch(Exception ex)
            {
                Response.Write("<script LANGUAGE='JavaScript'>alert('An error occured while deleting!')</script>");
            }
            var list = dbobj.Books.ToList().ToPagedList(page ?? 1,3);
            return View("BookList", list);
        }
        #endregion

        #region Action: Edit Books
        [Authorize]
        public ActionResult Edit(Book Obj2, int? page)
        {
            ViewData["username"] = User.Identity.Name;
            var username = ViewData["username"];
            var res = dbobj.Users.Where(x => x.username == username.ToString()).First();
            Obj2.UsernameID = res.Id;
            ViewData["userId"] = res.Id;

            return View("Books", Obj2);
        }
        #endregion

        #region Action: Borrow Books
        [Authorize]
        public ActionResult Borrow(Book book, int? page)
        {
            ViewData["username"] = User.Identity.Name;
            var username = ViewData["username"];
            var res2 = dbobj.Users.Where(x => x.username == username.ToString()).First();
            ViewData["userId"] = res2.Id;

            dbborrrow.BookId = book.id;
            dbborrrow.UserId = res2.Id;
            dbborrrow.borrowDateTime = DateTime.Now;
            var res = dbobj.Books.Where(x => x.id == book.id).First();

            dbobj.Borrows.Add(dbborrrow);
            try
            {
                dbobj.SaveChanges();
                Response.Write("<script LANGUAGE='JavaScript'>alert('Successfully borrowed!')</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script LANGUAGE='JavaScript'>alert('An error occurred while borrowing!')</script>");
            }
            var list = dbobj.Books.ToList().ToPagedList(page ?? 1, 3);

            return View("BookList", list);
        }
        #endregion

        #region Action: Return Books
        [Authorize]
        public ActionResult Return(Book book, int? page)
        {
            ViewData["username"] = User.Identity.Name;
            var username = ViewData["username"];
            var res2 = dbobj.Users.Where(x => x.username == username.ToString()).First();
            ViewData["userId"] = res2.Id;

            dbborrrow.BookId = book.id;
            var res = dbobj.Borrows.Where(x => x.BookId == book.id).ToList();
            if(res != null)
            {
                foreach(var i in res)
                {
                    if (i.UserId == res2.Id)
                    {
                        dbobj.Borrows.Remove(i);
                    }    
                }
            }
            try
            {
                dbobj.SaveChanges();
                Response.Write("<script LANGUAGE='JavaScript'>alert('Book returned successfully!')</script>");
            }
            catch(Exception ex)
            {
                Response.Write("<script LANGUAGE='JavaScript'>alert('An error occurred while returning!')</script>");
            }

            var list = dbobj.Books.ToList().ToPagedList(page ?? 1, 3);
            return View("BookList", list);
        }
        #endregion

        #region Action: Show Borrowed Books
        [Authorize]
        public IPagedList BorrowedBooks(int userid, int? page)
        {
            var mainRes = new List<Book> ();
            var res = dbobj.Borrows.Where(x => x.UserId == userid).ToList();
            if(res != null)
            {
                foreach (var i in res)
                {
                    var res2 = dbobj.Books.Where(x => x.id == i.BookId).First();
                    mainRes.Add(res2);
                }
            }
            
            return (mainRes.ToPagedList(page ?? 1,3));
        }
        #endregion
    }
}