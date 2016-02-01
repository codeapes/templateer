/// <reference path="completion.ts" />
import {CompletionItem} from 'vscode';

export interface ISearchTree
{
    insert(toIndex: string, item: CompletionItem) : void;
    
    search(search: string) : CompletionItem[];
}
    
export class SearchTree implements ISearchTree {
    
    private item : CompletionItem;
    private isRoot : boolean;
    private key : string;
    private children : SearchTree[];
    
    constructor(isRoot: boolean, key?: string) {
        this.isRoot = isRoot;
        this.key = key;
        this.children = [];
    }
    
    search(search: string) : CompletionItem[] {
        
        if(this.isRoot) {
            return this.searchInChildren(search).filter(function(n) { return n != undefined });
        }
        
        if(search.length == 1 && this.isResponsibleFor(search.charAt(0))) {
            return this.getItems().filter(function(n) { return n != undefined });
        }
        
        if(search.length > 1 && this.isResponsibleFor(search.charAt(0))) {
            return this.searchInChildren(search.substring(1)).filter(function(n) { return n != undefined });
        }
    }
    
    private getItems() : CompletionItem[] {
        var items = [];
        if(this.children.length > 0) {
            for(var key in this.children) {
                items.push.apply(items, this.children[key].getItems());
            }
        }
        
        items.push(this.item);
        return items;
    }
    
    private searchInChildren(search: string) : CompletionItem[] {
        if(search.length > 1) {
            for(var key in this.children) {
                if(this.children[key].isResponsibleFor(search))
                {
                    return this.children[key].search(search);
                }
            }
        }
        
        return this.getItems();
    }
    
    private isResponsibleFor(c: string) : boolean {
        if(this.isRoot) {
            return true;
        }
        
        if(c.length > 1) {
            return c.substr(0, 1) == this.key;
        }
        
        return c == this.key;
    }
    
    insert(toIndex: string, item: CompletionItem) : void {
        if(this.isRoot) {
            this.insertIntoChildren(toIndex, item);
            return;
        }
        
        if(toIndex.length > 1) {
            this.insertIntoChildren(toIndex.substring(1), item);
        } else {
            this.item = item; 
        }
    }
    
    private insertIntoChildren(toIndex: string, item: CompletionItem) : void {
        var inserted = false;
        for (var key in this.children) {
            if(this.children[key].isResponsibleFor(toIndex.substr(0,1))) {
                this.children[key].insert(toIndex.substring(1), item);
                inserted = true;
                break;
            }
        }
        
        if(!inserted) {
            var tree = new SearchTree(false, toIndex.substr(0, 1));
            tree.insert(toIndex.substring(1), item);
            this.children.push(tree);
        }
    }
}