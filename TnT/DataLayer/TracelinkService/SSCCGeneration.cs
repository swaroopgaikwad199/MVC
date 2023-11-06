using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.DataLayer.TracelinkService
{
    public class SSCCGeneration
    {
        private static List<string> SSCCList;

        private static List<string> ErrorList;

        public static List<string> GenerateSSCC(int Qty, string CompanyCode, string PlantCode, int LastSSCC, string Extension)
        {
            List<string> result;
            string text2 = string.Empty;
            string text = string.Empty;
            int totalWidth = 0;
            try
            {
                SSCCGeneration.SSCCList = new List<string>();
                if (Extension == null)
                {
                    text = DateTime.Now.ToString("yyyy");
                    text = text.Substring(3, 1);
                }
                else
                {
                    text = Extension;
                }

                if (PlantCode.Length > 2)
                {
                    PlantCode = PlantCode.Substring(PlantCode.Length - 2);
                }
                //text = text.Substring(3, 1);
                //int totalWidth = 17 - (text.ToString().Length + CompanyCode.Length + PlantCode.Length);
                //if (CompanyCode.Length == 7)
                //{
                totalWidth = 17 - (text.ToString().Length + CompanyCode.Length /*+ PlantCode.Length*/);
                //}
                //else
                //{
                //    totalWidth = 17 - (text.ToString().Length + CompanyCode.Length);
                //}
                for (int i = 0; i < Qty; i++)
                {
                    //if (CompanyCode.Length == 7)
                    //{
                         text2 = string.Format("{0}{1}", text, CompanyCode/*, PlantCode*/);
                    //}
                    //else
                    //{
                    //     text2 = string.Format("{0}{1}", text, CompanyCode);
                    //}
                    text2 += (LastSSCC + i).ToString().PadLeft(totalWidth, '0');
                    text2 += SSCCGeneration.GetGS1CheckSum(text2).ToString();
                    SSCCGeneration.SSCCList.Add(text2);
                }
                result = SSCCGeneration.SSCCList;
            }
            catch (Exception ex)
            {
                SSCCGeneration.ErrorList = new List<string>();
                SSCCGeneration.ErrorList.Add(ex.Message);
                SSCCGeneration.ErrorList.Add(ex.StackTrace);
                result = SSCCGeneration.ErrorList;
            }
            return result;
        }

        public static List<string> GenerateSSCC(int Qty, string CompanyCode, string PlantCode, int LastSSCC)
        {
            List<string> result;
            string text = string.Empty;
            text = DateTime.Now.ToString("yyyy");
            int totalWidth = 0;
            string text2 = string.Empty;
            try
            {
                SSCCGeneration.SSCCList = new List<string>();
                
                text = text.Substring(3, 1);
                //if (CompanyCode.Length == 7)
                //{
                     totalWidth = 17 - (text.ToString().Length + CompanyCode.Length + PlantCode.Length);
                //}
                //else 
                //{
                //    totalWidth = 17 - (text.ToString().Length + CompanyCode.Length);
                //}
                for (int i = 0; i < Qty; i++)
                {
                    //if (CompanyCode.Length == 7)
                    //{
                    //    text2 = string.Format("{0}{1}{2}", text, CompanyCode, PlantCode);
                    //}
                    //else
                    //{
                        text2 = string.Format("{0}{1}", text, CompanyCode);
                    //}

                   
                        text2 += (LastSSCC + i).ToString().PadLeft(totalWidth, '0');
                   
                    text2 += SSCCGeneration.GetGS1CheckSum(text2).ToString();
                    SSCCGeneration.SSCCList.Add(text2);
                }
                result = SSCCGeneration.SSCCList;
            }
            catch (Exception ex)
            {
                SSCCGeneration.ErrorList = new List<string>();
                SSCCGeneration.ErrorList.Add(ex.Message);
                SSCCGeneration.ErrorList.Add(ex.StackTrace);
                result = SSCCGeneration.ErrorList;
            }
            return result;
        }

        public static int GetGS1CheckSum(string GS1Data)
        {
            char[] array = GS1Data.ToCharArray();
            int num = 0;
            int num2 = 3;
            for (int i = 0; i < array.Length; i++)
            {
                int num3 = int.Parse(array[i].ToString());
                int num4 = num3 * num2;
                if (num2 == 3)
                {
                    num2 = 1;
                }
                else
                {
                    num2 = 3;
                }
                num += num4;
            }
            decimal num5 = num;
            num5 /= 10m;
            decimal value = Math.Ceiling(num5);
            int num6 = (int)value * 10;
            return num6 - num;
        }
    }
}