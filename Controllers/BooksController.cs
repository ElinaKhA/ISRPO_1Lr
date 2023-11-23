using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISRPO_1Lr.Model;
using Microsoft.AspNetCore.Cors;

namespace ISRPO_1Lr.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly BooksList1Context _context;

        public BooksController(BooksList1Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/GetBooks")]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: Books(id)
        [HttpGet("{id}", Name = "GetBook")]
        public ActionResult GetBookId(int id)
        {
            var b = _context.Books.FirstOrDefault(t => t.Id == id);
            if (b == null)
            {
                return NotFound();
            }
            return Ok(b);
        }
        [HttpPost(Name = "CreateBook")]
        public ActionResult CreateBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest(); // Вместо NotFound() в случае, если данные не переданы
            }

            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                return CreatedAtRoute("GetBook", new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                // Обработка ошибки сохранения данных в базу
                return StatusCode(500, "Failed to create book");
            }
        }
        // PUT: UpdateBook
        [HttpPut("{id}", Name = "UpdateBook")]
        public ActionResult UpdateBook(int id, [FromBody] Book book)
        {
            var existingB = _context.Books.FirstOrDefault(t => t.Id == id);
            if (existingB == null)
            {
                return NotFound();
            }
            existingB.Name = book.Name;
            existingB.Author = book.Author;
            existingB.Genre = book.Genre;
            existingB.Status = book.Status;
            _context.SaveChanges();
            return NoContent();
        }
        // DELETE: DeleteBook
        [HttpDelete("{id}", Name = "DeleteBook")]
        public ActionResult DeleteBook(int id)
        {
            var b = _context.Books.FirstOrDefault(t => t.Id == id);
            if (b == null)
            {
                return NotFound();
            }
            _context.Books.Remove(b);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
