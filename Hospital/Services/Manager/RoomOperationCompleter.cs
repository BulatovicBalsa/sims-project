using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Hospital.Services.Manager
{
    public class RoomOperationCompleter
    {
        private static Timer Timer = new Timer(2000);
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
}
