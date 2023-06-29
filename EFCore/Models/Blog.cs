using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Models
{
    public class Blog
    {
        public int Id { get; set; }

        //[Column("BlogUrl")]
        //[Column(TypeName = "varchar(200)")]
        //[MaxLength(200)]
        //[Comment("The URL for the blog")]
        public string Url { get; set; }

        //[NotMapped] // Exclude this property from being mapped to DB
        public DateTime AddedOn { get; set; }

        ////[Column(TypeName = "decimal(5,2)")]
        //public int Rating { get; set; }

        // [NotMapped]
        public List<Post> Posts { get; set; } // Navigation property
        /*
         * Blog has many Posts.
         * Post belong to one Blog.
         */
    }
}
