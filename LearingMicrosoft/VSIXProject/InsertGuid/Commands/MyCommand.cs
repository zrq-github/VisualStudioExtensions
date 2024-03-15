using Microsoft.VisualStudio.Text;
using System.Linq;

namespace InsertGuid
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var docView = await VS.Documents.GetActiveDocumentViewAsync();
            var selection = docView?.TextView.Selection.SelectedSpans.FirstOrDefault();
            if (!selection.HasValue)
                return;

            var insertGuid = Guid.NewGuid().ToString();
            docView.TextBuffer.Replace(selection.Value, insertGuid);
        }
    }
}
