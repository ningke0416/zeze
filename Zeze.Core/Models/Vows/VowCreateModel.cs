using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zeze.Core.Models.Vows
{
    public class VowCreateModel
    {
        /// <summary>
        /// 许愿者名称
        /// </summary>
        public string VowerName { get; set; }

        /// <summary>
        /// 装备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 装备属性
        /// </summary>
        public string EquipmentAttribute { get; set; }
    }
}
