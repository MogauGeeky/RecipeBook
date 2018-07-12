import { Component, OnInit } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  Validators
} from "../../../../../node_modules/@angular/forms";
import { HttpClient } from "../../../../../node_modules/@angular/common/http";
import { environment } from "../../../../environments/environment";
import {
  Router,
  ActivatedRoute
} from "../../../../../node_modules/@angular/router";
import { RecipeEntry } from "../../../models";
import { AlertService } from "../../../../../node_modules/ngx-alerts";

@Component({
  selector: "app-add-recipe",
  templateUrl: "./add-recipe.component.html"
})
export class AddRecipeComponent implements OnInit {
  addRecipeForm: FormGroup;
  addRecipeFormError: string;

  constructor(
    private http: HttpClient,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.addRecipeForm = this.fb.group({
      title: [null, [Validators.required, Validators.maxLength(100)]],
      description: [null, [Validators.required, Validators.maxLength(100)]],
      notes: [null, [Validators.maxLength(500)]]
    });
  }

  addRecipe() {
    this.http
      .post<RecipeEntry>(
        `${environment.apiLocation}/api/recipes`,
        this.addRecipeForm.value
      )
      .subscribe(
        recipe => {
          this.alertService.success('Added');
          this.router.navigate(["../update", recipe.id], {
            relativeTo: this.route
          });
        },
        error => {
          this.alertService.success('Error');
          this.addRecipeFormError = error.error;
        }
      );
  }
}
