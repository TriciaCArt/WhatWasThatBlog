using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatWasThatBlog.Data;
using WhatWasThatBlog.Enums;
using WhatWasThatBlog.Models;

namespace WhatWasThatBlog.Controllers.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlogPostAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("/GetTopXPosts/{num}")]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetTopXPosts(int num)
        {
            if (_context.BlogPosts == null)
            {
                return NotFound();
            }
            //1. these need to be ordered by decending created date
            //2. I need some way to limit the records I get using the incoming num
            //3. Also need to be aware of the BloPostState and the Deleted property
            var blogPosts = _context.BlogPosts
                        .Where(b => b.BlogPostState == BlogPostState.ProductionReady && !b.IsDeleted)
                        .OrderByDescending(b=>b.Created)
                        .Take(num);
           
            return await blogPosts.ToListAsync();
        }

        // GET: api/BlogPostAPI
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts()
        //{
        //  if (_context.BlogPosts == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.BlogPosts.ToListAsync();
        //}

        //// GET: api/BlogPostAPI/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<BlogPost>> GetBlogPost(int id)
        //{
        //  if (_context.BlogPosts == null)
        //  {
        //      return NotFound();
        //  }
        //    var blogPost = await _context.BlogPosts.FindAsync(id);

        //    if (blogPost == null)
        //    {
        //        return NotFound();
        //    }

        //    return blogPost;
        //}

        // PUT: api/BlogPostAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogPost(int id, BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return BadRequest();
            }

            _context.Entry(blogPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BlogPostAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BlogPost>> PostBlogPost(BlogPost blogPost)
        {
          if (_context.BlogPosts == null)
          {
              return Problem("Entity set 'ApplicationDbContext.BlogPosts'  is null.");
          }
            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlogPost", new { id = blogPost.Id }, blogPost);
        }

        // DELETE: api/BlogPostAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            if (_context.BlogPosts == null)
            {
                return NotFound();
            }
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogPostExists(int id)
        {
            return (_context.BlogPosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
