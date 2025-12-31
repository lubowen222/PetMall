using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offline.PetMall.Domain.Shared.Entities
{
    /// <summary>
    /// 京东商品图片实体
    /// </summary>
    [SugarTable("JD_ProductImg")]
    public class JDProductImgEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 京东商品Id
        /// </summary>
        public long JDId{ get; set; }

        /// <summary>
        /// 图片类型
        /// </summary>
        public int ImgType { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatTime { get; set; }
    }
}
