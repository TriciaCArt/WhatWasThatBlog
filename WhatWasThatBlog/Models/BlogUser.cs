using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatWasThatBlog.Models
{
    public class BlogUser : IdentityUser
    {

        [Required]
        [Display(Name="First Name")]
        [StringLength(40, ErrorMessage = "Go pound Sand and enter a real name, ya loser!", MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name ="Last Name")]
        [StringLength(40, ErrorMessage = "Nope. Try again.", MinimumLength =2)]
        public string? LastName { get; set; }

        public string? NickName { get; set; }


        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }
        [NotMapped]
        [Display(Name = "Choose an Image")]
        public IFormFile? ImageFile { get; set; }


        [NotMapped]
        public string? FullName => $"{FirstName} {LastName}";


        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();
        public virtual ICollection<BlogPostComment> BlogPostComments { get; set; } = new HashSet<BlogPostComment>();



    }
}
