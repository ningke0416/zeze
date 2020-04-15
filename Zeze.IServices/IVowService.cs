using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zeze.Domin.Models.Vows;

namespace Zeze.IServices
{
    public interface IVowService
    {
        //(List<Vow>, int) GetVowPageList(Vow vow);

        Task<List<Vow>> GetVowList();

        Task<bool> AddVow(Vow vow);

        Task<bool> DeleteVow(Guid id);
    }
}
