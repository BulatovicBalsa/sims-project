using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Manager
{
    public class Transfer
    {
        public string Id { get; set; }
        public Room Origin { get; set; }
        public Room Destination { get; set; }
        public List<EquipmentTransferItem> Items { get; set; }
        public DateTime DeliveryDateTime { get; set; }

        public bool Delivered { get; set; }
        public bool Failed { get; set; }

        public bool IsPossible()
        {
            throw new NotImplementedException();
        }

        public bool TryDeliver()
        {
            throw new NotImplementedException();
        }

    }
}
