using CsvHelper.Configuration;
using Library.Models;

namespace Library.Serialization.Mappers
{
    public sealed class MemberWriteMapper : ClassMap<Member>
    {
        public MemberWriteMapper()
        {
            Map(member => member.Id).Index(0);
            Map(member => member.FirstName).Index(1);
            Map(member => member.LastName).Index(2);
            Map(member => member.BirthDate).Index(3);
            Map(member => member.Email).Index(4);
            Map(member => member.PhoneNumber).Index(5);
            Map(member => member.Jmbg).Index(6);
            Map(member => member.Type).Index(7);
            Map(member => member.Profile.Username).Index(8);
            Map(member => member.Profile.Password).Index(9);
            Map(member => member.MembershipNumber).Index(10);
            Map(member => member.MembershipExpires).Index(11);
            Map(member => member.Membership.TypeId).Index(12);
        }
    }
}
