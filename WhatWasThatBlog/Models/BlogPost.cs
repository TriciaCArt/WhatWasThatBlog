using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WhatWasThatBlog.Enums;

namespace WhatWasThatBlog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string? AuthorId { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long", MinimumLength = 5)]
        public string Title { get; set; } = "";

        [Required]
        public string Abstract { get; set; } = "";

        [Required]
        public string Body { get; set; } = "";

        public string TruncatedBody { get { return this.Body.Length > 400 ? this.Body.Substring(0, 400) + "..." : this.Body; } }

        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Display(Name ="Updated Date")]
        public DateTime? Updated { get; set; }

        //This property is derived from the Title. This will eventually be used in some cases INSTEAD of the Primary Key (Id)            
        public string Slug { get; set; } = "";

        public bool IsDeleted { get; set; }

        public BlogPostState BlogPostState { get; set; }

        [NotMapped]
        [Display(Name = "Choose an Image")]
        public IFormFile? ImageFile { get; set; }


        //What if I wanted to record an image with a blog post
        [Display(Name = "Blog Image")]
        public byte[] ImageData { get; set; } = Array.Empty<byte>();

        [Display(Name = "Image Type")]
        public string ImageType { get; set; } = string.Empty;

        //Nav props
        public virtual BlogUser? Author { get; set; }
        public virtual ICollection<BlogPostComment> BlogPostComments { get; set; } = new HashSet<BlogPostComment>();

        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    }
}
