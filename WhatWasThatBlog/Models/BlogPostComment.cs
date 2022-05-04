using System.ComponentModel.DataAnnotations;

namespace WhatWasThatBlog.Models
{
    public class BlogPostComment
    {
        public int Id { get; set; } //incrementing primary key showing what comment ID it is
        public int BlogPostId { get; set; } //foreign key that ties the comments to the blog post

        [Required]
        public string Comment { get; set; } = string.Empty;


        //navigational properties
        public virtual BlogPost? BlogPost { get; set; }
    }
}
