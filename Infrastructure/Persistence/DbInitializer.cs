using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Domin.Models.OrderModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Sherd;
using Sherd.Seed;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Persistence
{
  public class DbInitializer : IDbInitializer
  {
    private readonly StoreDbContext _StoreContext;
    private readonly AuthDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(
            StoreDbContext StoreContext,
        AuthDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
      _StoreContext = StoreContext;
      _context = context;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
      if (!_StoreContext.Categories.Any())
      {

        // 1.Read All Data From Types json to String
        var categoriesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\Category.json"); 
        // 2. TransForm To String To C# object [List<ProductType>]
        var types = JsonSerializer.Deserialize<List<Category>>(categoriesData);

        // 3. Add To DataBase
        if (types is not null && types.Any())
        {
          await _StoreContext.Categories.AddRangeAsync(types);
          await _StoreContext.SaveChangesAsync();
        }
      }
      ////Seeding ProductTypes From Jeson Files
      if (!_StoreContext.ProductTypes.Any())
      {

        // 1.Read All Data From Types json to String
        var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json"); 

        // 2. TransForm To String To C# object [List<ProductType>]
        var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

        // 3. Add To DataBase
        if (types is not null && types.Any())
        {
          await _StoreContext.ProductTypes.AddRangeAsync(types);
          await _StoreContext.SaveChangesAsync();
        }
      }
      ////Seeding Products From Jeson Files
      if (!_StoreContext.Products.Any())
      {
        try
        {
          var productSeedData =
              await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

          var options = new JsonSerializerOptions
          {
            PropertyNameCaseInsensitive = true
          };

          var seedProducts =
              JsonSerializer.Deserialize<List<ProductSeedDto>>(productSeedData, options);

          if (seedProducts != null && seedProducts.Any())
          {
            foreach (var p in seedProducts)
            {
              var product = new Product
              {
                Name = p.Name,
                Description = string.IsNullOrWhiteSpace(p.Description)
                      ? "No description provided."
                      : p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                TypeId = p.ProductTypeId,
                CategoryId = p.CategoryId,
                CreatedAt = DateTime.Now,

                Variants = p.Variants?.Select(v => new ProductVariant
                {
                  Color = v.Color,          // ✅ مهم
                  Size = v.Size,            // ✅ مهم
                  StockQuantity = v.StockQuantity
                }).ToList()
              };

              await _StoreContext.Products.AddAsync(product);
            }

            await _StoreContext.SaveChangesAsync();
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"❌ Error Seeding Products: {ex.Message}");
        }
      }

      

      ////Seeding delivery From Jeson Files
      if (!_StoreContext.DeliveryMethods.Any())
      {

        // 1.Read All Data From delivery json to String
        var deliveryData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\delivery.json");

        // 2. TransForm To String To C# object [List<deliveryData>]
        var types = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

        // 3. Add To DataBase
        if (types is not null && types.Any())
        {
          await _StoreContext.DeliveryMethods.AddRangeAsync(types);
          await _StoreContext.SaveChangesAsync();
        }
      }
    }



    public async Task InitializeIdentityAsync()
    {
      if (_context.Database.GetPendingMigrations().Any())
      {
        await _context.Database.MigrateAsync();
      }

      if (!_roleManager.Roles.Any())
      {
        await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
      }

      if (await _userManager.FindByNameAsync("SuperAdmin") == null)
      {
        var superAdminUser = new AppUser
        {
          DisplayName = "Super Admin",
          Email = "SuperAdmin@gmail.com",
          UserName = "SuperAdmin",
          PhoneNumber = "0123456789"
        };
        await _userManager.CreateAsync(superAdminUser, "0108584583aA@");
        await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
      }

      if (await _userManager.FindByNameAsync("Admin") == null)
      {
        var adminUser = new AppUser
        {
          DisplayName = "Admin",
          Email = "Admin@gmail.com",
          UserName = "Admin",
          PhoneNumber = "0123456789"
        };
        await _userManager.CreateAsync(adminUser, "0108584583aA#");
        await _userManager.AddToRoleAsync(adminUser, "Admin");
      }
    }




   

  }




}
