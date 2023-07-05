using System.Collections.Generic;
using Library.Models.Memberships;
using Library.Serialization;

namespace Library.Repositories.Memberships
{
    public class MembershipRepository
    {
        private const string FilePath = "../../../Data/memberships.csv";
        private readonly ISerializer<Membership> _serializer;

        public MembershipRepository(ISerializer<Membership> serializer)
        {
            _serializer = serializer;
        }

        public List<Membership> GetAll()
        {
            return _serializer.Load(FilePath);
        }

        public Membership? GetById(string id)
        {
            return GetAll().Find(membership => membership.TypeId == id);
        }

        public void Add(Membership membership)
        {
            var allMembership = GetAll();

            allMembership.Add(membership);

            _serializer.Save(allMembership, FilePath);
        }

        public void Update(Membership membership)
        {
            var allMembership = GetAll();

            var indexToUpdate = allMembership.FindIndex(e => e.TypeId == membership.TypeId);
            if (indexToUpdate == -1) throw new KeyNotFoundException();

            allMembership[indexToUpdate] = membership;

            _serializer.Save(allMembership, FilePath);
        }

        public void Delete(Membership membership)
        {
            var allMembership = GetAll();

            var indexToDelete = allMembership.FindIndex(e => e.TypeId == membership.TypeId);
            if (indexToDelete == -1) throw new KeyNotFoundException();

            allMembership.RemoveAt(indexToDelete);

            _serializer.Save(allMembership, FilePath);
        }

        public void DeleteAll()
        {
            var emptyMembershipList = new List<Membership>();
            _serializer.Save(emptyMembershipList, FilePath);
        }
    }
}
