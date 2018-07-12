import { Component, OnInit } from "@angular/core";
import { RecipeEntry, RecipeStep } from "../../../models";
import { HttpClient } from "../../../../../node_modules/@angular/common/http";
import { ActivatedRoute, Router } from "../../../../../node_modules/@angular/router";
import { environment } from "../../../../environments/environment";
import {
  FormBuilder,
  FormGroup,
  Validators
} from "../../../../../node_modules/@angular/forms";
import { AlertService } from "../../../../../node_modules/ngx-alerts";

@Component({
  selector: "app-edit-recipe",
  templateUrl: "./edit-recipe.component.html"
})
export class EditRecipeComponent implements OnInit {
  recipeId: string;
  recipe: RecipeEntry;
  editRecipeEntryForm: FormGroup;
  editRecipeEntryFormError: string;
  addRecipeStepForm: FormGroup;

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private alertService: AlertService
  ) {
    this.route.params.subscribe(params => (this.recipeId = params["id"]));
  }

  ngOnInit() {
    this.editRecipeEntryForm = this.fb.group({
      id: [null, Validators.required],
      title: [null, [Validators.required, Validators.maxLength(100)]],
      description: [null, [Validators.required, Validators.maxLength(100)]],
      notes: [null, [Validators.maxLength(500)]]
    });

    this.http
      .get<RecipeEntry>(
        `${environment.apiLocation}/api/recipes/${this.recipeId}`
      )
      .subscribe(value => {
        this.recipe = value;

        this.editRecipeEntryForm.patchValue(value);
      });
  }

  updateRecipeEntry() {
    this.editRecipeEntryFormError = null;
    this.http
      .put<RecipeEntry>(
        `${environment.apiLocation}/api/recipes/${this.recipeId}`,
        this.editRecipeEntryForm.value
      )
      .subscribe(
        recipe => {
          this.alertService.success('Recipe Updated');
          this.recipe = recipe;
        },
        error => {
          this.alertService.danger('Update Error');
          console.log(error);
          this.editRecipeEntryFormError = error.error;
        }
      );
  }

  deleteStep(id: string) {
    this.http
    .delete(
      `${environment.apiLocation}/api/recipes/${this.recipeId}/steps/${id}`
    )
    .subscribe(() => {
      this.alertService.success('Step Deleted');
      this.recipe.recipeEntrySteps = this.recipe.recipeEntrySteps.filter(c => c.id === id);
    },
      error => {
        this.alertService.danger('Error');
        console.log(error);
      }
    );
  }

  deleteRecipe() {
    this.http.delete( `${environment.apiLocation}/api/recipes/${this.recipeId}`)
    .subscribe(() => {
      this.alertService.success('Recipe Deleted');
      this.router.navigate(['../'], { relativeTo: this.route });
    }, (error) => {
      this.alertService.danger('Error');
    });
  }
}
