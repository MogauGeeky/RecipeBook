import { Component, OnInit } from '@angular/core';
import { HttpClient } from '../../../../node_modules/@angular/common/http';
import { Observable } from '../../../../node_modules/rxjs';
import { RecipeEntry } from '../../models';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.css']
})
export class RecipeListComponent implements OnInit {

  constructor(private http: HttpClient) { }

  recipeList: Observable<RecipeEntry[]>;

  ngOnInit() {
    this.recipeList = this.http.get<RecipeEntry[]>(`${environment.apiLocation}/api/recipes`);
  }

}
