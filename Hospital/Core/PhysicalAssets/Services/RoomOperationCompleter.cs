using System.Timers;

namespace Hospital.Core.PhysicalAssets.Services;

public class RoomOperationCompleter
{
    private static readonly Timer Timer = new(2000);

    static RoomOperationCompleter()
    {
        Timer.Enabled = true;
        Timer.AutoReset = true;
        Timer.Elapsed += (sender, args) => TryCompleteAll();
    }

    public static void TryCompleteAll()
    {
        TransferService.AttemptDeliveryOfAllTransfers();
        EquipmentOrderService.AttemptPickUpOfAllOrders();
        ComplexRenovationService.TryCompleteAll();
    }
}