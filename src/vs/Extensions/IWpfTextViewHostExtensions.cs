using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Luminous.Code.VisualStudio.Extensions.IWpfTextViewHostExtensions
{
    public static class IWpfTextViewHostExtensions
    {
        // return the ITextDocument for the IWpfTextViewHost that represents the currently selected editor pane
        public static ITextDocument GetTextDocumentForView(this IWpfTextViewHost viewHost)
        {
            viewHost.TextView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(ITextDocument), out ITextDocument document);

            return document;
        }

        public static ITextSelection GetSelection(this IWpfTextViewHost viewHost)
            => viewHost.TextView.Selection;
    }
}