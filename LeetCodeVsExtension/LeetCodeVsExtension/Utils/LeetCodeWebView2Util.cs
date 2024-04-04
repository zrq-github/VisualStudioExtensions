using Microsoft.Web.WebView2.Wpf;
using System.Threading.Tasks;

namespace LeetCodeVsExtension.Utils;

internal static class LeetCodeWebView2Util
{
    /// <summary>
    /// 将外观设置为深色
    /// </summary>
    /// <remarks> 通过执行JS代码,模拟点击的操作实现 </remarks>
    public static async Task ColorToDarkAsync(WebView2 webView2)
    {
        _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
        _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('grow text-left')[10].click()");
        _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
    }

    /// <summary>
    /// 将外观设置为深色
    /// </summary>
    /// <remarks> 通过执行JS代码,模拟点击的操作实现 </remarks>
    public static async Task ColorToLightAsync(WebView2 webView2)
    {
        _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
        _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('grow text-left')[9].click()");
        _ = await webView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
    }

    /// <summary>
    /// 获取题目名字
    /// </summary>
    /// <remarks> 获取题目名字 需要先打开名字 </remarks>
    public static async Task<string> GetTopicNameAsync(WebView2 webView2)
    {
        var jsString = await webView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-start gap-2')[0].innerText");
        return System.Text.Json.JsonSerializer.Deserialize<string>(jsString);
    }

    /// <summary>
    /// 获取代码位置的数据
    /// </summary>
    public static async Task<string> GetCode(WebView2 webView2)
    {
        var jsString = await webView2.ExecuteScriptAsync("document.getElementsByClassName('view-lines monaco-mouse-cursor-text')[0].innerText");
        return System.Text.Json.JsonSerializer.Deserialize<string>(jsString);
    }

    /// <summary>
    /// LeetCode 跟随系统
    /// </summary>
    /// <remarks>
    /// 因为不知道设置 WebView2 本身默认的设置, 暂时先不使用
    /// </remarks>
    private static void ColorToSystem(WebView2 webView2)
    {
        throw new NotImplementedException();
        _ = webView2.ExecuteScriptAsync("document.getElementByClassName('flex cursor-pointer items-center space-x-3 rounded px-2 py-[10px] hover:bg-fill-4 dark:hover:bg-dark-fill-4 text-text-secondary dark:text-text-secondary')[0].click()");
    }
}
