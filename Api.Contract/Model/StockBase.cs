using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Contract.Model
{
    /// <summary>
    /// 表示个股基础信息的实体（简要信息），用于存储股票代码、名称及一些标识位。
    /// </summary>
    public class StockBase
    {

        /// <summary>
        /// 股票唯一代码（主键），例如 "000001"。
        /// </summary>
        [Key]
        public required string code { get; set; }

        /// <summary>
        /// 股票名称，例如 "平安银行"。
        /// </summary>
        public required string name { get; set; }

        /// <summary>
        /// 是否被用户标记为收藏。
        /// </summary>
        public bool isfavorite { get; set; }

        /// <summary>
        /// 是否在系统中被排除（例如不参与批量操作或计算）。
        /// </summary>
        public bool isexclude { get; set; }

    }
}
