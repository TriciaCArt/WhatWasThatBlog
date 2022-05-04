using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WhatWasThatBlog.Models;

namespace WhatWasThatBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BlogPost> BlogPosts { get; set; } = default!;

        public DbSet<WhatWasThatBlog.Models.BlogPostComment> BlogPostComment { get; set; }
       
    }
}