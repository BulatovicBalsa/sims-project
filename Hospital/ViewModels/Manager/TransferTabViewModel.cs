using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Views.Manager;

namespace Hospital.ViewModels.Manager;

public class TransferTabViewModel : ViewModelBase
{
    private Transfer _selectedTransfer;
    private ObservableCollection<TransferItem> _selectedTransferItems;
    private BindingList<Transfer> _transfers;

    public TransferTabViewModel()
    {
        Transfers = new BindingList<Transfer>(TransferRepository.Instance.GetAll());
        OpenAddStaticEquipmentTransferCommand =
            new RelayCommand(() => OpenAddTransferDialog(new AddStaticEquipmentTransfer()));
        OpenAddDynamicEquipmentTransferCommand =
            new RelayCommand(() => OpenAddTransferDialog(new AddDynamicEquipmentTransfer()));
    }

    public BindingList<Transfer> Transfers
    {
        get => _transfers;
        set
        {
            if (Equals(value, _transfers)) return;
            _transfers = value;
            OnPropertyChanged(nameof(Transfers));
        }
    }

    public Transfer SelectedTransfer
    {
        get => _selectedTransfer;
        set
        {
            if (Equals(value, _selectedTransfer)) return;
            _selectedTransfer = value;
            if (_selectedTransfer != null)
                SelectedTransferItems = new ObservableCollection<TransferItem>(_selectedTransfer.Items);
            OnPropertyChanged(nameof(SelectedTransfer));
        }
    }

    public ObservableCollection<TransferItem> SelectedTransferItems
    {
        get => _selectedTransferItems;
        set
        {
            if (Equals(value, _selectedTransferItems)) return;
            _selectedTransferItems = value;
            OnPropertyChanged(nameof(SelectedTransferItems));
        }
    }

    public ICommand OpenAddStaticEquipmentTransferCommand { get; set; }

    public ICommand OpenAddDynamicEquipmentTransferCommand { get; set; }

    private void RefreshTransfersOnFormClose(object? sender, EventArgs eventArgs)
    {
        Transfers = new BindingList<Transfer>(TransferRepository.Instance.GetAll());
    }

    public void OpenAddTransferDialog(Window dialog)
    {
        dialog.Closed += RefreshTransfersOnFormClose;
        dialog.Show();
    }
}