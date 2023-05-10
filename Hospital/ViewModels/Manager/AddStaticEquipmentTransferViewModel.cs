using System.ComponentModel;
using System.Linq;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.ViewModels.Manager;

public class AddStaticEquipmentTransferViewModel : AddTransferViewModelBase
{
    protected override void UpdateEquipmentList()
    {
        base.UpdateEquipmentList();
        Equipment = new BindingList<Equipment>(Equipment.Where(equipment =>
            equipment.Type != EquipmentType.DynamicEquipment).ToList());
    }
}