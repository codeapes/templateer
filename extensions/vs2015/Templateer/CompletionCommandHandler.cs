
namespace CodeApes.Templateer
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;

    internal class CompletionCommandHandler : IOleCommandTarget
    {
        private readonly IOleCommandTarget nextCommandHandler;
        private readonly ITextView textView;
        private readonly CompletionHandlerProvider provider;
        private ICompletionSession session;

        internal CompletionCommandHandler(IVsTextView textViewAdapter, ITextView textView, CompletionHandlerProvider provider)
        {
            this.textView = textView;
            this.provider = provider;

            textViewAdapter.AddCommandFilter(this, out nextCommandHandler);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid commandGroupId, uint commandId, uint commandExecutionOption, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (VsShellUtilities.IsInAutomationFunction(provider.ServiceProvider))
            {
                return nextCommandHandler.Exec(ref commandGroupId, commandId, commandExecutionOption, pvaIn, pvaOut);
            }

            uint cmdId = commandId;
            char typedChar = char.MinValue;

            if (commandGroupId == VSConstants.VSStd2K && commandId == (uint)VSConstants.VSStd2KCmdID.TYPECHAR)
            {
                typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);
            }

            if (commandId == (uint)VSConstants.VSStd2KCmdID.RETURN
                || commandId == (uint)VSConstants.VSStd2KCmdID.TAB
                || (char.IsWhiteSpace(typedChar) || char.IsPunctuation(typedChar)))
            {
                if (session != null && !session.IsDismissed)
                {
                    if (session.SelectedCompletionSet.SelectionStatus.IsSelected)
                    {
                        session.Commit();
                        return VSConstants.S_OK;
                    }

                    session.Dismiss();
                }
            }
            
            int retVal = nextCommandHandler.Exec(ref commandGroupId, commandId, commandExecutionOption, pvaIn, pvaOut);
            bool handled = false;
            if (!typedChar.Equals(char.MinValue) && char.IsLetterOrDigit(typedChar))
            {
                if (session == null || session.IsDismissed)
                {
                    TriggerCompletion();
                    // ReSharper disable once PossibleNullReferenceException
                    session.Filter();
                }
                else
                {
                    session.Filter();
                }

                handled = true;
            }
            else if (cmdId == (uint)VSConstants.VSStd2KCmdID.BACKSPACE
                || cmdId == (uint)VSConstants.VSStd2KCmdID.DELETE)
            {
                if (session != null && !session.IsDismissed)
                {
                    session.Filter();
                }

                handled = true;
            }

            return handled ? VSConstants.S_OK : retVal;
        }

        private void TriggerCompletion()
        {
            var caretPoint = textView.Caret.Position.Point.GetPoint(textBuffer => 
                !textBuffer.ContentType.IsOfType("projection"), PositionAffinity.Predecessor);

            if (!caretPoint.HasValue)
            {
                return;
            }

            session = provider.CompletionBroker.CreateCompletionSession(textView, 
                caretPoint.Value.Snapshot.CreateTrackingPoint(caretPoint.Value.Position, PointTrackingMode.Positive),
                true);

            session.Dismissed += OnSessionDismissed;
            session.Start();
        }

        private void OnSessionDismissed(object sender, EventArgs e)
        {
            session.Dismissed -= OnSessionDismissed;
            session = null;
        }
    }
}
