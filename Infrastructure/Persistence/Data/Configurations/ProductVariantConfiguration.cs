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
  public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
  {
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
      builder.HasKey(pv => pv.Id);

      builder.HasOne(pv => pv.Product)
             .WithMany(p => p.Variants)
             .HasForeignKey(pv => pv.ProductId)
             .OnDelete(DeleteBehavior.Cascade);

      builder.Property(pv => pv.Color)
             .IsRequired()
             .HasMaxLength(50);

      builder.Property(pv => pv.Size)
             .IsRequired()
             .HasMaxLength(20);

      builder.Property(pv => pv.StockQuantity)
             .IsRequired();

      // ✅ يمنع تكرار نفس (Color + Size) لنفس المنتج
      builder.HasIndex(pv => new { pv.ProductId, pv.Color, pv.Size })
             .IsUnique();
    }
  }
}
