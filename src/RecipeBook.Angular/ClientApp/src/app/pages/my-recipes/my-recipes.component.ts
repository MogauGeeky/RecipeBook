import { Component, OnInit } from "@angular/core";
import { Observable } from "../../../../node_modules/rxjs";
import { RecipeEntry } from "../../models";
import { HttpClient } from "../../../../node_modules/@angular/common/http";
import { AuthService } from "../../auth";
import { environment } from "../../../environments/environment";
import "rxjs/add/operator/concatMap";
import "rxjs/add/operator/filter";
import "rxjs/add/observable/from";
import "rxjs/add/operator/toArray";
import {
  Router,
  ActivatedRoute
} from "../../../../node_modules/@angular/router";
import { AlertService } from "../../../../node_modules/ngx-alerts";

@Component({
  selector: "app-my-recipes",
  templateUrl: "./my-recipes.component.html"
})
export class MyRecipesComponent implements OnInit {
  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private alertService: AlertService
  ) {}

  recipeList: Observable<RecipeEntry[]>;

  ngOnInit() {
    this.recipeList = this.http
      .get<RecipeEntry[]>(`${environment.apiLocation}/api/recipes`)
      .concatMap((array: Array<RecipeEntry>) => {
        const userId = this.authService.getUserId();
        const usersRecipes = array.filter(c => c.ownerId === userId);
        return Observable.from(usersRecipes);
      })
      .toArray();
  }

  onRecipeClicked(id: string) {
    this.router.navigate(["update/", id], { relativeTo: this.route });
  }
}
