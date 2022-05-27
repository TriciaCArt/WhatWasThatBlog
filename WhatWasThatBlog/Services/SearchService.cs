using Microsoft.EntityFrameworkCore;
using WhatWasThatBlog.Data;
using WhatWasThatBlog.Enums;
using WhatWasThatBlog.Models;

namespace WhatWasThatBlog.Services
{
    public class SearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BlogPost> Search(string searchString)
        {
            var postRepository = new List<BlogPost>().AsQueryable();

            if (string.IsNullOrWhiteSpace(searchString))
            {
                return postRepository;
            }

            searchString = searchString.ToLower();

            postRepository = _context.BlogPosts
               .Include(b => b.Tags)
               .Include(b => b.BlogPostComments)
               .ThenInclude(c => c.Author)
               .Where(b => b.BlogPostState == BlogPostState.ProductionReady && !b.IsDeleted)
               .AsQueryable();

            postRepository = postRepository.Where(b => b.Title.ToLower().Contains(searchString)
                    || b.Abstract.ToLower().Contains(searchString)
                    || b.Body.ToLower().Contains(searchString)
                    || b.BlogPostComments.Any(
                                   c => c.Comment.ToLower().Contains(searchString)
                                || c.Author!.FirstName.ToLower().Contains(searchString)
                                || c.Author!.LastName.ToLower().Contains(searchString))
                    || b.Tags.Any(t=>t.Text.ToLower().Contains(searchString)));

            
            return postRepository.OrderByDescending(b=>b.Created);
                  
        }
    }
}
