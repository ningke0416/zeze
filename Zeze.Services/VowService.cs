using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeze.Common;
using Zeze.Domin.Models.Vows;
using Zeze.IRepository.Vows;
using Zeze.IServices;

namespace Zeze.Services
{
    public class VowService : IVowService
    {
        private readonly IVowRepository _vowRepository;

        public VowService(IVowRepository vowRepository)
        {
            _vowRepository = vowRepository;
        }

        /// <summary>
        /// 分页获取按钮列表
        /// </summary>
        /// <param name="baseQuery"></param>
        /// <param name="Total"></param>
        /// <returns></returns>
        //public (List<Vow>, int) GetVowPageList(Vow vow)
        //{
        //    var query = ExpressionBuilder.True<SysButton>();
        //    query = query.And(menu => menu.IsDrop == false);
        //    var menuquery = ExpressionBuilder.True<SysMenu>();
        //    menuquery = menuquery.And(menu => menu.IsDrop == false);
        //    if (!buttonQuery.Name.IsNullOrEmpty())
        //    {
        //        query = query.And(m => m.Name.Contains(buttonQuery.Name.Trim()));
        //    }
        //    if (!buttonQuery.JSKeyCode.IsNullOrEmpty())
        //    {
        //        query = query.And(m => m.KeyCode.Contains(buttonQuery.JSKeyCode.Trim()));
        //    }
        //    if (!buttonQuery.APIAddress.IsNullOrEmpty())
        //    {
        //        query = query.And(m => m.APIAddress.Contains(buttonQuery.APIAddress.Trim()));
        //    }
        //    if (!buttonQuery.MenuName.IsNullOrEmpty())
        //    {
        //        menuquery = menuquery.And(m => m.Name.Contains(buttonQuery.MenuName.Trim()));
        //    }
        //    var list = (from a in _buttonRepositoty.GetAll(query)
        //                join b in _menuRepositoty.GetAll(menuquery) on a.MenuId equals b.Id
        //                select new ButtonViewMoel
        //                {
        //                    Id = a.Id,
        //                    Name = a.Name,
        //                    APIAddress = a.APIAddress,
        //                    KeyCode = a.KeyCode,
        //                    Memo = a.Memo,
        //                    ButtonStyle = a.ButtonStyle,
        //                    IsShow = a.IsShow,
        //                    CreatedDate = a.CreatedDate,
        //                    MenuId = b.Id,
        //                    MenuName = b.Name,
        //                });
        //    int Total = list.Count();//查询符合添加的总数执行一次
        //    //添加排序
        //    OrderCondition<ButtonViewMoel>[] orderConditions = new OrderCondition<ButtonViewMoel>[]
        //    {
        //        new OrderCondition<ButtonViewMoel>(x=>x.CreatedDate,SortDirectionEnum.Descending)
        //    };
        //    Parameters parameters = new Parameters();
        //    parameters.PageIndex = buttonQuery.PageIndex;
        //    parameters.PageSize = buttonQuery.PageSize;

        //    parameters.OrderConditions = orderConditions;
        //    return (list.PageBy(parameters).ToList(), Total);//再查询符合条件的数据在查一次
        //}
        /// <summary>
        /// 获取所有的按钮列表
        /// </summary>
        /// <returns></returns>
        /// 
        [Caching(AbsoluteExpiration = 10)]
        public async Task<List<Vow>> GetVowList()
        {
            return await _vowRepository.GetAllListAsync();
        }

        /// <summary>
        /// 许愿
        /// </summary>
        /// <param name="vow"></param>
        /// <returns></returns>
        public async Task<bool> AddVow(Vow vow)
        {
            return await _vowRepository.InsertAsync(vow);
        }

        public async Task<bool> DeleteVow(Guid id)
        {

            var isDelete = await _vowRepository.DeleteAsync(id);
            return isDelete;
        }
    }
}
