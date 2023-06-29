using EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCore.Configurations
{
    public class BlogEntityTypeConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.Property(m => m.Url)
            .IsRequired(false)
            .HasDefaultValue(null);
            //.HasColumnName("BlogUrl");
            //.HasColumnType("varchar")

            builder.Ignore(m => m.AddedOn);


            //builder.Property(m => m.Url).HasColumnType("varchar(200)");
            //builder.Property(m => m.Rating).HasColumnType("decimal(5,2)");
            //builder.Property(m => m.Url).HasMaxLength(50);
            //builder.Property(m => m.Url).HasComment("The URL of the blog");
        }
    }
}
