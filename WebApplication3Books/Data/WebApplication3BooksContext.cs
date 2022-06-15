using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication3Books.Models;

namespace WebApplication3Books.Data
{
    public class WebApplication3BooksContext : DbContext
    {
        public WebApplication3BooksContext (DbContextOptions<WebApplication3BooksContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplication3Books.Models.Book>? Book { get; set; }
    }
}
