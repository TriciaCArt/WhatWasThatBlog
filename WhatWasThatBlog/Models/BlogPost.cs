using System.ComponentModel.DataAnnotations;

namespace WhatWasThatBlog.Models
{
    public class BlogPost
    {
        public int Id { get; set; } //This turns into an auto incrementing integer to act as primary key Unique
        [Required]
        public string Title { get; set; } = "";
        [Required]
        public string Abstract { get; set; } = "";
        [Required]
        public string Body { get; set; } = "";

        [DataType(DataType.Date)]
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }

        public virtual ICollection<BlogPostComment> BlogPostComments { get; set; } = new HashSet<BlogPostComment>();

    }
}
