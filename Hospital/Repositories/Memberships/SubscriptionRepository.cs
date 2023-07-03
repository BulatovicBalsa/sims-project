using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Memberships;
using Hospital.Serialization;

namespace Hospital.Repositories.Memberships
{
    public class SubscriptionRepository
    {
        private const string FilePath = "../../../Data/subscriptions.csv";
        private readonly ISerializer<Subscription> _serializer;

        public SubscriptionRepository(ISerializer<Subscription> serializer)
        {
            _serializer = serializer;
        }

        public List<Subscription> GetAll()
        {
            return _serializer.Load(FilePath);
        }

        public Subscription? GetById(string id)
        {
            return GetAll().Find(subscription => subscription.Id == id);
        }

        public void Add(Subscription subscription)
        {
            var allSubscription = GetAll();

            allSubscription.Add(subscription);

            _serializer.Save(allSubscription, FilePath);
        }

        public void Update(Subscription subscription)
        {
            var allSubscription = GetAll();

            var indexToUpdate = allSubscription.FindIndex(e => e.Id == subscription.Id);
            if (indexToUpdate == -1) throw new KeyNotFoundException();

            allSubscription[indexToUpdate] = subscription;

            _serializer.Save(allSubscription, FilePath);
        }

        public void Delete(Subscription subscription)
        {
            var allSubscription = GetAll();

            var indexToDelete = allSubscription.FindIndex(e => e.Id == subscription.Id);
            if (indexToDelete == -1) throw new KeyNotFoundException();

            allSubscription.RemoveAt(indexToDelete);

            _serializer.Save(allSubscription, FilePath);
        }

        public void DeleteAll()
        {
            var emptySubscriptionList = new List<Subscription>();
            _serializer.Save(emptySubscriptionList, FilePath);
        }

        public List<Subscription> GetAllForMember(string memberId)
        {
            return GetAll().Where(sub => sub.MemberId == memberId).ToList();
        }

        public List<Subscription> GetAllForMembership(string membershipId)
        {
            return GetAll().Where(sub => sub.MemberId == membershipId).ToList();
        }
    }
}
