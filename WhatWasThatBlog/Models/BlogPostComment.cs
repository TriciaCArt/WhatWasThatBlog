using System.ComponentModel.DataAnnotations;
using WhatWasThatBlog.Enums;

namespace WhatWasThatBlog.Models
{
    public class BlogPostComment
    {
        public int Id { get; set; } //incrementing primary key showing what comment ID it is
        public int BlogPostId { get; set; } //foreign key that ties the comments to the blog post
        public string AuthorId { get; set; } = Guid.Empty.ToString();
        public string? ModeratorId { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? Moderated { get; set; }

        [Display(Name = "Moderated Comment")]
        public string? ModeratedComment { get; set; }
        public ModReason? ModReason { get; set; }
        public bool IsDeleted { get; set; }


        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and no more than {1}!")]
        public string Comment { get; set; } = string.Empty;


        //navigational properties
        public virtual BlogPost? BlogPost { get; set; }
        public virtual BlogUser? Author { get; set; }

        //public virtual BlogUser? Moderator { get; set; }
    }
}
