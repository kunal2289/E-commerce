using System.Reflection.Metadata;
namespace E_commerce.Models
{
    public class Order
    {
        public string orderId { get; set; }
        public string userId { get; set; }
        public string orderDate { get; set; }
        public decimal totalAmount { get; set; }
        public string status { get; set; }
    }
}
