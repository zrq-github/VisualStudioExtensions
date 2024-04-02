using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Imaging;

namespace LeetCodeVsExtension;

public class MyToolWindow : BaseToolWindow<MyToolWindow>
{
    public override Type PaneType => typeof(Pane);

    public override Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
    {
        return Task.FromResult<FrameworkElement>(new MyToolWindowControl());
    }

    public override string GetTitle(int toolWindowId)
    {
        return "Leet Code";
    }

    #region Nested type: Pane

    [Guid("8cb87add-3e49-44b6-8e69-048d8960ad36")]
    internal class Pane : ToolkitToolWindowPane
    {
        public Pane()
        {
            BitmapImageMoniker = KnownMonikers.ToolWindow;
        }
    }

    #endregion
}
