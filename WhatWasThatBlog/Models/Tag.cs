using System.ComponentModel.DataAnnotations;

namespace WhatWasThatBlog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = "";
        public string? Description { get; set; }
        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();

    }
}
