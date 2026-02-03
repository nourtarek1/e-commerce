using Domain.Models.Identity;
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
  public class BasketConfiguration : IEntityTypeConfiguration<Basket>
  {
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
      builder.HasMany(b => b.Items)
              .WithOne(i => i.Basket)
              .HasForeignKey(i => i.BasketId)
              .OnDelete(DeleteBehavior.Cascade);

    }
  }
}
