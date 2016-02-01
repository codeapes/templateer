import {CompletionItemProvider, TextDocument, Position, CancellationToken, CompletionItem} from 'vscode';
import {SearchTree} from './searchtree';

export class LiveTemplateItemProvider implements CompletionItemProvider 
{ 
    private searchTree = new SearchTree(true);
    
    constructor() 
    {
        var dummy = new CompletionItem("dummy")
        dummy.insertText = 'Dummy Live Template added';
        this.searchTree.insert("dummy", dummy);
    }
    
    /**
     * Provide completion items for the given position and document.
     *
        * @param document The document in which the command was invoked.
        * @param position The position at which the command was invoked.
        * @param token A cancellation token.
        * @return An array of completions or a thenable that resolves to such. The lack of a result can be
        * signaled by returning `undefined`, `null`, an empty array.
        */
    provideCompletionItems(document: TextDocument, position: Position, token: CancellationToken) : CompletionItem[]
    {
        var line = document.lineAt(position);
        var stringToCurrentPosition = line.text.substring(0, position.character + 1);
        var splittedStrings = stringToCurrentPosition.split(' ');
        
        return this.searchTree.search(splittedStrings[splittedStrings.length - 1]);
    }

    /**
     * Given a completion item fill in more data, like [doc-comment](#CompletionItem.documentation)
     * or [details](#CompletionItem.detail).
     *
        * The editor will only resolve a completion item once.
        *
        * @param item A completion item currently active in the UI.
        * @param token A cancellation token.
        * @return The resolved completion item or a thenable that resolves to of such. It is OK to return the given
        * `item`. When no result is returned, the given `item` will be used.
        */
    resolveCompletionItem(item: CompletionItem, token: CancellationToken) : CompletionItem
    {
        throw Error("Not implemented yet");
    }
}