import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { SignInComponent } from './components/sign-in/sign-in.component';
import { AuthGuard, AuthService } from './auth';
import { RecipeListComponent } from './components/recipe-list/recipe-list.component';
import { MyRecipeListComponent } from './components/my-recipe-list/my-recipe-list.component';
import { ViewRecipeComponent } from './components/view-recipe/view-recipe.component';
import { AddRecipeComponent } from './components/add-recipe/add-recipe.component';
import { EditRecipeComponent } from './components/edit-recipe/edit-recipe.component';
import { HomeComponent } from './components/home/home.component';
import { JwtModule } from '@auth0/angular-jwt';
import { environment } from '../environments/environment';
import { CommonModule } from '../../node_modules/@angular/common';
export function tokenGetter() {
  console.log(localStorage.getItem('access_token'));
  return localStorage.getItem('access_token');
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SignUpComponent,
    SignInComponent,
    RecipeListComponent,
    ViewRecipeComponent,
    MyRecipeListComponent,
    AddRecipeComponent,
    EditRecipeComponent
  ],
  imports: [
    CommonModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: [environment.apiLocation],
        blacklistedRoutes: [
          `${environment.apiLocation}/auth/session/signup`,
          `${environment.apiLocation}/auth/session/authorize`
        ]
      }
    }),
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'recipes', pathMatch: 'full' },
      {
        path: 'recipes',
        component: HomeComponent,
        children: [
          { path: '', component: RecipeListComponent },
          { path: 'details/:id', component: ViewRecipeComponent },
          { path: 'addnew', component: AddRecipeComponent, canActivate: [AuthGuard] },
          { path: '**', redirectTo: '' }
        ]
      },
      { path: 'signin', component: SignInComponent },
      { path: 'signup', component: SignUpComponent },
      { path: '**', redirectTo: '' }
    ])
  ],
  providers: [AuthGuard, AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
