

namespace CodeApes.Templateer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Operations;

    internal class CompletionSource : ICompletionSource
    {
        private readonly CompletionSourceProvider sourceProvider;
        private readonly ITextBuffer textBuffer;
        private List<Completion> completions;
        private bool isDisposed;

        public CompletionSource(CompletionSourceProvider sourceProvider, ITextBuffer textBuffer)
        {
            this.sourceProvider = sourceProvider;
            this.textBuffer = textBuffer;
        }

        void ICompletionSource.AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            IDictionary<string, string> availableCompletions = new Dictionary<string, string>();
            availableCompletions.Add("dummy", "Dummy Live Template added");

            completions = availableCompletions.Select(x => new Completion(x.Key, x.Value, x.Key, null, null)).ToList();

            var applicableTo = FindTokenSpanAtPosition(session.GetTriggerPoint(textBuffer), session);
            completionSets.Add(new CompletionSet("templateer", "templateer", applicableTo, completions, null));
        }

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
        {
            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;
            ITextStructureNavigator navigator = sourceProvider.NavigatorService.GetTextStructureNavigator(textBuffer);
            TextExtent extent = navigator.GetExtentOfWord(currentPoint);
            return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
        }
        
        public void Dispose()
        {
            if (!isDisposed)
            {
                GC.SuppressFinalize(this);
                isDisposed = true;
            }
        }
    }
}
