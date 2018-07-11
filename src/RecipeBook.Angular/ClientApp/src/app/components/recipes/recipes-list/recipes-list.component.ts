import { Component, OnInit, Input } from "@angular/core";
import { RecipeEntry } from "../../../models";

@Component({
  selector: "app-recipes-list",
  templateUrl: "./recipes-list.component.html"
})
export class RecipesListComponent {
  @Input() items: RecipeEntry[] = [];
}
