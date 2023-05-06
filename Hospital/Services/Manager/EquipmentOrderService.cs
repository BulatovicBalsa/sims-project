using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Timer = System.Timers.Timer;

namespace Hospital.Services.Manager
{
    public class EquipmentOrderService
    {
        private static System.Timers.Timer timer = new (1000);

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

        static EquipmentOrderService()
        {
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Elapsed += (sender, _) => AttemptPickUpOfAllOrders();
        }

        
    }
}
