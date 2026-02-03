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
  public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
  {
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
      builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(50);

      builder.HasMany(t => t.Products)
             .WithOne(p => p.Type)
             .HasForeignKey(p => p.TypeId)
             .OnDelete(DeleteBehavior.Restrict);

    }
  }
}
