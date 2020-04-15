
using System;
using Zeze.Domin.Models.Users;

namespace Zeze.Domin.Models.Vows
{
    public class Vow : BaseEntity
    {
        public string VowerName { get; set; }

        public string EquipmentName { get; set; }

        public string EquipmentAttribute { get; set; }

        //public Guid UserId { get; set; }

        //public User User { get; set; }

    }
}
