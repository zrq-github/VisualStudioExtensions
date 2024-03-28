using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCodeVsExtension.Utils
{
    internal static class VsExtensionUtil
    {
        public static string VsExtensionUserDataPath = $@"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VsExtension")}";
        public static string WebView2UserDataFolderPath = VsExtensionUtil.VsExtensionUserDataPath;
        public static bool IsDarkTheme()
        {
            IVsUIShell5 vsShell = (IVsUIShell5)ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShell));
            if (vsShell != null)
            {
                // 获取当前主题
                bool isDark = false;
                //vsShell.GetThemedColor(ENVVSUIELEMENT.VSUIE_WINDOW, ENVVSUICOLOR.VSCOLOR_WINDOW_BACKGROUND_DARK, out uint backgroundColor);
                //vsShell.GetThemedColor(ENVVSUIELEMENT.VSUIE_WINDOW, ENVVSUICOLOR.VSCOLOR_WINDOW_TEXT_DARK, out uint textColor);

                // 这里可以根据 backgroundColor 或者 textColor 的值来判断当前主题是深色还是浅色
                // 在深色主题下，通常 backgroundColor 会比 textColor 更接近黑色

                return isDark;
            }

            // 默认返回 false
            return false;
        }
    }
}
