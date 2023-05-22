using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.Converters
{
    public class RoomIdToRoomConverter
    {
        public static Room? FromId(string id)
        {
            return RoomRepository.Instance.GetById(id);
        }
    }
}
