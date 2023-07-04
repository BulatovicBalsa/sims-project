using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Memberships;

namespace Hospital.Models
{
    public class Member : User
    {
        public string MembershipNumber { get; set; }
        public DateTime MembershipExpires { get; set; }
        public Membership Membership { get; set; }

        public Member() : base()
        {
            
        }

        public Member(string firstName, string lastName, DateTime birthDate, string email, string phoneNumber,
            string jmbg, string membershipNumber, DateTime membershipExpires, Membership membership,
            string username, string password) : base(firstName, lastName, birthDate, 
            email, phoneNumber, jmbg, UserType.Member, username, password)
        {
            MembershipNumber = membershipNumber;
            MembershipExpires = membershipExpires;
            Membership = membership;
        }
    }
}
