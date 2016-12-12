using BookBase.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BookBase.DAL
{
    //This initializer creates Database if not exist in location defined in connection string.
    public class BookBaseInitializer : CreateDatabaseIfNotExists<BookBase.DAL.BookBaseDB>
    {
        protected override void Seed(BookBaseDB context)
        {
            //Filling database with Authors
            var authorList = new List<Author>()
                {
                    new Author() { Name  = "Thomas Nield" },
                    new Author() { Name = "Tomasz Jakut" },
                    new Author() { Name = "Robert C. Martin" },
                    new Author() { Name = "Stephen Prata" }
                };

            foreach (var author in authorList)
                context.Authors.Add(author);
            context.SaveChanges();

            //Filling database with Books
            var bookList = new List<Book>() {
                    new Book()
                    {
                        Title = "Pierwsze kroki z SQL. Praktyczne podejście dla początkujących",
                        DateOfRelease = new DateTime(2016, 11, 18),
                        ISBN = "978-83-283-2818-1",
                        AuthorId = 1
                    },
                    new Book()
                    {
                        Title = "JavaScript. Programowanie zaawansowane.",
                        DateOfRelease = new DateTime(2016, 09, 12),
                        ISBN = "978-83-283-2528-9",
                        AuthorId = 2
                    },
                    new Book()
                    {
                        Title = "Czysty kod. Podręcznik dobrego programisty.",
                        DateOfRelease = new DateTime(2010, 02, 19),
                        ISBN = "978-83-283-0234-1",
                        AuthorId = 3
                    },
                    new Book()
                    {
                        Title = "Język C++. Szkoła programowania. Wydanie VI.",
                        DateOfRelease = new DateTime(2012, 12, 10),
                        ISBN = "978-83-246-4336-3",
                        AuthorId = 4
                    }
            };

            foreach (var book in bookList)
                context.Books.Add(book);
            context.SaveChanges();
        }
    }
}