import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { AuthGuard, AuthService, HttpAuthInterceptor } from './auth';
import { JwtModule } from '@auth0/angular-jwt';
import { environment } from '../environments/environment';
import { CommonModule } from '../../node_modules/@angular/common';
import { RecipesListComponent } from './components/recipes/recipes-list/recipes-list.component';
import { RecipesComponent } from './pages/recipes/recipes.component';
import { MyRecipesComponent } from './pages/my-recipes/my-recipes.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { ContentComponent } from './components/content/content.component';
import { ViewRecipeComponent } from './components/recipes/view-recipe/view-recipe.component';
import { AddRecipeComponent } from './components/recipes/add-recipe/add-recipe.component';
import { EditRecipeComponent } from './components/recipes/edit-recipe/edit-recipe.component';
import { SignInComponent } from './pages/sign-in/sign-in.component';
import { SignUpComponent } from './pages/sign-up/sign-up.component';
import { UpdateRecipeStepComponent } from './components/recipes/update-recipe-step/update-recipe-step.component';

export function tokenGetter() {
  return localStorage.getItem('access_token');
}

@NgModule({
  declarations: [
    AppComponent,
    RecipesListComponent,
    RecipesComponent,
    MyRecipesComponent,
    NavigationComponent,
    ContentComponent,
    ViewRecipeComponent,
    AddRecipeComponent,
    EditRecipeComponent,
    SignInComponent,
    SignUpComponent,
    UpdateRecipeStepComponent
  ],
  imports: [
    CommonModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      {
        path: '', redirectTo: 'recipes', pathMatch: 'full'
      },
      {
        path: 'recipes',
        component: ContentComponent,
        children: [
          { path: '', component: RecipesComponent },
          { path: 'details/:id', component: ViewRecipeComponent },
          { path: 'my-recipes', component: MyRecipesComponent, canActivate: [AuthGuard] },
          { path: 'my-recipes/update/:id', component: EditRecipeComponent, canActivate: [AuthGuard] },
          { path: 'my-recipes/update/:id/steps/:stepId', component: UpdateRecipeStepComponent, canActivate: [AuthGuard] },
          { path: 'my-recipes/addnew', component: AddRecipeComponent, canActivate: [AuthGuard] },
        ]
      },
      { path: 'signin', component: SignInComponent },
      { path: 'signup', component: SignUpComponent },
      { path: '**', redirectTo: 'recipes' }
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpAuthInterceptor, multi: true },
    AuthGuard, AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
