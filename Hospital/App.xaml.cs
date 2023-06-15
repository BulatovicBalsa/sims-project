using System;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Windows;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Core.Pharmacy.Services;
using Hospital.Core.PhysicalAssets.Services;
using Hospital.Core.Workers.Models;
using Hospital.GUI.Views;
using Hospital.GUI.Views.Accounts;
using Hospital.Injectors;
using Hospital.Notifications.Services;
using Hospital.Serialization;
using Hospital.Workers.Repositories;
using Timer = System.Timers.Timer;

namespace Hospital;

public partial class App : Application
{
    private const string _unsuccessfulLoginMessage = "Login was not successful.";
    private readonly MedicationOrderService _medicationOrderService = new();
    private readonly Timer _medicationOrderTimer = new(60000);

    private void ProcessEventsThatOccurredBeforeAppStart()
    {
        EquipmentOrderService.AttemptPickUpOfAllOrders();
        TransferService.AttemptDeliveryOfAllTransfers();
        _medicationOrderService.ExecuteMedicationOrders();
    }

    protected void ApplicationStart(object sender, EventArgs e)
    {
        CultureInfo.CurrentCulture = new CultureInfo("sr-RS");
        ProcessEventsThatOccurredBeforeAppStart();
        _medicationOrderTimer.Elapsed += ExecuteMedicationOrders;
        _medicationOrderTimer.Enabled = true;
        RoomOperationCompleter.TryCompleteAll();

        var loginView = new LoginView();
        loginView.Show();
        loginView.IsVisibleChanged += (s, ev) =>
        {
            if (loginView.IsVisible || !loginView.IsLoaded) return;

            var identityName = Thread.CurrentPrincipal.Identity.Name;
            var id = identityName.Split("|")[0];
            var role = identityName.Split("|")[1];

            if (role == "PATIENT")
            {
                var patient = PatientRepository.Instance.GetById(id);
                if (patient == null)
                {
                    MessageBox.Show(_unsuccessfulLoginMessage);
                    return;
                }

                if (patient.IsBlocked)
                {
                    MessageBox.Show("Your profile is blocked.");
                    return;
                }

                var patientView = new PatientView(patient);
                patientView.Show();
            }

            else if (role == "NURSE")
            {
                var nurseView = new NurseMainView();
                nurseView.Show();
            }

            else if (role == "MANAGER")
            {
                var managerView = new ManagerView();
                managerView.Show();
            }

            else if (role == "DOCTOR")
            {
                var doctor = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(id);
                if (doctor == null)
                {
                    MessageBox.Show(_unsuccessfulLoginMessage);
                    return;
                }

                var doctorView = new DoctorView(doctor);
                doctorView.Show();

                ShowNotifications(id);
            }

            loginView.Close();
        };
    }

    private void ShowNotifications(string id)
    {
        var notificationService = new NotificationService();
        var notificationsToShow = notificationService.GetAllUnsent(id);

        notificationsToShow.ForEach(notification =>
        {
            MessageBox.Show(notification.Message, "Notification");
            notificationService.MarkSent(notification);
        });
    }

    private void ExecuteMedicationOrders(object? source, ElapsedEventArgs e)
    {
        _medicationOrderService.ExecuteMedicationOrders();
    }
}