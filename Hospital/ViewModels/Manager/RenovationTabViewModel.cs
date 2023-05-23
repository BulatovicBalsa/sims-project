using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Services.Manager;

namespace Hospital.ViewModels.Manager
{
    public class RenovationTabViewModel: ViewModelBase
    {
        private BindingList<Renovation> _renovations;

        public BindingList<Renovation> Renovations
        {
            get => _renovations;
            set
            {
                if (Equals(value, _renovations)) return;
                _renovations = value;
                OnPropertyChanged(nameof(Renovations));
            }
        }

        public RenovationTabViewModel()
        {

            _renovations = new BindingList<Renovation>(RenovationRepository.Instance.GetAll());
        }
    }
}
