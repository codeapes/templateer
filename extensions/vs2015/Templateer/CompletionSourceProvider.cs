
namespace CodeApes.Templateer
{
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Utilities;
    using Microsoft.VisualStudio.Text.Operations;

    [Export(typeof(ICompletionSourceProvider))]
    [ContentType("any")]
    [Name("token completion")]
    internal class CompletionSourceProvider : ICompletionSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new CompletionSource(this, textBuffer);
        }
    }
}
