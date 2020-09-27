namespace EntityFramework4._1.Models
{
    class Employee : User
    {
        public int Salary { get; set; }
        public override string ToString() => base.ToString() + $" {this.Salary}";
    }
}
