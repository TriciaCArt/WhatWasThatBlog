#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhatWasThatBlog.Data;
using WhatWasThatBlog.Models;
using WhatWasThatBlog.Services.Interfaces;
using WhatWasThatBlog.Utilities;
using WhatWasThatBlog.Services;

namespace WhatWasThatBlog.Controllers
{
    [Authorize(Roles = "Admin, Moderator")]
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;
        private readonly SearchService _searchService;


        public BlogPostsController(ApplicationDbContext context, IConfiguration configuration, IImageService imageService, SearchService searchService)
        {
            _context = context;
            _configuration = configuration;
            _imageService = imageService;
            _searchService = searchService;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index()
        {
            var posts = await _context.BlogPosts
                                      .Include(b => b.Tags)
                                      .ToListAsync();
            return View(posts);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> PostsByTag(int id)
        {

            var taggy = await _context.Tag
                            .Include(t => t.BlogPosts)
                            .FirstOrDefaultAsync(t => t.Id == id);
            return View("SearchPosts", taggy.BlogPosts.ToList());
        }

        public async Task<IActionResult> InDevIndex()
        {
            var posts = await _context.BlogPosts.Include(b => b.Tags).Where(b => b.BlogPostState == Enums.BlogPostState.InDevelopment).ToListAsync();
            return View("Index", posts);
        }

        public async Task<IActionResult> InPreviewIndex()
        {
            var posts = await _context.BlogPosts.Include(b => b.Tags).Where(b => b.BlogPostState == Enums.BlogPostState.InPreview).ToListAsync();
            return View("Index", posts);
        }

        public async Task<IActionResult> SoftDeleteIndex()
        {
            var posts = await _context.BlogPosts.Include(b => b.Tags).Where(b => b.IsDeleted).ToListAsync();
            return View("Index", posts);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchPosts(string searchString)
        {
            var searchHits = _searchService.Search(searchString);

            return View(searchHits);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.Tags)
                .Include(b => b.BlogPostComments.Where(b => !b.IsDeleted))
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(m => m.Slug == slug);

            if (blogPost is null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create        
        public IActionResult Create()
        {
            BlogPost blogPost = new BlogPost();

            ViewData["TagIds"] = new MultiSelectList(_context.Tag, "Id", "Text");
            return View(blogPost);
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Abstract,Body,BlogPostState,ImageFile")] BlogPost blogPost, List<int> tagIds)
        {
            if (ModelState.IsValid)
            {
                SlugService slugSvc = new();
                var slug = slugSvc.URLFriendly(blogPost.Title);
                if (_context.BlogPosts.Any(b => b.Slug == slug))
                {
                    ModelState.AddModelError("Title", "The Title must be changed because it's slug has been used.");

                    ViewData["TagIds"] = new MultiSelectList(_context.Tag, "Id", "Text", tagIds);
                    return View(blogPost);
                }
                else
                {
                    blogPost.Slug = slug;
                }

                if (blogPost.ImageFile != null)
                {
                    blogPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.ImageFile);
                    blogPost.ImageType = blogPost.ImageFile.ContentType;
                }              

                foreach (var tagId in tagIds)
                {
                    blogPost.Tags.Add(await _context.Tag.FindAsync(tagId));
                }

                blogPost.Created = DateTime.SpecifyKind(blogPost.Created, DateTimeKind.Utc);


                _context.Add(blogPost);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["TagIds"] = new MultiSelectList(_context.Tag, "Id", "Text", tagIds);
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                            .Include(b => b.Tags)
                            .FirstOrDefaultAsync(b => b.Id == id);


            if (blogPost == null)
            {
                return NotFound();
            }

            //the fourth parameter in a multiselect list is a List<int> representing the current selection
            var tagPks = blogPost.Tags
                                 .Select(b => b.Id)
                                 .ToList();
            ViewData["TagIds"] = new MultiSelectList(_context.Tag, "Id", "Text", tagPks);

            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Abstract,Body,BlogPostState,ImageFile")] BlogPost blogPost, List<int> tagIds)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPost = await _context.BlogPosts
                                                    .Include(b => b.Tags)
                                                    .FirstOrDefaultAsync(b => b.Id == blogPost.Id);

                    if (blogPost.ImageFile != null)
                    {
                        existingPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.ImageFile);
                        existingPost.ImageType = blogPost.ImageFile.ContentType;
                    }

                    SlugService slugSvc = new();
                    var newslug = slugSvc.URLFriendly(existingPost.Title);
                    if (newslug != existingPost.Slug)
                    {
                        if (_context.BlogPosts.Any(b => b.Slug == newslug))
                        {
                            ModelState.AddModelError("Title", "Change the Title, this slug has been oozed.");
                            ViewData["TagIds"] = new MultiSelectList(_context.Tag, "Id", "Text", tagIds);
                            return View(blogPost);
                        }
                    }

                    existingPost.Tags.Clear();

                    await _context.SaveChangesAsync();

                    existingPost.Slug = newslug;
                    existingPost.Updated = DateTime.UtcNow;
                    existingPost.Title = blogPost.Title;
                    existingPost.Abstract = existingPost.Abstract;
                    existingPost.Body = blogPost.Body;
                    existingPost.BlogPostState = blogPost.BlogPostState;

                    foreach (var tagId in tagIds)
                    {
                        existingPost.Tags.Add(await _context.Tag.FindAsync(tagId));
                    }

                    //_context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
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


            ViewData["TagIds"] = new MultiSelectList(_context.Tag, "Id", "Text");
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }
        public async Task<IActionResult> SoftDelete(int id)
        {
            var deletedPosts = await _context.BlogPosts.FirstOrDefaultAsync(m => m.IsDeleted);
            _context.BlogPosts.Remove(deletedPosts);
              
            if (deletedPosts == null)
            {
                return NotFound();
            }

            return View("Index", deletedPosts);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }

    }
}
