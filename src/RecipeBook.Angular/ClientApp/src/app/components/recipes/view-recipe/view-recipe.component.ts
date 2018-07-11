import { Component, OnInit } from "@angular/core";
import { RecipeEntry } from "../../../models";
import { HttpClient } from "../../../../../node_modules/@angular/common/http";
import { ActivatedRoute } from "../../../../../node_modules/@angular/router";
import { environment } from "../../../../environments/environment";

@Component({
  selector: "app-view-recipe",
  templateUrl: "./view-recipe.component.html"
})
export class ViewRecipeComponent implements OnInit {
  recipeId: string;
  recipe: RecipeEntry;

  constructor(private http: HttpClient, private route: ActivatedRoute) {
    this.route.params.subscribe(params => (this.recipeId = params["id"]));
  }

  ngOnInit() {
    this.http
      .get<RecipeEntry>(
        `${environment.apiLocation}/api/recipes/${this.recipeId}`
      )
      .subscribe(value => (this.recipe = value));
  }
}
