using System.Collections.Generic;
using System.Linq;
using Library.Injectors;
using Library.Models.Memberships;
using Library.Repositories.Memberships;
using Library.Serialization;

namespace Library.Services.Memberships;

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