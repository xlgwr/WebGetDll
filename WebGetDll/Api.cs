using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using WebGetDll.model;

namespace WebGetDll
{
    public sealed class Api
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmpMCase"></param>
        /// <returns></returns>
        public static responCaseData getData(m_Case tmpMCase)
        {
            var tmpresponData = new responCaseData();
            try
            {

                //解析caseNo
                var tmpDeccase = decCaseNo(tmpMCase);
                if (tmpDeccase == null)
                {
                    tmpresponData.ReturnStatus = -2;
                    tmpresponData.message = "Error: 参数为不符合要求。";
                    return tmpresponData;
                }
                //初始化，caseNo
                tmpresponData.Cases.t_Case = tmpDeccase;
                //解析原告,被告
                decPlainTiffAndDefendant(tmpMCase, ref tmpresponData);
                //解析原告,被告 应讯代表
                decPlainTiffAndDefendant_Representations(tmpMCase, ref tmpresponData);
                //解析法官
                decJudge(tmpMCase, ref tmpresponData);


                //////////////
                tmpresponData.ReturnStatus = 0;
                tmpresponData.message = "解析成功";
                return tmpresponData;
            }
            catch (Exception ex)
            {
                tmpresponData.ReturnStatus = -1;
                tmpresponData.message = "Error:" + ex.Message;
                tmpresponData.Cases = null;
                return tmpresponData;
            }
            return tmpresponData;

        }

        private static void decJudge(m_Case tmpMCase, ref responCaseData res)
        {
            try
            {
                if (tmpMCase == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(tmpMCase.tname) || string.IsNullOrEmpty(tmpMCase.Judge))
                {
                    return;
                }
                //
                //终审及高等法院
                // "cacfi","ct","lands","dc","dcmc",
                // //所有裁判法院
                //       "wtnmag",
                ////勞資審裁處
                //    "lt",
                ////小额钱债审裁处
                //    "smt"


                List<person> tmpPersonJudge = new List<person>();


                var allText = tmpMCase.Judge.Replace("Coram:", "@").Split('@');
                var allCount = allText.Count();
                if (allCount == 1)
                {

                    var tmpOne = new person();
                    tmpOne.FullName_En = allText[0];
                    tmpOne.Type = 2;
                    tmpPersonJudge.Add(tmpOne);
                    return;
                }
                if (allCount < 2) { return; }
                switch (tmpMCase.tname)
                {
                    case "cacfi":
                    case "lands":
                    case "ct":
                    case "lt":
                    case "wtnmag":
                        if (allCount % 2 == 0)
                        {
                            for (int i = 0; i < allCount; i = i + 2)
                            {
                                var tmpOneJ = new person();
                                tmpOneJ.FullName_Cn = allText[i].Trim();
                                tmpOneJ.FullName_En = allText[i + 1].Trim();
                                tmpOneJ.Type = 2;
                                tmpPersonJudge.Add(tmpOneJ);
                            }
                        }
                        break;
                    case "dc":
                    case "dcmc":
                        var tmpOneDc = new person();
                        tmpOneDc.FullName_Cn = allText[0].Trim();
                        tmpOneDc.FullName_En = allText[3].Trim();
                        tmpOneDc.Type = 2;
                        tmpPersonJudge.Add(tmpOneDc);
                        break;
                    case "smt":
                        var tmpOneSmt = new person();
                        tmpOneSmt.FullName_Cn = allText[1].Trim();
                        tmpOneSmt.FullName_En = allText[0].Trim();
                        tmpOneSmt.Type = 2;
                        tmpPersonJudge.Add(tmpOneSmt);
                        break;
                    default:
                        break;
                }
                res.Cases.Judges = tmpPersonJudge;

            }
            catch (Exception)
            {
                return;
            }
        }

