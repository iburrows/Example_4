namespace Example_4.ViewModel
{
    public class ProductVM
    {

        public string ProductID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }

        public ProductVM(string productID, string name, double price, string type)
        {
            ProductID = productID;
            Name = name;
            Type = type;
            Price = price;
        }
    }
}