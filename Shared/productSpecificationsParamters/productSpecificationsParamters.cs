using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherd.productSpecificationsParamters
{
  public class productSpecificationsParamters
  {
    public int? CategoryId { get; set; }
    public int? TypeId { get; set; }
    public string? sort { get; set; }
    public string? Search { get; set; }

    private int _pageIndex = 1;
    private int _pageSize = 5;

    public int pageIndex
    { get
      {
        return _pageIndex;
      }
      set { _pageIndex = value; }
    }

    public int pageSize
    {
      get
      {
        return _pageSize;
      }
      set { _pageSize = value; }
    }

  }
}
