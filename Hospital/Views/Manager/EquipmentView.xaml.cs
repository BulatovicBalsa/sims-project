using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Services.Manager;
using System.ComponentModel;

namespace Hospital.Views.Manager
{
    /// <summary>
    /// Interaction logic for EquipmentView.xaml
    /// </summary>
    public partial class EquipmentView : Window
    {
        public ObservableCollection<Models.Manager.Equipment> Equipment { get; set; }

        public bool _includeOperationEquipment;
        public bool IncludeOperationEquipment
        {
            get => _includeOperationEquipment;
            set
            {
                _includeOperationEquipment = value;
                OnPropertyChanged("IncludeOperationEquipment");
            }
        }

        private bool _includeHallwayEquipment;
        public bool IncludeHallwayEquipment { get => _includeHallwayEquipment;
            set
            {
                _includeHallwayEquipment = value;
                OnPropertyChanged("IncludeHallwayEquipment");
            }
        }

        private bool _includeFurniture;

        public bool IncludeFurniture
        {
            get => _includeFurniture;
            set
            {
                _includeFurniture = value;
                OnPropertyChanged("IncludeFurniture");
            }
        }

        private bool _includeExaminationEquipment;

        public bool IncludeExaminationEquipment
        {
            get => _includeExaminationEquipment;
            set
            {
                _includeExaminationEquipment = value;
                OnPropertyChanged("IncludeExaminationEquipment");
            }
        }

        public EquipmentView()
        {
            InitializeComponent();
            Equipment = new ObservableCollection<Models.Manager.Equipment>();
            this.DataContext = this;
            
            foreach (var e in EquipmentRepository.Instance.GetAll())
            {
                Equipment.Add(e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            UpdateEquipmentList();
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        private bool IsEquipmentTypeFilterOn()
        {
            return IncludeExaminationEquipment || IncludeFurniture || IncludeHallwayEquipment ||
                IncludeOperationEquipment;
        }
        public void UpdateEquipmentList()
        {
            Equipment.Clear();
            foreach (var e in EquipmentRepository.Instance.GetAll())
            {
                Equipment.Add(e);
            }

            var equipmentFilterService = new EquipmentFilterService();

            
            if (IsEquipmentTypeFilterOn())
            {
                List<Models.Manager.Equipment.EquipmentType> includedTypes = new List<Equipment.EquipmentType>();

                if (IncludeExaminationEquipment)
                {
                    includedTypes.Add(Models.Manager.Equipment.EquipmentType.ExaminationEquipment);
                }

                if (IncludeFurniture)
                {
                    includedTypes.Add(Models.Manager.Equipment.EquipmentType.Furniture);
                }

                if (IncludeHallwayEquipment)
                {
                    includedTypes.Add(Models.Manager.Equipment.EquipmentType.HallwayEquipment);
                }

                if (IncludeOperationEquipment)
                {
                    includedTypes.Add(Models.Manager.Equipment.EquipmentType.OperationEquipment);
                }

                Equipment.Clear();
                foreach (var e in EquipmentRepository.Instance.GetAll())
                {
                    if (includedTypes.Contains(e.Type)) Equipment.Add(e);
                }
            }
        }
    }
}
