using System.Collections.Generic;
using System.Linq;
using Hospital.Injectors;
using Hospital.Models.Memberships;
using Hospital.Repositories.Memberships;
using Hospital.Serialization;

namespace Hospital.Services.Memberships;

public class SubscriptionService
{
    private readonly SubscriptionRepository _subscriptionRepository = new(SerializerInjector.CreateInstance<ISerializer<Subscription>>());

    private readonly MembershipRepository _membershipRepository = new(SerializerInjector.CreateInstance<ISerializer<Membership>>());

    public Membership? GetCurrentMembership(string memberId)
    {
        var subscription = _subscriptionRepository.GetAllForMember(memberId).MaxBy(sub => sub.Start) ?? throw new KeyNotFoundException();
        return _membershipRepository.GetById(subscription.MembershipId);
    }
}