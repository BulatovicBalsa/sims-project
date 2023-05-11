using System.ComponentModel;
using System.Linq;
using Hospital.Models.Manager;

namespace Hospital.ViewModels.Manager;

public class AddStaticEquipmentTransferViewModel : AddTransferViewModelBase
{
    protected override void UpdateEquipmentList()
    {
        base.UpdateEquipmentList();
        var avaliableStaticEquipmentAtOrigin = Equipment.Where(equipment =>
            equipment.Type != EquipmentType.DynamicEquipment).ToList();
        Equipment = new BindingList<Equipment>(avaliableStaticEquipmentAtOrigin);
    }
}