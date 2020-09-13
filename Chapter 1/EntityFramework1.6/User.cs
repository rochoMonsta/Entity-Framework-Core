namespace EntityFramework
{
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public override string ToString()
        {
            return $"ID: {this.Id};\nName: {this.Name};\nSurname: {this.Surname};\nAge: {this.Age};\n";
        }
    }
}
