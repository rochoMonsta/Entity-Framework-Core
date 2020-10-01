using System.Collections.Generic;

namespace EntityFramework5._4.Models
{
    class Country
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Company> Companies { get; set; }
    }
}
