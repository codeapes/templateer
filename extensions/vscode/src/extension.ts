import * as completion from './completion/completion';
import * as vscode from 'vscode';

export function activate(context: vscode.ExtensionContext) {
    // TODO: Start backend
    var liveTemplateItemProvider = new completion.LiveTemplateItemProvider();
    var disposable = vscode.languages.registerCompletionItemProvider('*', liveTemplateItemProvider, '*');
    context.subscriptions.push(disposable);
}

export function deactivate() {
    // TODO: Stop backend 
}