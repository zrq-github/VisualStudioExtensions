using System.Diagnostics;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace LeetCodeVsExtension.Utils;

internal static class VsExtensionUtil
{
    private static readonly string VsExtensionUserDataPath = $@"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VsExtension")}";
    public static string WebView2UserDataFolderPath { get; } = VsExtensionUserDataPath;

    #region Test Visual Studio Community 工具包

    /// <summary>
    /// 生成项目
    /// </summary>
    private static async Task Community_BuildProjectAsync()
    {
        var project = await VS.Solutions.GetActiveProjectAsync();
        if (project != null)
            await project.BuildAsync(BuildAction.Rebuild);
    }

    /// <summary>
    /// 生成解决方案
    /// </summary>
    private static async Task Community_BuildSolutionAsync()
    {
        var buildStarted = await VS.Build.BuildSolutionAsync();
    }

    /// <summary>
    /// 获取生成属性
    /// </summary>
    private static async Task Community_GetAttributeAsync()
    {
        var item = await VS.Solutions.GetActiveProjectAsync();
        var value = await item?.GetAttributeAsync("propertyName")!;
    }

    /// <summary>
    /// 判断是不是黑暗主题
    /// </summary>
    private static bool Community_IsDarkTheme()
    {
        var vsShell = (IVsUIShell5)ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShell));
        if (vsShell != null)
        {
            // 获取当前主题
            var isDark = false;
            //vsShell.GetThemedColor(ENVVSUIELEMENT.VSUIE_WINDOW, ENVVSUICOLOR.VSCOLOR_WINDOW_BACKGROUND_DARK, out uint backgroundColor);
            //vsShell.GetThemedColor(ENVVSUIELEMENT.VSUIE_WINDOW, ENVVSUICOLOR.VSCOLOR_WINDOW_TEXT_DARK, out uint textColor);

            // 这里可以根据 backgroundColor 或者 textColor 的值来判断当前主题是深色还是浅色
            // 在深色主题下，通常 backgroundColor 会比 textColor 更接近黑色

            return isDark;
        }

        // 默认返回 false
        return false;
    }

    private static void Community_PrintProjectInfo()
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        var dte = Package.GetGlobalService(typeof(DTE)) as DTE;
        var solution = dte.Solution;
        for (var i = 0; i < solution.Projects.Count; i++)
        {
            var project = solution.Projects.Item(i);
            var projectName = project.Name;
            var projectPath = project.FullName;
            Debug.WriteLine($"Project Name: {projectName}, Path: {projectPath}");
        }
    }

    /// <summary>
    /// 设置生成属性
    /// </summary>
    private static async Task Community_SetAttributeAsync()
    {
        var project = await VS.Solutions.GetActiveProjectAsync();
        var succeeded = await project?.TrySetAttributeAsync("propertyName", "value")!;
    }

    /// <summary>
    /// 获取活动文本视图
    /// </summary>
    /// <remarks>
    /// 获取当前活动文本视图以操作其文本缓冲区文本。
    /// </remarks>
    private static async Task Community_GetActiveDocumentViewAsync()
    {
        var docView = await VS.Documents.GetActiveDocumentViewAsync();
        if (docView?.TextView == null) return; //not a text window
        var position = docView.TextView.Caret.Position.BufferPosition;
        docView.TextBuffer?.Insert(position, "some text"); // Inserts text at the caret
    }

    /// <summary>
    /// 打开文件
    /// </summary>
    private static async Task Community_OpenFile()
    {
        var fileName = "c:\\file.txt";
        await VS.Documents.OpenAsync(fileName);
    }

    /// <summary>
    /// 来自文件的 SolutionItem
    /// </summary>
    private static async Task Community_GetFilePathByPhysical()
    {
        var fileName = "c:\\file.txt";
        var item = await PhysicalFile.FromFileAsync(fileName);
    }

    #endregion
}
