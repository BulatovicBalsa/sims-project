﻿using Hospital.Serialization;
using System.Collections.Generic;
using System.Linq;
using Hospital.Models;

namespace Hospital.Repositories
{
    public class MemberRepository
    {
        private const string FilePath = "../../../Data/members.csv";
        public List<Member> GetAll()
        {
            return CsvSerializer<Member>.FromCSV(FilePath);
        }

        public Member? GetById(string id)
        {
            return GetAll().FirstOrDefault(member => member.Id == id);
        }

        public Member? GetByUsername(string username)
        {
            return GetAll().FirstOrDefault(member => member.Profile.Username == username);
        }

        public void Add(Member member)
        {
            var allMembers = GetAll();
            allMembers.Add(member);
            CsvSerializer<Member>.ToCSV(allMembers, FilePath);
        }

        public void Update(Member member)
        {
            var allMembers = GetAll();

            var indexToUpdate = allMembers.FindIndex(m => m.Id == member.Id);
            if (indexToUpdate == -1)
                throw new KeyNotFoundException($"Member with id {member.Id} was not found.");
            allMembers[indexToUpdate] = member;

            CsvSerializer<Member>.ToCSV(allMembers, FilePath);
        }

        public void Delete(Member member)
        {
            var allMembers = GetAll();

            if (!allMembers.Remove(member))
                throw new KeyNotFoundException($"Member with id {member.Id} was not found.");

            CsvSerializer<Member>.ToCSV(allMembers, FilePath);
        }

        public static void DeleteAll()
        {
            var emptyMemberList = new List<Member>();
            CsvSerializer<Member>.ToCSV(emptyMemberList, FilePath);
        }
    }
}
