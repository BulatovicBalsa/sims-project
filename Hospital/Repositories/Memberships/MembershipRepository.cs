﻿using Hospital.Models.Memberships;
using Hospital.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories.Memberships
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
            return GetAll().Find(membership => membership.Id == id);
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

            var indexToUpdate = allMembership.FindIndex(e => e.Id == membership.Id);
            if (indexToUpdate == -1) throw new KeyNotFoundException();

            allMembership[indexToUpdate] = membership;

            _serializer.Save(allMembership, FilePath);
        }

        public void Delete(Membership membership)
        {
            var allMembership = GetAll();

            var indexToDelete = allMembership.FindIndex(e => e.Id == membership.Id);
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
