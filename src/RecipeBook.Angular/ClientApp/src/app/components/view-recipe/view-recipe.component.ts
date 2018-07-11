import { Component, OnInit } from '@angular/core';
import { RecipeEntry } from '../../models';
import { environment } from '../../../environments/environment';
import { ActivatedRoute } from '../../../../node_modules/@angular/router';
import { HttpClient } from '../../../../node_modules/@angular/common/http';

@Component({
  selector: 'app-view-recipe',
  templateUrl: './view-recipe.component.html',
  styleUrls: ['./view-recipe.component.css']
})
export class ViewRecipeComponent implements OnInit {

  recipeId: string;
  recipe: RecipeEntry;

  constructor(private http: HttpClient, private route: ActivatedRoute) {
    this.route.params.subscribe(params => this.recipeId = params['id']);
  }

  ngOnInit() {
    this.http.get<RecipeEntry>(`${environment.apiLocation}/api/recipes/${this.recipeId}`).subscribe(value => this.recipe = value);
  }
}
