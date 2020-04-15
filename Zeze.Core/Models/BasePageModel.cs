using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zeze.Core.Models
{
    public class BasePageModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;
        
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; } = 15;
    }
}
