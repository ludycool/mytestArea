using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace WebApplication2
{
    internal class UICulInfoHelp
    {

       
        internal static CultureInfo GetCultureInfo(String lan)
        {
            try
            {
                if (String.IsNullOrEmpty(lan))
                {
                    lan = "zh-cn";
                }
                switch (lan.ToLower())
                {
                    case "zh-cn":
                    case "zh":
                    case "zh-hans":
                    case "zh-hans-cn":
                        lan = "zh-cn";
                        break;
                    case "zh-hk":
                    case "zh-tw":
                  
                    case "zh-hant":
                    case "zh-hant-cn":
                        lan = "zh-hk";
                        break;

                    default:
                        lan = "en-US";
                        break;
                }
                return new CultureInfo(lan);
            }
            catch (Exception ex)
            {

                return new CultureInfo("zh-cn");
            }
        }


        internal static CultureInfo GetCurrent()
        {
            return Thread.CurrentThread.CurrentUICulture;
        }
    }
}
