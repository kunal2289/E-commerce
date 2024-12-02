using System.Reflection.Metadata;

namespace E_commerce.Models
{
    public class Product
    {
        public string productId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string category { get; set; }
        public int stock { get; set; }
        public byte[] image
        {
            get; set;
        }
    }
}
