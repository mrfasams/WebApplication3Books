using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3Books.Data;
using WebApplication3Books.Models;

namespace WebApplication3Books.Controllers
{
    public class Books1Controller : Controller
    {
        private readonly WebApplication3BooksContext _context;

        public Books1Controller(WebApplication3BooksContext context)
        {
            _context = context;
        }

        // GET: Books1
        public async Task<IActionResult> Index()
        {
            return _context.Book != null ?
                        View(await _context.Book.ToListAsync()) :
                        Problem("Entity set 'WebApplication3BooksContext.Book'  is null.");
        }

        // GET: Books1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,author,description,isbn,type,Content")] Book book, IFormFile uploadFile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,author,description,isbn,type,Content,FileType")] Book book, List<IFormFile> file)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'WebApplication3BooksContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> Search(string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Book

                                            select m.Title;
            var books = from m in _context.Book
                        select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title!.Contains(searchString) || s.author!.Contains(searchString)).OrderBy(book => book.Title);
            }

            return View(await books.ToListAsync());
        }


        //this method is used to create new book with uploded file
        [HttpPost]
        public async Task<IActionResult> UploadBookToDatabase(List<IFormFile> files, Book book)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileModel = new Book
                {
                    author = book.author,
                    isbn = book.isbn,
                    Title = book.Title,
                    type = book.type,
                    FileType = file.ContentType,
                    Extension = extension,
                    fileName = fileName,
                    description = book.description

                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Content = dataStream.ToArray();
                }
                _context.Update(fileModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {

            var book = await _context.Book.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (book == null) return null;
            return File(book.Content, "application/force-download", book.fileName + book.Extension);
            // return File(book.Content, book.FileType, book.fileName + book.Extension);
        }
        //get some book for recommended
        public async Task<IActionResult> Random()
        {
            int first = 13;
            int second = 11;
            int third = 3;


            // Use LINQ to get list of tree books.

            var books = from m in _context.Book
                        select m;


            books = books.Where(x => (x.Id == first || x.Id == second || x.Id == second)).OrderBy(book => book.Title);


            return View(await books.ToListAsync());
        }

        //this method is used to edit new book with uploded file
        [HttpPost]
        public async Task<IActionResult> editBookDatabase(List<IFormFile> files, Book book)

        {//if no new file is uploded , keep current 
            if (files.Count == 0)
            {
                var item = await _context.Book.SingleOrDefaultAsync(x => x.Id == book.Id);
                book.Content = item.Content;
                book.fileName = item.fileName;
                book.Extension = item.Extension;
                book.FileType = item.FileType;
                //remove from context because error occure on update
                _context.Remove(item);
            }
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);

                book.FileType = file.ContentType;
                book.fileName = fileName;
                book.Extension = extension;
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    book.Content = dataStream.ToArray();
                }

            }
            _context.Update(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
