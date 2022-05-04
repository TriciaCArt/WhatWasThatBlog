using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

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
        [StringLength(40, ErrorMessage ="Nope. You are an idiot. Do you now know your own name?", MinimumLength =2)]
        public string? LastName { get; set; }
        public string? NickName { get; set; }
       



    }
}
