using System;

namespace Hospital.Models;

public class Notification
{
    public string Id { get; set; }
    public string ForId { get; set; }
    public string Message { get; set; }
    public bool Sent { get; set; }

    public Notification()
    {
        Id = Guid.NewGuid().ToString();
        ForId = "";
        Message = "";
        Sent = false;
    }
    public Notification(string forId, string message)
    {
        Id = Guid.NewGuid().ToString();
        ForId = forId;
        Message = message;
        Sent = false;
    }
}
