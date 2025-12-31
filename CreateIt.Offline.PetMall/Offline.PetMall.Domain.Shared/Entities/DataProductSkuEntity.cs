using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Offline.PetMall.Domain.Shared.Entities
{
    [SugarTable("Data_ProductSku")]
    public class DataProductSkuEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 商品主表ID
        /// </summary>
        public long Pid { get; set; }

        /// <summary>
        /// 规格编码
        /// </summary>
        public string SkuCode { get; set; }

        /// <summary>
        /// 原始规格名称
        /// </summary>
        public string OriginSkuName { get; set; }

        /// <summary>
        /// 规格名称
        /// </summary>
        public string SkuName { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int StockNum { get; set; }
    }
}
