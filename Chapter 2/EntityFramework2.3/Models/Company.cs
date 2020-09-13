using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityFramework2._3
{
    //[NotMapped] - означає, що таблиця по заданії моделі не буде створюватись
    class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
