using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
  public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
  {
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
      builder.HasOne(i => i.Product)
              .WithMany()
              .HasForeignKey(i => i.ProductId);
      builder.Property(i => i.Price)
       .HasPrecision(18, 2);
    }
  }
}
