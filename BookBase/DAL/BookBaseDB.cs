using BookBase.Models;
using System.Data.Entity;

namespace BookBase.DAL
{
    public class BookBaseDB : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}