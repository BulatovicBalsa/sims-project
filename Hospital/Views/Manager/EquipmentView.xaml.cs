using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Services.Manager;

namespace Hospital.Views.Manager;

/// <summary>
///     Interaction logic for EquipmentView.xaml
/// </summary>
public partial class EquipmentView : Window
{
    private bool _includeExaminationEquipment;

    private bool _includeFurniture;

    private bool _includeHallwayEquipment;

    private bool _includeOperationEquipment;

    public EquipmentView()
    {
        InitializeComponent();
        Equipment = new ObservableCollection<Equipment>();
        DataContext = this;

        foreach (var e in EquipmentRepository.Instance.GetAll()) Equipment.Add(e);
    }

    public ObservableCollection<Equipment> Equipment { get; set; }

    public bool IncludeOperationEquipment
    {
        get => _includeOperationEquipment;
        set
        {
            _includeOperationEquipment = value;
            OnPropertyChanged("IncludeOperationEquipment");
        }
    }

    public bool IncludeHallwayEquipment
    {
        get => _includeHallwayEquipment;
        set
        {
            _includeHallwayEquipment = value;
            OnPropertyChanged("IncludeHallwayEquipment");
        }
    }

    public bool IncludeFurniture
    {
        get => _includeFurniture;
        set
        {
            _includeFurniture = value;
            OnPropertyChanged("IncludeFurniture");
        }
    }

    public bool IncludeExaminationEquipment
    {
        get => _includeExaminationEquipment;
        set
        {
            _includeExaminationEquipment = value;
            OnPropertyChanged("IncludeExaminationEquipment");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string name)
    {
        UpdateEquipmentList();
        if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
    }


    private bool IsEquipmentTypeFilterOn()
    {
        return IncludeExaminationEquipment || IncludeFurniture || IncludeHallwayEquipment ||
               IncludeOperationEquipment;
    }

    public void UpdateEquipmentList()
    {
        Equipment.Clear();
        foreach (var e in EquipmentRepository.Instance.GetAll()) Equipment.Add(e);

        var equipmentFilterService = new EquipmentFilterService();


        if (!IsEquipmentTypeFilterOn()) return;
        
        var includedTypes = new List<Equipment.EquipmentType>();

        if (IncludeExaminationEquipment)
            includedTypes.Add(Models.Manager.Equipment.EquipmentType.ExaminationEquipment);

        if (IncludeFurniture) includedTypes.Add(Models.Manager.Equipment.EquipmentType.Furniture);

        if (IncludeHallwayEquipment) includedTypes.Add(Models.Manager.Equipment.EquipmentType.HallwayEquipment);

        if (IncludeOperationEquipment) includedTypes.Add(Models.Manager.Equipment.EquipmentType.OperationEquipment);

        Equipment.Clear();
        foreach (var e in EquipmentRepository.Instance.GetAll())
            if (includedTypes.Contains(e.Type))
                Equipment.Add(e);
        
    }
}