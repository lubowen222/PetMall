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
    public class JDProductRepository : Repository<JDProductEntity>, IJDProductRepository
    {
        private readonly ISqlSugarClient _client;
        public JDProductRepository(ISqlSugarClient sugarClient) : base(sugarClient)
        {
            _client = sugarClient;
        }
    }
}
