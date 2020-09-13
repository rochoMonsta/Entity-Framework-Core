namespace EntityFramework2._3
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public Company Company { get; set; }
        public override string ToString()
        {
            return $"Product id: {this.Id};\nName: {this.Name};\nPrice: {this.Price};\nCompany: {this.Company};\n";
        }
    }
}
