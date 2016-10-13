using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFLibForApi.emms;
using WebGetDll.model;
using System.Text.RegularExpressions;
using EFLibForApi.emms.models;

namespace WebGetDllConsoleTest
{
    class Program
    {
        static emmsApiDbContext db = new emmsApiDbContext();
        static void Main(string[] args)
        {
            //var tmpReg = new Regex(@"\“", RegexOptions.IgnoreCase);
            //var str = "xlg-“wr,co-“me china.";
            //str = tmpReg.Replace(str, " ");

            //var str0 = "陳建平 CHAN, KIN-PING";
            //var regxRA1Name = new Regex(@"[\u4e00-\u9fa5]+ [A-Za-z]+\, [A-Za-z]+", RegexOptions.IgnoreCase);//
            //var dd = regxRA1Name.IsMatch(str0);

            //testCaseNoConsole();
            //testPlaintiffsConsole();
            //testDB(0, DateTime.Now);
            //testRemoveSplic();
            testPlaintiffsConsole2();
            Console.Read();
        }
        static void testT()
        {

            var tmp1 = "被告人Defendant(s):@VIRGEN JIMENEZ IGNACIO GUILLERMO@第三債務人GARNISHEE(s):@THE HONGKONG AND SHANGHAI BANKING CORPORATION LIMITED";
            var tmp10 = "被告人 Defendant(s):@VIRGEN JIMENEZ IGNACIO GUILLERMO@第三債務人GARNISHEE(s):@THE HONGKONG AND SHANGHAI BANKING CORPORATION LIMITED";
            var regxRADefendant1 = @"(Defendant[\ ]?\(s\):)";//被告 Defendant(s)
            var regxRADefendantReg1 = new Regex(regxRADefendant1, RegexOptions.IgnoreCase);

            var tmp2 = "答辯人Respondent(s):@D1 ALL OCCUPIER(S) OF PORTION OF SECTION A OF LOT NO 432 IN DEMARCATION DISTRICT NO 84";
            var tmp20 = "答辯人 Respondent(s):@D1 ALL OCCUPIER(S) OF PORTION OF SECTION A OF LOT NO 432 IN DEMARCATION DISTRICT NO 84";
            var regxRADefendant2 = @"(Respondent[\ ]?\(s\):)";//被告 答辯人Respondent(s)
            var regxRADefendantReg2 = new Regex(regxRADefendant2, RegexOptions.IgnoreCase);

            var tmp3 = "收款方RECEIVING PARTY(s):@R1:CITIFAME COMPANY LIMITED@付款方PAYING PARTY(s):@A1:CHOW SUI LAN (鄒瑞蘭)";
            var tmp4 = "收款方 RECEIVING PARTY(s):@R1:CITIFAME COMPANY LIMITED@付款方 PAYING PARTY(s):@A1:CHOW SUI LAN (鄒瑞蘭)";
            var regxRADefendant3 = @"(付款方[\ ]?PAYING\ PARTY\(s\):)";//被告 付款方PAYING PARTY(s)
            var regxRADefendantReg3 = new Regex(regxRADefendant3, RegexOptions.IgnoreCase);

            var dd = regxRADefendantReg1.IsMatch(tmp1);
            var dd1 = regxRADefendantReg1.IsMatch(tmp10);

            var dd2 = regxRADefendantReg2.IsMatch(tmp2);
            var dd22 = regxRADefendantReg2.IsMatch(tmp20);

            var dd3 = regxRADefendantReg3.IsMatch(tmp3);
            var dd33 = regxRADefendantReg3.IsMatch(tmp4);
        }
        static void testDB(int skip, DateTime dt)
        {
            var dd = new List<string>{
                //终审及高等法院
                     "cacfi","ct","lands","dc","dcmc",
                //所有裁判法院
                      "wtnmag",
               //勞資審裁處
                   "lt",
               //小额钱债审裁处
                   "smt"
            };

            foreach (var item in dd)
            {
                var tmpitem = item + dt.ToString("ddMMyyyy");//12052016


                var tmpMcase = getDb(tmpitem, skip);
                consolelog(tmpMcase, tmpitem);
            }
        }
        static void testCaseNoConsole()
        {
            var rev = @"建築及仲裁訴訟@HCCT 29/2013@[35/30+6]##";
            var rev3 = @"民事訴訟@HCA 1674/2014, HCA 2212/2014";
            var rev2 = @"ESS30291/2015";
            testReqCaseNo(rev3);
            testReqCaseNo(rev);
            testReqCaseNo(rev2);
        }
        static void testRemoveSplic()
        {
            var rev = new string[] {
             " 被告人Defendant(s):@THE HONG KONG HOUSING AUTHORITY@第三方Third Party(s):@CREATIVE PROPERTY SERVICES CONSULTANTS LIMITED ",
             " 被告人Defendant(s):@CHAN CHUN CHEE (陳鎮芝) ",
             " 被告人Defendant(s):@WING FAI TRANSPORTATION COMPANY ",
             " 收款方RECEIVING PARTY(s):@A1:THE INCORPORATED OWNERS OF YUEN LONG ON FAI BUILDING 元朗安輝樓業主立法團@付款方PAYING PARTY(s):@R1:LI KOON TAI ",
             " 原告人Plaintiff(s):@DAH SING INSURANCE COMPANY (1976) LIMITED ",
             " 原告人Plaintiff(s):@POWER PRINTING PRODUCTS LIMITED ",
             " Government of HKSAR (香港特別行政區)  ",
             //
             " 申請人Applicant(s):@郭家麟 ",
             " D1: HONGKONG WORLD LOGISTICS LIMITED   D2: 新會區會城泰森家具店 "
             };

            foreach (var item in rev)
            {
                System.Console.WriteLine("{0}------>{1}", item, regReplacToSpace(item));
            }
        }

