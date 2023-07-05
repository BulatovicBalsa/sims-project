using System;

namespace Library.Models.Memberships;

public class Subscription
{
    public Subscription()
    {
        Id = Guid.NewGuid().ToString();
        MemberId = MembershipId = "";
    }

    public Subscription(string memberId, string membershipId, DateOnly start)
    {
        Id = Guid.NewGuid().ToString();
        MemberId = memberId;
        MembershipId = membershipId;
        Start = start;
    }

    public string Id { get; set; }
    public string MemberId { get; set; }
    public string MembershipId { get; set; }
    public DateOnly Start { get; set; }
}