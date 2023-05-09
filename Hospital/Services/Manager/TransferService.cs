using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.Services.Manager
{
    public class TransferService
    {
        private static readonly Timer Timer = new(1500);

        static TransferService()
        {
            Timer.Enabled = true;
            Timer.AutoReset = true;
            Timer.Elapsed += (sender, _) => AttemptDeliveryOfAllTransfers();
        }

        public static void DisableAutomaticDelivery()
        {
            Timer.Enabled = false;
        }

        public static bool TrySendTransfer(Room origin, Room destination, List<TransferItem> items, DateTime deliveryDateTime)
        {
            
            var transfer = new Transfer(origin, destination, deliveryDateTime);
            foreach (var item in items)
            {
                transfer.AddItem(item);
            }

            if (!transfer.IsPossible())
            {
                return false;
            }

            transfer.Origin.ReserveEquipment(transfer);
            RoomRepository.Instance.Update(origin);
            TransferRepository.Instance.Add(transfer);
            return true;
        }

        private static void SaveChanges(Transfer transfer)
        {
            TransferRepository.Instance.Update(transfer);
            RoomRepository.Instance.Update(transfer.Origin);
            RoomRepository.Instance.Update(transfer.Destination);
        }

        public static void AttemptDeliveryOfAllTransfers()
        {
            var transfers = TransferRepository.Instance.GetAll();

            foreach (var transfer in transfers.ToList())
            {
                transfer.TryDeliver();
                try
                {
                    SaveChanges(transfer);
                }
                catch (System.IO.IOException e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }
    }
}
