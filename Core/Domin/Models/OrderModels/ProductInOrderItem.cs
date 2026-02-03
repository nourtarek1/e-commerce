namespace Domin.Models.OrderModels
{
  public class ProductInOrderItem
  {
    public ProductInOrderItem(int productId, string productName, string picturUrl)
    {
      ProductId = productId;
      ProductName = productName;
      PicturUrl = picturUrl;
    }

    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string PicturUrl { get; set; }
  }
}
