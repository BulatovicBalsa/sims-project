﻿using System.Collections.Generic;
using System.Linq;
using Hospital.Core.Notifications.Models;
using Hospital.Core.Notifications.Repositories;
using Hospital.Core.PatientHealthcare.Services;

namespace Hospital.Core.Notifications.Services;

public class NotificationService
{
    private readonly NotificationRepository _notificationRepository;
    private readonly PatientService _patientService;

    public NotificationService()
    {
        _notificationRepository = new NotificationRepository();
        _patientService = new PatientService();
    }

    public List<Notification> GetAll(string forId)
    {
        return _notificationRepository.GetAll(forId);
    }

    public List<Notification> GetAllUnsent(string forId)
    {
        return GetAll(forId).Where(notification => notification.Sent == false).ToList();
    }

    public void Send(Notification notification)
    {
        _notificationRepository.Add(notification);
    }

    public void MarkSent(Notification notification)
    {
        notification.Sent = true;
        _notificationRepository.Update(notification);
    }
}