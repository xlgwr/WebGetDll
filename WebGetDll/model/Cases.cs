using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGetDll.model
{
    public class Cases
    {
        public Cases()
        {
            this.t_Case = new t_Case();
            this.Plaintiffs = new List<person>();
            this.Defendants = new List<person>();
            this.Plaintiffs_Representations = new List<person>();
            this.Defendants_Representations = new List<person>();
            this.Judges = new List<person>();
        }
        /// <summary>
        /// 案件信息
        /// </summary>
        public t_Case t_Case { get; set; }
        /// <summary>
        /// 原告
        /// </summary>
        public List<person> Plaintiffs { get; set; }
        /// <summary>
        /// 被告
        /// </summary>
        public List<person> Defendants { get; set; }
        /// <summary>
        /// 原告代表
        /// </summary>
        public List<person> Plaintiffs_Representations { get; set; }
        /// <summary>
        /// 被告代表
        /// </summary>
        public List<person> Defendants_Representations { get; set; }
        /// <summary>
        /// 法官
        /// </summary>
        public List<person> Judges { get; set; }
    }
    public class person
    {
        public person()
        {
            this.Type = 0;
        }
        /// <summary>
        /// 英文名字
        /// </summary>
        public string FullName_En { get; set; }
        /// <summary>
        /// 中文名字
        /// </summary>
        public string FullName_Cn { get; set; }
        /// <summary>
        /// 公司/个人 0:个人1:公司2:法官  默认为个人
        /// </summary>
        public int Type { get; set; }
    }
}
