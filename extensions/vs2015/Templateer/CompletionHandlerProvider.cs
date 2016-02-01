
namespace CodeApes.Templateer
{
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Language.Intellisense;

    [Export(typeof(IVsTextViewCreationListener))]
    [Name("token completion handler")]
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    internal class CompletionHandlerProvider : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService;

        [Import]
        internal ICompletionBroker CompletionBroker { get; set; }

        [Import]
        internal SVsServiceProvider ServiceProvider { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            if (textView == null)
            {
                return;
            }
                
            textView.Properties.GetOrCreateSingletonProperty(() => new CompletionCommandHandler(textViewAdapter, textView, this));
        }
    }
}
