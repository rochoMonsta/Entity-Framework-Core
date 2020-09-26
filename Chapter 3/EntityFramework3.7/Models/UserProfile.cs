using Microsoft.EntityFrameworkCore;

namespace EntityFramework3._7.Models
{
    //[Owned]
    class UserProfile
    {
        public Claim Name { get; set; }
        public Claim Surname { get; set; }
        public Claim Age { get; set; }
    }
}
