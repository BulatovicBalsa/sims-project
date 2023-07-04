using System;
using Hospital.Models.Memberships;

namespace Hospital.Models
{
    public class Member : Person
    {
        public string MembershipNumber { get; set; }
        public DateTime MembershipExpires { get; set; }
        public Membership Membership { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserType Type { get; set; }

        public Member() : base()
        {
            
        }

        public Member(string firstName, string lastName, DateTime birthDate, string email, string phoneNumber,
            string jmbg, string membershipNumber, DateTime membershipExpires, Membership membership,
            string username, string password) : base(firstName,
        lastName, jmbg, username, password)
        {
            MembershipNumber = membershipNumber;
            MembershipExpires = membershipExpires;
            Membership = membership;

            BirthDate = birthDate;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
