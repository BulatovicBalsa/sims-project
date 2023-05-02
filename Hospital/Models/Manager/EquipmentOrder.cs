using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hospital.Models.Manager
{
    public class EquipmentOrder
    {
        public List<EquipmentOrderItem> items;

        public EquipmentOrder()
        {
            items = new List<EquipmentOrderItem>();
        }

        public void AddOrUpdateItem(Equipment equipment, uint amount)
        {
            EquipmentOrderItem item = items.Find(e => e.EquipmentId == equipment.Id);
            if (item == null)
                items.Add(new EquipmentOrderItem(amount, equipment.Id, equipment));
            else
                item.Amount = amount;
        }
    }
}