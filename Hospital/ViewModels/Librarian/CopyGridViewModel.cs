using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Books;
using Hospital.Repositories.Books;
using Hospital.Serialization;
using Hospital.Views.Librarian;

namespace Hospital.ViewModels.Librarian
{
    public class CopyGridViewModel : ViewModelBase
    {
        private readonly CopyRepository _copyRepository;
        private ObservableCollection<Copy> _copies;
        private Copy? _selectedCopy;

        public CopyGridViewModel()
        {
            _copyRepository = new CopyRepository(new JsonSerializer<Copy>());
            _copies = new ObservableCollection<Copy>(_copyRepository.GetAll());

            AddCopyCommand = new ViewModelCommand(ExecuteAddCopyCommand);
            UpdateCopyCommand = new ViewModelCommand(ExecuteUpdateCopyCommand, IsCopySelected);
            DeleteCopyCommand = new ViewModelCommand(ExecuteDeleteCopyCommand, IsCopySelected);
        }

        public Copy? SelectedCopy
        {
            get => _selectedCopy;
            set
            {
                _selectedCopy = value;
                OnPropertyChanged(nameof(SelectedCopy));
            }
        }

        public ObservableCollection<Copy> Copies
        {
            get => _copies;
            set
            {
                _copies = value;
                OnPropertyChanged(nameof(Copies));
            }
        }

        public ICommand AddCopyCommand { get; }
        public ICommand UpdateCopyCommand { get; }
        public ICommand DeleteCopyCommand { get; }
        private void ExecuteAddCopyCommand(object obj)
        {
            var addCopyDialog = new AddCopyView();
            addCopyDialog.Closed += (sender, args) =>
            {
                Copies.Clear();
                _copyRepository.GetAll().ForEach(copy => Copies.Add(copy));
            };

            addCopyDialog.ShowDialog();
        }

        private void ExecuteUpdateCopyCommand(object obj)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void ExecuteDeleteCopyCommand(object obj)
        {
            _copyRepository.Delete(SelectedCopy);
            _copies.Remove(SelectedCopy);
            SelectedCopy = null;
        }

        private bool IsCopySelected(object obj)
        {
            return _selectedCopy != null;
        }
    }
}