        private static void decPlainTiffAndDefendant_Representations(m_Case tmpMCase, ref responCaseData res)
        {
            try
            {
                if (tmpMCase == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(tmpMCase.tname) || string.IsNullOrEmpty(tmpMCase.Representation))
                {
                    return;
                }
                //
                //终审及高等法院
                // "cacfi","ct","lands","dc","dcmc",
                // //所有裁判法院
                //       "wtnmag",
                ////勞資審裁處
                //    "lt",
                ////小额钱债审裁处
                //    "smt"

                var allText = regDoubleSpace2(tmpMCase.Representation).Split('@');


                List<person> tmpPersonPlaintiffs_Re = new List<person>();
                List<person> tmpPersonDefendants_Re = new List<person>();

                var allCount = allText.Count();
                if (allCount == 2)
                {
                    var tmpOne = new person();
                    tmpOne.FullName_Cn = allText[0].Trim();
                    tmpOne.FullName_En = allText[1].Trim();
                    tmpOne.Type = reType(tmpOne);
                    tmpPersonPlaintiffs_Re.Add(tmpOne);
                }
                else if (allCount % 2 == 0)
                {
                    var tmpOne = new person();
                    tmpOne.FullName_Cn = allText[0].Trim();
                    tmpOne.FullName_En = allText[1].Trim();
                    tmpOne.Type = reType(tmpOne);
                    tmpPersonPlaintiffs_Re.Add(tmpOne);
                    for (int i = 2; i < allCount; i = i + 2)
                    {
                        var tmpTwe = new person();
                        tmpTwe.FullName_Cn = allText[i].Trim();
                        tmpTwe.FullName_En = allText[i + 1].Trim();
                        tmpTwe.Type = reType(tmpTwe);
                        tmpPersonDefendants_Re.Add(tmpTwe);
                    }
                }
                else if (allCount % 2 == 1)
                {

                    var tmpOne = new person();
                    tmpOne.FullName_En = allText[0].Trim();
                    tmpOne.Type = reType(tmpOne);
                    tmpPersonPlaintiffs_Re.Add(tmpOne);

                    for (int i = 1; i < allCount; i = i + 2)
                    {
                        var tmpTwe = new person();
                        tmpTwe.FullName_Cn = allText[i].Trim();
                        tmpTwe.FullName_En = allText[i + 1].Trim();
                        tmpTwe.Type = reType(tmpTwe);
                        tmpPersonDefendants_Re.Add(tmpTwe);
                    }
                }

                res.Cases.Plaintiffs_Representations = tmpPersonPlaintiffs_Re;
                res.Cases.Defendants_Representations = tmpPersonDefendants_Re;
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 原告与被告
        /// </summary>
        /// <param name="tmpMCase"></param>
        /// <returns></returns>
        private static void decPlainTiffAndDefendant(m_Case tmpMCase, ref responCaseData res)
        {
            try
            {
                if (tmpMCase == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(tmpMCase.tname) || string.IsNullOrEmpty(tmpMCase.Plaintiff + tmpMCase.Defendant))
                {
                    return;
                }
                if (string.IsNullOrEmpty(tmpMCase.Plaintiff) && !string.IsNullOrEmpty(tmpMCase.Defendant))
                {

                    var regxRADefendant = @"([D][0-9]+:)";//被告
                    var getStr = WebGetDll.Api.tRegex(regxRADefendant, tmpMCase.Defendant, 1, false, false, false);

                    if (!string.IsNullOrEmpty(getStr))
                    {
                        var startIndex = tmpMCase.Defendant.IndexOf(getStr, StringComparison.OrdinalIgnoreCase);
                        if (startIndex > getStr.Length)
                        {
                            tmpMCase.Plaintiff = tmpMCase.Defendant.Substring(0, startIndex);
                            tmpMCase.Defendant = tmpMCase.Defendant.Substring(startIndex + getStr.Length);
                        }

                    }
                }

                //
                //终审及高等法院
                // "cacfi","ct","lands","dc","dcmc",
                // //所有裁判法院
                //       "wtnmag",
                ////勞資審裁處
                //    "lt",
                ////小额钱债审裁处
                //    "smt"

                var regxRA1Name = @"[\u4e00-\u9fa5]+ [A-Za-z]+\,[ ]{0,1}[A-Za-z]+";//王健明 WONG, KIN-MING

                var regxRA1 = @"[ACDRP][0-9]+:";
                var regxRA1Reg = new Regex(regxRA1, RegexOptions.IgnoreCase);

                var retFlagEN = @"(?<ENname>[0-9A-Za-z\ \&\.\'\-\/\(\)]+)";
                var retFlagZH = @"(?<zh>[0-9\u4e00-\u9fa5\（\）\(\)\、\“]+)";
                var retFlagKH = @"[\（\）\(\)]";
                var retFlagKHReg = new Regex(retFlagKH, RegexOptions.IgnoreCase);
                var retFlagZHno = @"(?<zh>[\u4e00-\u9fa5]+)";

                bool starD1 = false;

                if (!string.IsNullOrEmpty(tmpMCase.Defendant))
                {
                    var regxRADefendant2 = @"(\([D][0-9]+\))[\ ]{0,1}(\([\u4e00-\u9fa5]+\),?)"; //被告  Lee, Wai-keung (D1) (利偉强),
                    var regxRADefendant3Reg = new Regex(@"[\(\),]", RegexOptions.IgnoreCase); //被告  Lee, Wai-keung (D1) (利偉强)

                    try
                    {

                        var getTmpValue = WebGetDll.Api.tRegex(regxRADefendant2, tmpMCase.Defendant, true);
                        var tmpDefendant = regxRADefendant3Reg.Replace(tmpMCase.Defendant, "");
                        foreach (var item in getTmpValue)
                        {
                            var item0 = regxRADefendant3Reg.Replace(item[0], "");
                            var item1 = regxRADefendant3Reg.Replace(item[1], "");
                            var item2 = regxRADefendant3Reg.Replace(item[2], "");
                            var item3 = @item2 + " " + item1 + ":";

                            tmpDefendant = ReplaceReg(item0, tmpDefendant, item3);
                        }
                        tmpMCase.Defendant = tmpDefendant;
                    }
                    catch (Exception)
                    {
                    }
                }
                var v = regDoubleSpace(tmpMCase.Plaintiff) + " V.@ " + regDoubleSpace(tmpMCase.Defendant);

                if (regxRA1Reg.IsMatch(v))
                {
                    starD1 = true;
                    retFlagEN = @"(?<ENname>[0-9A-Za-z\ \&\,\'\-\.\/\(\)]+)";
                }
                var regxRA1NameReg = new Regex(regxRA1Name, RegexOptions.IgnoreCase);//

                if (tmpMCase.tname.Equals("wtnmag"))
                {
                    var dd = regxRA1NameReg.IsMatch(v);
                    if (dd)
                    {
                        starD1 = true;
                        retFlagEN = @"(?<ENname>[0-9A-Za-z\ \&\,\'\-\.\/\(\)]+)";
                    }
                }
                Dictionary<string, string> listForReq = genDicForV(v, starD1);
                if (listForReq == null)
                {
                    return;
                }
                List<person> tmpPersonPlaintiffs = new List<person>();
                List<person> tmpPersonDefendants = new List<person>();
                foreach (var tmpItem in listForReq)
                {
                    var tmpen = "";
                    var tmpzh = "";
                    var tmpFlag = "";

                    var checkZh = WebGetDll.Api.tRegex(retFlagZHno, tmpItem.Value, 1, false, false, false);
                    var getEnValue = tmpItem.Value;

                    if (!string.IsNullOrEmpty(checkZh))
                    {
                        tmpzh = WebGetDll.Api.tRegex(retFlagZH, tmpItem.Value, 1, false, false, false);
                        if (tmpzh.Length < 2)
                        {
                            var toZH = retFlagKHReg.Replace(tmpItem.Value, "");
                            getEnValue = retFlagKHReg.Replace(tmpItem.Value, "");
                            tmpzh = WebGetDll.Api.tRegex(retFlagZH, toZH, 1, false, false, false);
                        }
                        getEnValue = getEnValue.Replace(tmpzh, "");
                        tmpzh = removeCharFL(tmpzh, "(", ")");


                    }
                    tmpen = WebGetDll.Api.tRegex(retFlagEN, getEnValue, 1, false, false, false);
                    tmpen = removeCharFL(tmpen, "(", ")");

                    if (tmpen.Length < 2)
                    {
                        tmpen = "";
                        tmpzh = removeCharFL(tmpItem.Value, "(", ")");
                    }

                    var tmpType = reType(tmpen, tmpzh);
                    if (tmpItem.Key.StartsWith("Plaintiffs"))
                    {
                        tmpFlag = "Plaintiffs";
                        tmpPersonPlaintiffs.Add(new person() { FullName_Cn = tmpzh, FullName_En = tmpen, Type = tmpType });
                    }
                    else
                    {
                        tmpFlag = "Defendants";
                        tmpPersonDefendants.Add(new person() { FullName_Cn = tmpzh, FullName_En = tmpen, Type = tmpType });
                    }
                    //Console.WriteLine(tmpFlag + " EN:" + tmpen);
                    //Console.WriteLine(tmpFlag + " ZH:" + tmpzh);
                }
                //Console.WriteLine("Plaintiffs：" + tmpPersonPlaintiffs.Count);
                //Console.WriteLine("Defendants：" + tmpPersonDefendants.Count);
                res.Cases.Plaintiffs = tmpPersonPlaintiffs;
                res.Cases.Defendants = tmpPersonDefendants;
            }
            catch (Exception)
            {
                return;
            }
        }
        static int reType(string tmpen, string tmpzh)
        {
            return (tmpen.IndexOf("LIMITED", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                                     tmpen.IndexOf("Ltd.", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                                     tmpen.IndexOf("Co.", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                                     tmpen.IndexOf("CO.,", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                                     tmpzh.IndexOf("行政區", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                                     tmpzh.IndexOf("公司", StringComparison.InvariantCultureIgnoreCase) > -1
                                    ) ? 1 : 0;
        }
        static int reType(person p)
        {
            return reType(p.FullName_En, p.FullName_Cn);
        }
        /// <summary>
        /// 去除首未括号
        /// </summary>
        /// <param name="v"></param>
        /// <param name="F"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        private static string removeCharFL(string v, string F, string L)
        {
            try
            {
                v = v.IndexOf(F) == 0 ? v.Substring(1) : v;
                v = v.IndexOf(L) == 0 ? v.Substring(1) : v;
                v = v.LastIndexOf(F) == (v.Length - 1) ? v.Substring(0, v.Length - 1) : v;
                v = v.LastIndexOf(L) == (v.Length - 1) ? (v.IndexOf(F) < 0 ? v.Substring(0, v.Length - 1) : v) : v;

                return v;
            }
            catch (Exception ex)
            {
                return v;
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
        private static Dictionary<string, string> genDicForV(string input, bool starD1)
        {
            var v = input;
            var splitChar = @",";
            if (starD1)
            {
                splitChar = @"|";
            }
            var regx0 = @"V.@";
            var regx1 = @"@And@";

            var regxRA0 = @"RE:";
            var regxRA1 = @"[ACDRP][0-9]+:";
            var regxRA2 = @"@,";
            var regxR2 = @"[\u4e00-\u9fa5]+[A-Da-z\ ]+\(s\):";

            v = ReplaceReg(regxRA0, v, " ");
            v = ReplaceReg(regxRA1, v, splitChar);
            v = ReplaceReg(regxRA2, v, splitChar);

            v = ReplaceReg(regxR2, v, splitChar);

            v = ReplaceReg(regx0, v, "@");
            v = ReplaceReg(regx1, v, "@");

            if (string.IsNullOrEmpty(v))
            {
                return null;
            }

            var listForReq = new Dictionary<string, string>();


            //Console.WriteLine("*********************:" + v);
            var tmpkeyfal = "Plaintiffs";
            if (v.IndexOf("@") > -1)
            {
                var getData = v.Split('@');
                var toIndex = 0;
                for (int i = 0; i < getData.Count(); i++)
                {
                    if (i > 0)
                    {
                        tmpkeyfal = "Defendants";
                    }

                    var currItem = getData[i].Trim();
                    if (string.IsNullOrEmpty(currItem)) continue;
                    if (!string.IsNullOrEmpty(currItem))
                    {
                        if (currItem.ToLower().Equals("and")) continue;
                        if (currItem.ToLower().Equals("(")) continue;
                        if (currItem.ToLower().Equals(")")) continue;
                        if (currItem.ToLower().Equals("*")) continue;

                        if (currItem.IndexOf(splitChar) > -1)
                        {
                            var getData2 = currItem.Split(splitChar.ToCharArray());
                            var tmpcheck = new Dictionary<string, bool>();
                            for (int x = 0; x < getData2.Count(); x++)
                            {
                                var getItem = getData2[x].Trim();
                                if (string.IsNullOrEmpty(getItem))
                                {
                                    continue;
                                }
                                if (tmpcheck.ContainsKey(getItem))
                                {
                                    continue;
                                }
                                tmpcheck.Add(getItem, true);
                                listForReq.Add(tmpkeyfal + toIndex, getItem);
                                toIndex++;
                            }
                        }
                        else
                        {
                            listForReq.Add(tmpkeyfal + toIndex, currItem.Trim());
                        }

                    }
                    toIndex++;
                }

            }
            else
            {
                listForReq.Add(tmpkeyfal, v.Trim());
            }

            return listForReq;
        }
        /// <summary>
        /// 去除换行符等/去空格,两个以上空格改为一个，全角、半角
        /// </summary>
        /// <param name="strV"></param>
        /// <returns></returns>
        public static string regDoubleSpace2(string strV)
        {
            var v = strV;
            try
            {

                var reg1 = @"[\f\n\r\t\v]";//去除换行符等
                var reg2 = @"[\u3000\u0020]{2,}";//去空格,两个以上空格改为一个，全角、半角

                v = ReplaceReg(reg1, v, " ");

                v = ReplaceReg(reg2, v, " ");


                return v;
            }
            catch (Exception)
            {
                return v;
            }

        }
        /// <summary>
        /// 去除双空格，及@
        /// </summary>
        /// <param name="strV"></param>
        /// <returns></returns>
        public static string regDoubleSpace(string strV)
        {
            var v = strV;
            try
            {
                //var regx2 = @"@(";
                //var regxRA = @"RE:|[ACDRP][0-9]:";

                var reg1 = @"[\f\n\r\t\v]";//去除换行符等
                var reg2 = @"[\u3000\u0020]{2,}";//去空格,两个以上空格改为一个，全角、半角
                var reg3 = @"CO.,";//公司缩写

                var regx2 = @"@";

                v = ReplaceReg(reg1, v, " ");

                v = ReplaceReg(reg3, v, " Company ");

                v = ReplaceReg(regx2, v, " ");

                v = ReplaceReg(reg2, v, " ");

                
                return v;
            }
            catch (Exception)
            {
                return v;
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
                var regxR = @"[\u4e00-\u9fa5]+[A-Da-z\ ]+\(s\):@";


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
        public static t_Case decCaseNo(m_Case tmpMCase)
        {
            var tmpTcase = new t_Case();
            try
            {
                if (tmpMCase == null)
                {
                    return null;
                }
                if (string.IsNullOrEmpty(tmpMCase.CaseNo))
                {
                    return null;
                }
                //
                tmpTcase.tname = tmpMCase.tname;
                tmpTcase.Actiondate = tmpMCase.Actiondate;
                tmpTcase.Amount = tmpMCase.Amount;

                //tmpTcase.Cause = tmpMCase.Cause;
                tmpTcase.CheckField = tmpMCase.CheckField;
                tmpTcase.CourtDay = regReplacToSpace(tmpMCase.CourtDay);
                tmpTcase.Currency = tmpMCase.Currency;
                tmpTcase.Defendant = regReplacToSpace(tmpMCase.Defendant);
                tmpTcase.Hearing = regReplacToSpace(tmpMCase.Hearing);
                tmpTcase.Judge = regReplacToSpace(tmpMCase.Judge);
                tmpTcase.Nature = regReplacToSpace(tmpMCase.Nature);
                tmpTcase.OpenCourtTime = tmpMCase.OpenCourtTime;
                tmpTcase.Plaintiff = regReplacToSpace(tmpMCase.Plaintiff);
                tmpTcase.Representation = regDoubleSpace(tmpMCase.Representation);

                //add new 2016-08-25
                tmpTcase.CourtID = tmpMCase.CourtID;
                tmpTcase.DataGradeID = tmpMCase.DataGradeID;
                tmpTcase.D_Address = tmpMCase.D_Address;
                tmpTcase.HtmlID = tmpMCase.HtmlID;
                tmpTcase.Judgement = tmpMCase.Judgement;
                tmpTcase.Other = regReplacToSpace(tmpMCase.Other);
                tmpTcase.Other1 = regReplacToSpace(tmpMCase.Other1);
                tmpTcase.P_Address = tmpMCase.P_Address;
                tmpTcase.Parties = regDoubleSpace(tmpMCase.Parties);
                tmpTcase.Representation_P = regReplacToSpace(tmpMCase.Representation_P);
                tmpTcase.Representation_D = regReplacToSpace(tmpMCase.Representation_D);

                tmpTcase.Caseid = "";

                //雜項案件@HCMP 270/2016
                //HCMP 270/2016

                //HCMP 
                //270
                //2016

                ///1
                var regFlag = @"(?<prefix1>[A-Za-z ]+)(?<no2>\d+)/(?<year3>[\d]+)";
                var regFlagZH = @"(?<zh1>[\u4e00-\u9fa5]+)@" + regFlag;
                var regFlagNumberTimes = @"@(?<time1>\[[\w/\+]+\]#+)";


                ////////////////////////////////////////////

                tmpTcase.CaseNoNew = tRegex(regFlag, tmpMCase.CaseNo, 0, true, false, false);

                if (string.IsNullOrEmpty(tmpTcase.CaseNoNew))
                {
                    return null;
                }
                if (tmpTcase.CaseNoNew.IndexOf(',') > -1)
                {
                    try
                    {
                        var dd = tmpTcase.CaseNoNew.Split(',');
                        tmpMCase.CaseNo = dd[0];
                    }
                    catch (Exception ex)
                    {

                    }
                }

                tmpTcase.CaseTypeID = tRegex(regFlag, tmpMCase.CaseNo, 1, false, false, false);
                tmpTcase.SerialNo = tRegex(regFlag, tmpMCase.CaseNo, 2, false, false, false);//2, true, true, true);

                var tmpyear = tRegex(regFlag, tmpMCase.CaseNo, 3, false, false, false);//3, true, true, true);
                var tmpSearchStrYear1 = @"/" + tmpyear;

                if (tmpyear.Length == 2)
                {
                    tmpyear = "20" + tmpyear;
                }
                //if (tmpTcase.CaseNoNew.IndexOf(',') > -1)
                //{
                //    var tmpYearStrMax = "";
                //    var tmpArrCase = tmpTcase.CaseNoNew.Split(',');

                //    var tmpyearMin = tRegex(regFlag, tmpMCase.CaseNo, 3, true, true, false);

                //    if (tmpyearMin.Length == 2)
                //    {
                //        tmpyearMin = "20" + tmpyearMin;
                //    }

                //    if (!tmpyear.Equals(tmpyearMin))
                //    {
                //        foreach (var item in tmpArrCase)
                //        {
                //            if (item.Contains(tmpSearchStrYear1))
                //            {
                //                tmpYearStrMax += item + ",";
                //            }
                //        }

                //        tmpTcase.CaseTypeID = tRegex(regFlag, tmpYearStrMax, 1, false, false, false);
                //        tmpTcase.SerialNo = tRegex(regFlag, tmpYearStrMax, 2, true, true, true);
                //    }
                //}


                tmpTcase.Year = tmpyear;
                tmpTcase.CaseNo = tmpTcase.CaseTypeID.Trim() + tmpTcase.SerialNo + @"/" + tmpTcase.Year;
                tmpTcase.CaseNoNew = tmpTcase.CaseNo;

                ////////////////////////////////////////
                var tmpZH = tRegex(regFlagZH, tmpMCase.CaseNo, 1, false, false, false);
                if (!string.IsNullOrEmpty(tmpZH))
                {
                    tmpTcase.CaseNo_Cn = tmpZH + tmpTcase.SerialNo + @"/" + tmpTcase.Year;
                }
                ////////////////////////////////////
                tmpTcase.NumberTimes = tRegex(regFlagNumberTimes, tmpMCase.CaseNo, 1, false, false, false);

                return tmpTcase;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 正则表达式 for CASE No
        ///  @"建築及仲裁訴訟@HCCT 29/2013@[35/30+6]##";
        ///  @"(?prefix>[A-Za-z ]+)(?no\d+)/(?year[\d]+)"--》1---》HCCT
        ///  @"(?zh[\u4e00-\u9fa5]+)@"--》1---》建築及仲裁訴訟
        ///   @"@(?time1\[[\w/\+]+\]#+)";--》1---》[35/30+6]##      
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="isnext"></param>
        /// <param name="getMaxOrMin"></param>
        /// <param name="isMaxOrMin">True:最大的，false:最小的</param>
        /// <returns></returns>
        public static string tRegex(string pattern, string input, int index, bool isnext, bool getMaxOrMin, bool isMaxOrMin)
        {
            try
            {
                Regex r1 = new Regex(pattern);
                Match m2 = r1.Match(input);
                var tmpvalue = input;

                if (m2.Success)
                {
                    if (isnext)
                    {
                        tmpvalue = m2.Groups[index].Value.Trim();

                        var isgo = true;
                        while (isgo)
                        {
                            m2 = m2.NextMatch();
                            isgo = m2.Success;
                            if (isgo)
                            {
                                var tmpgetNext = m2.Groups[index].Value.Trim();
                                if (getMaxOrMin)
                                {
                                    double inttmpgetNext = 0;
                                    double inttmpvalue = 0;

                                    double.TryParse(tmpgetNext, out inttmpgetNext);
                                    double.TryParse(tmpvalue, out inttmpvalue);
                                    if (isMaxOrMin)
                                    {
                                        if (inttmpgetNext > inttmpvalue)
                                        {
                                            tmpvalue = tmpgetNext;
                                        }
                                    }
                                    else
                                    {
                                        if (inttmpgetNext < inttmpvalue)
                                        {
                                            tmpvalue = tmpgetNext;
                                        }
                                    }

                                }
                                else
                                {
                                    tmpvalue += "," + tmpgetNext;
                                }

                            }
                        }

                        return tmpvalue;
                    }
                    else
                    {

                        return m2.Groups[index].Value.Trim();
                    }

                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
                //throw;
            }

        }

        /// <summary>
        /// 正则表达式 for CASE No
        ///  @"建築及仲裁訴訟@HCCT 29/2013@[35/30+6]##";
        ///  @"(?<prefix>[A-Za-z ]+)(?<no>\d+)/(?<year>[\d]+)"--》1---》HCCT
        ///  @"(?<zh>[\u4e00-\u9fa5]+)@"--》1---》建築及仲裁訴訟
        ///  @"@(?<time1>\[[\w/\+]+\]#+)";--》1---》[35/30+6]##  
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <param name="input">输入内容</param>
        /// <param name="index">返第几个配对值</param>
        /// <param name="isnext">是否取下一下</param>
        /// <returns></returns>
        public static string[] tRegex(string pattern, string input, int index, bool isnext)
        {
            var getList = tRegex(pattern, input, isnext);

            if (getList.Count >= (index))
            {
                if (isnext)
                {
                    return getList[index];
                }
                else
                {
                    return getList[0];
                }

            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 正则表达式 for CASE No
        ///  @"建築及仲裁訴訟@HCCT 29/2013@[35/30+6]##";
        ///  @"(?<prefix>[A-Za-z ]+)(?<no>\d+)/(?<year>[\d]+)"--》1---》HCCT
        ///  @"(?<zh>[\u4e00-\u9fa5]+)@"--》1---》建築及仲裁訴訟
        ///  @"@(?<time1>\[[\w/\+]+\]#+)";--》1---》[35/30+6]##  
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <param name="input">输入内容</param>
        /// <param name="index">返第几个配对值</param>
        /// <returns></returns>
        public static List<string[]> tRegex(string pattern, string input, bool isnext)
        {
            var tmpList = new List<string[]>();
            try
            {
                Regex r1 = new Regex(pattern);
                Match m2 = r1.Match(input);

                if (m2.Success)
                {
                    var tmpStr = "";
                    foreach (Group item in m2.Groups)
                    {
                        if (string.IsNullOrEmpty(tmpStr))
                        {
                            tmpStr = item.Value.Trim();
                        }
                        else
                        {
                            tmpStr += "|" + item.Value.Trim();
                        }
                    }
                    tmpList.Add(tmpStr.Split('|'));

                    ////get next
                    if (isnext)
                    {
                        var isgo = true;
                        while (isgo)
                        {
                            m2 = m2.NextMatch();
                            isgo = m2.Success;
                            if (isgo)
                            {
                                tmpStr = "";
                                foreach (Group item in m2.Groups)
                                {
                                    if (string.IsNullOrEmpty(tmpStr))
                                    {
                                        tmpStr = item.Value.Trim();
                                    }
                                    else
                                    {
                                        tmpStr += "|" + item.Value.Trim();
                                    }
                                }
                                tmpList.Add(tmpStr.Split('|'));
                            }
                        }

                    }

                    return tmpList;
                }
                else
                {
                    tmpList.Add(input.Split('|'));
                    return tmpList;
                }
            }
            catch (Exception)
            {
                tmpList.Add(input.Split('|'));
                return tmpList;
                //throw;
            }

        }
    }
}