        static void testPlaintiffsConsole2()
        {
            var tmpmcase = new m_Case();
            //tmpmcase.Plaintiff = "申請人 Applicant(s):@A1: 鄧樹坤@A2: 李綺年";
            //tmpmcase.Defendant = "答辯人 Respondent(s):@R1: 袁張華@R2: 高玉貞@R3: 林劍龍@R4: 溫民城@R5: 黃兆照@R6: 高日忠@R7: 張偉強@R8: 孟耀永@R9: 帥娟娟@R10: 容小玲@R11: 金滿閣、金堂閣、金旺閣業主立案法團";
            // tmpmcase.Defendant = "收款方RECEIVING PARTY(s):@P1:CHOW DANNY HOK YIN FRANCIS@P2:HIRAIDE CHIYURI@付款方PAYING PARTY(s):@D1:CANTON CENTURY LIMITED (粵宇有限公司)";
            //tmpmcase.Defendant = "D1: NG, BOON-PENG D2: LEE, KIAN-SEONG D3: CHOONG, CHEE-YONG D4:鄧梅平 DENG, MEIPING";
            //tmpmcase.Defendant = "Chu, Ka-yin (D1) (朱家言)";//, Kwong, Chun-lung (D2) (鄺振駹), Ng, Lai-ying (D3) (吳麗英), Poon, Tsz-hang (D4) (潘子行)
            //tmpmcase.Defendant = "答辯人Respondent(s):@HONG KONG DAZHAN TRADING (CHINA) LIMITED (香港大展貿易(中國)有限公司)@有意的一方INTENDED PARTY(s):@EMPLOYEES COMPENSATION ASSISTANCE FUND BOARD (僱員補償援助基金管理局)";
            //tmpmcase.Defendant = "陳建平 CHAN, KIN-PING";
            //tmpmcase.Defendant = "香港金德國際有限公司 HONG KONG GOLDEN RECIPROCATION INTERNATIONAL LIMITED";
            //tmpmcase.Plaintiff = "申請人Applicant(s):@CHONG CHI KEUNG";

            tmpmcase.Plaintiff = "A1: 温超平 wen chao ping A2: 陳耀鴻";
            tmpmcase.Defendant = "伍爾特(香港)有限公司(WURTH HONG KONG COMPANY LIMITED)";// "R1: 銅鑼灣灣景樓C座業主立案法團";

            tmpmcase.tname = "other";
            tmpmcase.Judge = "陳玲玲土地審裁處@暫委法官@Deputy Judge Tracy CHAN";
            tmpmcase.CaseNo = @"雜項案件@HCMP 1568/2015, HCA 2355/2015";// @"建築及仲裁訴訟@HCCT 29/2013@[35/30+6]##";
            consolelog(tmpmcase, tmpmcase.Plaintiff + " and " + tmpmcase.Defendant);
        }
        static void testPlaintiffsConsole()
        {
            var rev = new string[] {
             "KRE,KRE,KREa@And@Htl,Htl,Htld",
             "Mok Kam Ping (莫錦平)@And@Yip Ka Kai (葉家啓), Lui Chi Ming  Raingo formerly trading as Raingo Management Company (呂志明經營永高管理公司)",
             "HKSAR (香港特別行政區) v.@Wu Ping Yang Jimmy (吳品洋)",
             "RE: Chan Chun Ho Erick (陳駿顥)",
             "Alam Shorab",
             "Sky Ace Enterprises Limited@And@Appeal Tribunal (Buildings)@And@Building Authority",
             "Cheung Ting Kau, Vincent@And@Koo Siu Ying, Ling Meng Chu, Pearl@And@Lam Kin Ngok, Peter, U Po Chu",
             //
             "原告人Plaintiff(s):@CHAN YIK MING, a   minor, by YU WING SZE, his mother and next friend@AND@被告人Defendant(s):@MTR CORPORATION   LIMITED",
             "申索人Claimant(s):@RUBELYN   CASTILLANO DUCSA@AND@答辯人Respondent(s):@R1:SHEK   KWOK-NGAI@R2:KWAN YUK YING",
             "申請人Applicant(s):@LIN JIANPING (林間平)@AND@答辯人Respondent(s):@JUBILEE   PARAMOUNT JOY CUISINE LIMITED (銀禧百樂門囍宴有限公司)",
             "原告人Plaintiff(s):@LI XIAOYAN (李晓艳)@AND@被告人Defendant(s):@API PREMIERE LIMITED@第三債務人GARNISHEE(s):@HANG SENG BANK LIMITED",
            //
            "收款方RECEIVING PARTY(s):@P1:曾氏工程公司@付款方PAYING PARTY(s):@D1:林慧嫻",
            //
             "C1: 劉用良(LAU YUNG LEUNG)及另8位@v.@D1: 泉記環保資源再生(香港)有限公司(CHUEN KEE ENVIRONMENTAL RESOURCES RECYCLE (HONG KONG)  LIMITED)@D2: 泉記資源有限公司(CHUEN KEE RESOURCES  LIMITED)",
             
            //
             "陳依蓮(CHAN YEE  LIN CAROL)@v.@美峰國際有限公司經營瑰麗堂(HILL  INTERNATIONAL LIMITED T/A DELUXE MEDICAL CENTRE)",
            //
             "B.M.W. Concessionaires (H.K.) Limited",
            //
             "Secretary for Justice, c/o Commissioner of Rating and  Valuation 律政司司長@(差餉物業估價署署長轉交@)",

             };
            var revAllmsg = new string[]
            {
                "WIJEMANNA, RANJITH PRABHA KEERTHI",
                "譚鏡 TAM, KAN",
                "D2:楊偉新 YANG, WEIXIN   D3:李萍 LI, PING  D4: 顏玲進 YAN, LINGJIN",
                "孖8汽車零件有限公司 DOUBLE-EIGHT AUTO PARTS CO., LIMITED",
                "D1: AHMED, SANI SALMAN D2: MOREIRA, ABHAY CHARAN DAS BATISTA",
                "駿輝建築有限公司 CHUN FAI CONSTRUCTION COMPANY LIMITED",
                "卡琳蒂芬（香港）國際集團有限公司 KLDF (HK) INTERNATIONAL GROUP CO., LIMITED",
                "D1:張本秀 ZHANG, BANXIU   D2:羅良存 LUO, LIANGCUN  D3: 黃耀玲 HUANG, YAOLING  D4: 王新愛 WANG, XINAI  D5: 何盛清 HE, SHENGQING"
            };

            foreach (var item in rev)
            {
                var tmpmcase = new m_Case();
                tmpmcase.Plaintiff = item;
                tmpmcase.tname = "other";
                tmpmcase.CaseNo = @"建築及仲裁訴訟@HCCT 29/2013@[35/30+6]##";
                consolelog(tmpmcase, item);
            }
            foreach (var item in revAllmsg)
            {
                var tmpmcase = new m_Case();
                tmpmcase.Plaintiff = item;
                tmpmcase.tname = "wtnmag";
                tmpmcase.CaseNo = @"建築及仲裁訴訟@HCCT 29/2013@[35/30+6]##";
                consolelog(tmpmcase, item);
            }
        }
        static string ReplaceReg(string patt, string input, string totxt)
        {
            try
            {
                Regex rgx = new Regex(patt, RegexOptions.IgnoreCase);
                return rgx.Replace(input.ToString(), totxt).Trim();
            }
            catch (Exception ex)
            {

                return input;
            }
        }
        public static string regReplacToSpace(string strV)
        {
            var v = strV;
            try
            {
                //var regx2 = @"@(";
                //var regxRA = @"RE:|[ACDRP][0-9]:";
                var reg2 = @"[\u3000\u0020]{2,}";//去空格,两个以上空格改为一个，全角、半角
                var regxR = @"[\w\ \(\)]+:@";


                var regx0 = @"v.@";
                var regx1 = @"@and@";
                var regx2 = @"@";

                v = ReplaceReg(regxR, v, " ");

                v = ReplaceReg(regx0, v, " ");
                v = ReplaceReg(regx1, v, " ");
                v = ReplaceReg(regx2, v, " ");

                v = ReplaceReg(reg2, v, " ");

                return v;
            }
            catch (Exception)
            {
                return v;
            }

        }
        /// <summary>
        /// 显示获取的对象。
        /// </summary>
        /// <param name="tmpmode"></param>
        /// <param name="msg"></param>
        static void consolelog(m_Case tmpmode, string msg)
        {
            //demo 获取解析后的对象。
            var getData = WebGetDll.Api.getData(tmpmode);

            if (getData.ReturnStatus == 0)
            {

                Console.WriteLine("\n");
                Console.WriteLine("##############New#################:##############New#################:" + getData.Cases.t_Case.tname);
                Console.WriteLine("***************CaseNo:" + getData.Cases.t_Case.CaseNo);
                Console.WriteLine("\tno:" + getData.Cases.t_Case.CaseNo);
                Console.WriteLine("\tNew:" + getData.Cases.t_Case.CaseNoNew);
                Console.WriteLine("\tzh:" + getData.Cases.t_Case.CaseNo_Cn);
                Console.WriteLine("\tPrefix:" + getData.Cases.t_Case.CaseTypeID);
                Console.WriteLine("\tYear:" + getData.Cases.t_Case.Year);
                Console.WriteLine("\tSerialNo:" + getData.Cases.t_Case.SerialNo);
                Console.WriteLine("\tnuTimes:" + getData.Cases.t_Case.NumberTimes);

                Console.WriteLine("\n");
                Console.WriteLine("***************P:" + tmpmode.Plaintiff + ",D:" + tmpmode.Defendant + ",R:" + tmpmode.Representation);


                foreach (var item in getData.Cases.Plaintiffs)
                {
                    Console.WriteLine("\tPlaintiffs: EN:" + item.FullName_En);
                    Console.WriteLine("\tPlaintiffs: ZH:" + item.FullName_Cn);
                    Console.WriteLine("\tPlaintiffs: Type:" + item.Type);
                }
                Console.WriteLine();
                foreach (var item in getData.Cases.Plaintiffs_Representations)
                {
                    Console.WriteLine("\tPlaintiffs_Representations: EN:" + item.FullName_En);
                    Console.WriteLine("\tPlaintiffs_Representations: ZH:" + item.FullName_Cn);
                    Console.WriteLine("\tPlaintiffs_Representations: Type:" + item.Type);
                }
                Console.WriteLine();
                foreach (var item in getData.Cases.Defendants)
                {
                    Console.WriteLine("\tDefendants: EN:" + item.FullName_En);
                    Console.WriteLine("\tDefendants: ZH:" + item.FullName_Cn);
                    Console.WriteLine("\tDefendants: Type:" + item.Type);
                }
                Console.WriteLine();
                foreach (var item in getData.Cases.Defendants_Representations)
                {
                    Console.WriteLine("\tDefendants_Representations: EN:" + item.FullName_En);
                    Console.WriteLine("\tDefendants_Representations: ZH:" + item.FullName_Cn);
                    Console.WriteLine("\tDefendants_Representations: Type:" + item.Type);
                }
                Console.WriteLine("###########法官:###########*******************");
                foreach (var item in getData.Cases.Judges)
                {
                    Console.WriteLine("\nJudges: EN:" + item.FullName_En);
                    Console.WriteLine("\nJudges: ZH:" + item.FullName_Cn);
                    Console.WriteLine("\nJudges: Type:" + item.Type);
                }
            }
            else
            {
                Console.WriteLine("\n****************************************************");
                Console.WriteLine(msg + " ##没有记录。Flag:" + getData.ReturnStatus + ",Message:" + getData.message);
                Console.WriteLine("\n****************************************************");

            }
        }
        static m_Case getDb(string tkeyno, int skip)
        {
            try
            {
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

                var models = db.m_Case_items.Where(a => a.tkeyNo.Equals(tkeyno)).OrderBy(a => a.tIndex).Skip(skip).Take(1).FirstOrDefault();

                if (models == null)
                {
                    return null;
                }

                var tmpMcase = new m_Case();

                tmpMcase.tname = models.tname;
                tmpMcase.Amount = models.Amount;

                tmpMcase.CaseNo = models.CaseNo;
                //tmpMcase.Cause = models.Cause;
                //tmpMcase.CheckField = models.CheckField;
                tmpMcase.CourtDay = models.CourtDay;
                tmpMcase.Currency = models.Currency;
                tmpMcase.Defendant = models.Defendant;
                tmpMcase.Hearing = models.Hearing;
                tmpMcase.Judge = models.Judge;
                tmpMcase.Nature = models.Nature;
                tmpMcase.OpenCourtTime = models.CourtDay;
                tmpMcase.Plaintiff = models.PlainTiff;
                tmpMcase.Representation = models.Representation;

                return tmpMcase;
                //foreach (var item in models)
                //{
                //    Console.WriteLine(item.CaseNo + ",\t\t" + item.Judge);
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        ///雜項案件@HCMP 270/2016
        ///HCMP 270/2016
        ///HCMP 
        ///270
        ///2016
        ///测试 Case No
        /// </summary>
        /// <param name="v"></param>
        /// <param name="isnext"></param>
        /// <param name="index">1:prefix,2:no,3:year|1:zh|1:times</param>
        static void testReqCaseNo(string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                return;
            }
            var tmpmcase = new m_Case();
            tmpmcase.CaseNo = v;
            var tCase = WebGetDll.Api.decCaseNo(tmpmcase);

            Console.WriteLine("******************" + v);
            Console.WriteLine("\tno:" + tCase.CaseNo);
            Console.WriteLine("\tNew:" + tCase.CaseNoNew);
            Console.WriteLine("\tzh:" + tCase.CaseNo_Cn);
            Console.WriteLine("\tPrefix:" + tCase.CaseTypeID);
            Console.WriteLine("\tYear:" + tCase.Year);
            Console.WriteLine("\tSerialNo:" + tCase.SerialNo);
            Console.WriteLine("\tnuTimes:" + tCase.NumberTimes);

        }
        static void tRegex(string regFlag, string v, int num)
        {
            Regex r1 = new Regex(regFlag);
            Match m2 = r1.Match(@v);

            //Console.WriteLine(@v);
            if (m2.Success)
            {
                Console.WriteLine(m2.Groups[num].Value);

                //foreach (Group g in m2.Groups)
                //{
                //    Console.WriteLine(g.Value);
                //}
            }
        }
    }
}