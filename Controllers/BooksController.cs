using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookManagementAPI.Models;
using BookManagementAPI.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Identity.Client.AppConfig;

namespace BookManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(books); 
        }

        // GET: api/Books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            
            book.ViewCount++;

            
            await _context.SaveChangesAsync();

            var yearSincePublished = DateTime.Now.Year - book.YearPublished;
            double popularity = 1;
            if (book.ViewCount != 0 && yearSincePublished != 0)
            {
                popularity = book.ViewCount / (yearSincePublished / 10.0);
            }
            
                
            var result = new
            {
                book.Id,
                book.Title,
                book.Author,
                book.ViewCount,
                popularity
            };

            return Ok(result);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);

        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest(); 

            }

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(); 
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        // DELETE: api/books/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> BulkDeleteBooks([FromBody] List<int> bookIds)
        {
            if (bookIds == null || !bookIds.Any())
            {
                return BadRequest("No book IDs provided.");
            }

            var booksToDelete = await _context.Books
                                               .Where(book => bookIds.Contains(book.Id))
                                               .ToListAsync();

            if (!booksToDelete.Any())
            {
                return NotFound("No books found with the given IDs.");
            }

            _context.Books.RemoveRange(booksToDelete);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        // POST: /api/books/bulk
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBooksInBulk([FromBody] List<Book> books)
        {
            if (books == null || books.Count == 0)
            {
                return BadRequest("No books provided.");
            }

            
            foreach (var book in books)
            {
                if (string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Author))
                {
                    return BadRequest("Each book must have a title and author.");
                }

                
                var existingBook = await _context.Books
                    .FirstOrDefaultAsync(b => b.Title == book.Title && b.Author == book.Author);

                if (existingBook != null)
                {
                    return BadRequest($"Book '{book.Title}' by {book.Author} already exists.");
                }
            }

            
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                
                _context.Books.AddRange(books);

                
                await _context.SaveChangesAsync();

               
                await transaction.CommitAsync();

                
                return CreatedAtAction(nameof(GetBookById), new { id = books[0].Id }, books);
            }
            catch (Exception)
            {
                
                await transaction.RollbackAsync();

                
                return StatusCode(500, "Internal server error while adding books.");
            }
        }
        // GET: api/Books/popular
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularBooks(int pageNumber = 1, int pageSize = 10)
        {
            
            if (pageNumber < 1)
            {
                return BadRequest("Page number must be greater than or equal to 1.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page size must be between 1 and 100.");
            }

            
            var books = await _context.Books
                .OrderByDescending(b => b.ViewCount)  
                .Skip((pageNumber - 1) * pageSize)   
                .Take(pageSize)                     
                .Select(b => new { b.Title })       
                .ToListAsync();

            return Ok(books);
        }

    }
}
