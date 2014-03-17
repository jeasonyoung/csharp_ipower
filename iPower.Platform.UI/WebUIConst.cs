using System;
using System.Collections.Generic;
using System.Text;

using iPower.Platform;
namespace iPower.Platform.UI
{
    /// <summary>
    ///  Web UI 公共常量。
    /// </summary>
    public class WebUIConst //:PlatformConst
    {
        #region 系统常量．
        /// <summary>
        /// 获取Banner Image。
        /// </summary>
        public static string BannerImageDesc
        {
            get
            {
                return "<img src=\"{0}\" width=\"{1}\" height=\"80\"/>";
            }
        }
        /// <summary>
        /// 获取Banner falsh。
        /// </summary>
        public static string BannerFlashDesc
        {
            get
            {
                return "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0\" width=\"{1}\" height=\"80\" id=\"flashLog\"><param name=\"movie\" value=\"{0}\"><param name=\"quality\" value=\"high\"><param name=\"wmode\" value=\"transparent\" /><param name=\"LOOP\" value=\"true\"><embed src=\"{0}\" width=\"{1}\" height=\"80\" loop=\"false\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\"  name=\"flashLog\" swliveconnect=\"true\" ></embed></object>";
            }
        }
        /// <summary>
        /// 获取保存脚本。
        /// </summary>
        public static string SaveReturnScript
        {
            get
            {
                return "<script language='javascript'>window.returnValue='{0}'; window.close();</script>";
            }
        }
        /// <summary>
        /// 获取错误信息字段。
        /// </summary>
        public static string ErrorMessage_QueryField
        {
            get { return "ErrorMessage"; }
        }
        #endregion
    }
}
