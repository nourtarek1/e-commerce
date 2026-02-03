
using Domain.Models;
using Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
  public class Basket:BaseEntity<int>
  {
    public string AppUserId { get; set; } = null!;
    public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();
 
  }
}
