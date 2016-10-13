using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGetDll.model
{
    /// <summary>
    /// 将案件实例传递给解析程序(dll),返回解析后的内容实体对象。
    /// </summary>
    public class responCaseData
    {
        public responCaseData()
        {
            this.Cases = new Cases();
        }
        /// <summary>
        /// ReturnStatus=0时：
        /// 案件实体对象
        /// （包括如下：案件信息，原告list,被告list,原告代表list，被告代表list，法官list）
        /// </summary>
        public Cases Cases { get; set; }

        /// <summary>
        /// 成功：0  失败：其他, 必输
        /// </summary>
        public int ReturnStatus { get; set; }
        /// <summary>
        /// 错误提示,必输
        /// </summary>
        public string message { get; set; }
    }
}
