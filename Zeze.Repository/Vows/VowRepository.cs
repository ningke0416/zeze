using System;
using Zeze.Domin.Data;
using Zeze.Domin.Models.Vows;
using Zeze.IRepository.Vows;

namespace Zeze.Repository.Vows
{
    public class VowRepository : BaseRepository<Vow, Guid>, IVowRepository
    {
        public VowRepository(BaseContext baseContext) : base(baseContext)
        {


        }
    }
}
