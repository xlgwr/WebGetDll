using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGetDll.model
{
    public class t_Case
    {
        /// <summary>
        /// 网页采集标记
        /// </summary>
        public string tname { get; set; }
        /// <summary>
        /// 案件id 返回时留空
        /// </summary>
        public string Caseid { get; set; }
        /// <summary>
        /// 案件编号 英文
        /// </summary>
        public string CaseNo { get; set; }
        /// <summary>
        /// 案件编号 默认CaseNo
        /// /////////////////////////////////////////////////////////////////////
        /// </summary>
        public string CaseNoNew { get; set; }

        /// <summary>
        /// 中文案件编号
        /// </summary>
        public string CaseNo_Cn { get; set; }

        /// <summary>
        /// 次数[2/4]#
        /// </summary>
        public string NumberTimes { get; set; }

        /// <summary>
        /// 案件前缀 case no 前 字母
        /// </summary>        
        public string CaseTypeID { get; set; }      


        /// <summary>
        /// 案件年份 case no 中的年
        /// </summary>
        public string Year { get; set; }


        /// <summary>
        /// 序号 case no 中的年字符前数字
        /// //////////////////////////////////////////////////////////////////////
        /// </summary>
        public string SerialNo { get; set; }


        /// <summary>
        /// 
        /// 开庭日期
        /// </summary>       
        public string CourtDay { get; set; }

        /// <summary>
        /// 开庭时间
        /// </summary>
        public string OpenCourtTime { get; set; }
        /// <summary>
        /// 原告
        /// </summary>       
        public string Plaintiff { get; set; }


        /// <summary>
        /// 被告
        /// </summary>       
        public string Defendant { get; set; }

        ///// <summary>
        ///// 原因
        ///// </summary>       
        //public string Cause { get; set; }

        /// <summary>
        /// 性质
        /// </summary>       
        public string Nature { get; set; }


        /// <summary>
        /// 法官
        /// </summary>
        public string Judge { get; set; }

        /// <summary>
        /// 应讯代表
        /// </summary>
        public string Representation { get; set; }

        /// <summary>
        /// 聆讯
        /// </summary>
        public string Hearing { get; set; }
        /// <summary>
        /// 计划行动日期
        /// </summary>
        public string Actiondate { get; set; }
        /// <summary>
        /// 币别
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 检验区
        /// </summary>
        public string CheckField { get; set; }

        #region add new by 2016-08-25
        /// <summary>
        /// 法院id
        /// </summary>
        public virtual long CourtID { get; set; }
        /// <summary>
        /// 数据级别(0:公开 1:内部人员可见 2:主管可见 3:超级用户可见)
        /// </summary>
        public virtual string DataGradeID { get; set; }
        /// <summary>
        /// 被告地址
        /// </summary>
        public virtual string D_Address { get; set; }
        /// <summary>
        /// HtmlID
        /// </summary>
        public virtual long HtmlID { get; set; }

        /// <summary>
        /// 有无判决书(0:无 1:有)
        /// </summary>
        public virtual int Judgement { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public virtual string Other { get; set; }

        /// <summary>
        /// 其他1
        /// </summary>
        public virtual string Other1 { get; set; }

        /// <summary>
        /// 原告地址
        /// </summary>
        public virtual string P_Address { get; set; }
        /// <summary>
        /// 当事人各方（原告，被告的原始记录)
        /// </summary>
        public virtual string Parties { get; set; }

        /// <summary>
        /// 原告代表
        /// </summary>
        public virtual string Representation_P { get; set; }

        /// <summary>
        /// 被告代表
        /// </summary>
        public virtual string Representation_D { get; set; }
        #endregion
    }
}
