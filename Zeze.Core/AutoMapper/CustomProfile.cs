using AutoMapper;
using Zeze.Core.Models.Vows;
using Zeze.Domin.Models.Vows;

namespace Zeze.Core.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<VowCreateModel, Vow>();
        }
    }
}
