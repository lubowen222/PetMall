using EasyFrame.SqlSugar;
using Offline.PetMall.Domain.Shared.Entities;
using Offline.PetMall.Domain.Shared.IRepositories;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offline.PetMall.Domain
{
    public class JDProductImgRepository : Repository<JDProductImgEntity>, IJDProductImgRepository
    {
        private readonly ISqlSugarClient _client;
        public JDProductImgRepository(ISqlSugarClient sugarClient) : base(sugarClient)
        {
            _client = sugarClient;
        }
    }
}
