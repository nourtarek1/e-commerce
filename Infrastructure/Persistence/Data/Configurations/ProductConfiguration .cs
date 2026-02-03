using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configuretion
{
  public class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);

      builder.Property(p => p.Description)
             .HasMaxLength(500);

      builder.Property(p => p.Price)
             .HasColumnType("decimal(18,2)");

      builder.Property(p => p.ImageUrl)
             .HasMaxLength(300);

      

      builder.Property(p => p.CreatedAt)
             .HasDefaultValueSql("GETUTCDATE()");
      builder.HasOne(p => p.Category)
              .WithMany(c => c.Products)
              .HasForeignKey(p => p.CategoryId)
              .OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(p => p.Type)
              .WithMany(t => t.Products)
              .HasForeignKey(p => p.TypeId)
               .OnDelete(DeleteBehavior.Restrict);

      builder.HasMany(p => p.Variants)
         .WithOne(v => v.Product)
         .HasForeignKey(v => v.ProductId)
         .OnDelete(DeleteBehavior.Cascade);


    }
  }
}
