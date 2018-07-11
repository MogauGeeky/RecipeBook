import { Component, OnInit } from '@angular/core';
import { Router } from '../../../../node_modules/@angular/router';
import { AuthService } from '../../auth';
import { FormBuilder, Validators, FormGroup, ValidationErrors, AbstractControl } from '../../../../node_modules/@angular/forms';
import { Observable, Observer } from '../../../../node_modules/rxjs';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  signUpForm: FormGroup;
  signUpError: string;

  constructor(private router: Router, private authService: AuthService, private fb: FormBuilder) { }

  ngOnInit() {
    if (this.authService.isAuthorised()) {
      this.router.navigate(['/']);
    }

    this.signUpForm = this.fb.group({
      fullname: [null, [Validators.required, Validators.maxLength(100)]],
      username: [null, [Validators.required, Validators.maxLength(60)]],
      password: [null, [Validators.required, Validators.minLength(8)]],
      confirmPassword: [null, [Validators.required],
      (control: AbstractControl) => Observable.create((observer: Observer<ValidationErrors>) => {
        if (control.value !== this.signUpForm.controls.password.value) {
          observer.next({ error: true, confirm: true });
        } else {
          observer.next(null);
        }

        observer.complete();
      })]
    });
  }

  signUp() {
    this.signUpError = null;
    this.authService.signUp(this.signUpForm.value).then(() => {
      this.router.navigate(['/']);
    }, (error: Error) => {
      console.log(error);
      this.signUpError = error.message;
    });
  }
}
