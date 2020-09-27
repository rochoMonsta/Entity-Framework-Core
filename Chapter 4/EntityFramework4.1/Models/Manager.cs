namespace EntityFramework4._1.Models
{
    class Manager : Employee
    {
        public string Departament { get; set; }
        public override string ToString() => base.ToString() + $" {this.Departament}";
    }
}
