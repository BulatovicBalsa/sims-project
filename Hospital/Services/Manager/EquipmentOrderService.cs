using System.Collections.Generic;
using System.Timers;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.Services.Manager;

public class EquipmentOrderService
{
    private static readonly Timer Timer = new(1000);

    static EquipmentOrderService()
    {
        Timer.Enabled = true;
        Timer.AutoReset = true;
        Timer.Elapsed += (sender, _) => AttemptPickUpOfAllOrders();
    }

    public static void SendOrder(List<EquipmentOrderItem> items)
    {
        var order = EquipmentOrder.CreateBlankOrder();
        foreach (var item in items) order.Items.Add(item);

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