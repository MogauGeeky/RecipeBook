import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { RecipeEntry } from "../../../models";

@Component({
  selector: "app-recipes-list",
  templateUrl: "./recipes-list.component.html"
})
export class RecipesListComponent {
  @Input() items: RecipeEntry[] = [];
  @Output() itemClicked: EventEmitter<String> = new EventEmitter<String>();

  onItemClick(item: RecipeEntry) {
    this.itemClicked.emit(item.id);
  }
}
