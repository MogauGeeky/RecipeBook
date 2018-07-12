import { Component, OnInit } from "@angular/core";
import {
  FormGroup,
  FormBuilder,
  Validators
} from "../../../../../node_modules/@angular/forms";
import { HttpClient } from "../../../../../node_modules/@angular/common/http";
import {
  ActivatedRoute,
  Router
} from "../../../../../node_modules/@angular/router";
import { RecipeStep } from "../../../models";
import { environment } from "../../../../environments/environment";
import { AlertService } from "../../../../../node_modules/ngx-alerts";

@Component({
  selector: "app-update-recipe-step",
  templateUrl: "./update-recipe-step.component.html"
})
export class UpdateRecipeStepComponent implements OnInit {
  recipeStepId: string;
  recipeId: string;
  updateRecipeStepFormError: string;
  updateRecipeStepForm: FormGroup;
  isNew: boolean;

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private alertService: AlertService
  ) {
    this.route.params.subscribe(params => {
      this.recipeId = params["id"];
      if (params["stepId"] === "add") {
        this.isNew = true;
      } else {
        this.recipeStepId = params["stepId"];
      }
    });
  }

  ngOnInit() {
    this.updateRecipeStepForm = this.fb.group({
      id: [''],
      recipeId: [''],
      notes: [null, [Validators.required, Validators.maxLength(500)]]
    });

    setTimeout(() => {
      if (!this.isNew) {
        this.http
        .get<RecipeStep>(`${environment.apiLocation}/api/recipes/${this.recipeId}/steps/${this.recipeStepId}`
        )
        .subscribe(recipeStep => {
          recipeStep["recipeId"] = this.recipeId;
          this.updateRecipeStepForm.patchValue(recipeStep);
        });
      }
    });
  }

  updateRecipeStep() {
    this.updateRecipeStepFormError = null;

    if (this.isNew) {
      this.http
      .post<RecipeStep>(
        `${environment.apiLocation}/api/recipes/${this.recipeId}/steps`,
        this.updateRecipeStepForm.value
      )
      .subscribe(
        recipe => {
          this.alertService.success('Step added');
          this.router.navigate(["../../"], { relativeTo: this.route });
        },
        error => {
          this.alertService.danger('Error');
          this.updateRecipeStepFormError = error.error;
        }
      );
    } else {
      this.http
      .put<RecipeStep>(
        `${environment.apiLocation}/api/recipes/${this.recipeId}/steps/${this.recipeStepId}`,
        this.updateRecipeStepForm.value
      )
      .subscribe(
        recipe => {
          this.alertService.success('Step updated');
          this.router.navigate(["../../"], { relativeTo: this.route });
        },
        error => {
          this.alertService.danger('Update Error');
          this.updateRecipeStepFormError = error.error;
        }
      );
    }
  }
}
