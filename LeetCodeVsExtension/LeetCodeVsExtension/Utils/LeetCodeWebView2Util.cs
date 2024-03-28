using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCodeVsExtension.Utils
{
    internal static class LeetCodeWebView2Util
    {
        /// <summary>
        /// LeetCode 跟随系统
        /// </summary>
        /// <remarks>
        /// 因为不知道设置 WebView2 本身默认的设置, 暂时先不使用
        /// </remarks>
        private static void ColorToSystem(Microsoft.Web.WebView2.Wpf.WebView2 webView2)
        {
            throw new NotImplementedException();
            _ = webView2.ExecuteScriptAsync("document.getElementByClassName('flex cursor-pointer items-center space-x-3 rounded px-2 py-[10px] hover:bg-fill-4 dark:hover:bg-dark-fill-4 text-text-secondary dark:text-text-secondary')[0].click()");
        }

        /// <summary>
        /// 将外观设置为深色
        /// </summary>
        /// <remarks> 通过执行JS代码,模拟点击的操作实现 </remarks>
        public static async Task ColorToLightAsync(Microsoft.Web.WebView2.Wpf.WebView2 webView2)
        {
            _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
            _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('grow text-left')[9].click()");
            _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
        }

        /// <summary>
        /// 将外观设置为深色
        /// </summary>
        /// <remarks> 通过执行JS代码,模拟点击的操作实现 </remarks>
        public static async Task ColorToDarkAsync(Microsoft.Web.WebView2.Wpf.WebView2 webView2)
        {
            _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
            _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('grow text-left')[10].click()");
            _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
        }
    }
}
