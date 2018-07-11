import { Component, OnInit } from '@angular/core';
import { HttpClient } from '../../../../node_modules/@angular/common/http';
import { Observable } from '../../../../node_modules/rxjs';
import { RecipeEntry } from '../../models';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html'
})
export class RecipesComponent implements OnInit {

  constructor(private http: HttpClient) { }

  recipeList: Observable<RecipeEntry[]>;

  ngOnInit() {
    this.recipeList = this.http.get<RecipeEntry[]>(`${environment.apiLocation}/api/recipes`);
  }
}
