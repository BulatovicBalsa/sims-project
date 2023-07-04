using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Hospital.Models;
using Hospital.Models.Memberships;
using Hospital.Repositories.Memberships;

namespace Hospital.Serialization.Mappers
{
    public sealed class MemberReadMapper : ClassMap<Member>
    {
        public MemberReadMapper()
        {
            Map(member => member.Id).Index(0);
            Map(member => member.FirstName).Index(1);
            Map(member => member.LastName).Index(2);
            Map(member => member.BirthDate).Index(3);
            Map(member => member.Email).Index(4);
            Map(member => member.PhoneNumber).Index(5);
            Map(member => member.JMBG).Index(6);
            Map(member => member.Type).Index(7);
            Map(member => member.Profile.Username).Index(8);
            Map(member => member.Profile.Password).Index(9);
            Map(member => member.MembershipNumber).Index(10);
            Map(member => member.MembershipExpires).Index(11);
            Map(member => member.Membership.Id).Index(12).TypeConverter<MembershipTypeConverter>();
        }

        public class MembershipTypeConverter : DefaultTypeConverter
        {
            private MembershipRepository membershipRepository = new MembershipRepository(new CsvSerializer<Membership>());

            public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
            {
                return membershipRepository.GetById(text);
            }
        }
    }
}
