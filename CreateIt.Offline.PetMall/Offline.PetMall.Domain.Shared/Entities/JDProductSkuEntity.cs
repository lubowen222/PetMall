using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offline.PetMall.Domain.Shared.Entities
{
    /// <summary>
    /// 京东商品规格实体
    /// </summary>
    [SugarTable("JD_ProductSku")]
    public class JDProductSkuEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 京东商品Id
        /// </summary>
        public long JDId { get; set; }

        /// <summary>
        /// 规格名称
        /// </summary>
        public string SkuName { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal SalePrice { get; set; }
    }
}
