using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.Services.Manager
{
    public class EquipmentOrderService
    {
        public static void SendOrder(List<EquipmentOrderItem> items)
        {
            var order = EquipmentOrder.CreateBlankOrder();
            foreach (var item in items)
            {
                order.Items.Add(item);
            }

            EquipmentOrderRepository.Instance.Add(order);
        }

        public static void SendOrder(EquipmentOrder order)
        {
            EquipmentOrderRepository.Instance.Add(order);
        }

        public static void AttemptPickUpOfAllOrders()
        {
            foreach (var order in EquipmentOrderRepository.Instance.GetAll())
            {
                order.PickUp(RoomRepository.Instance.GetWarehouse());
                EquipmentOrderRepository.Instance.Update(order);
            }
        }

        
    }
}
