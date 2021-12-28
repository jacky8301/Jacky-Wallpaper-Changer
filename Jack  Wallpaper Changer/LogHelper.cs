using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacky__Wallpaper_Changer
{
    public class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("MsgLogger");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("DebugRFLogger");
        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        public static void WriteLog(string info, Exception ex)
        {
            if (loginfo.IsErrorEnabled)
            {
                logerror.Error(info, ex);
            }
        }
    }
}
