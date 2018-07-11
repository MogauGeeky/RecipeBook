import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '../../../../node_modules/@angular/forms';
import { Router } from '../../../../node_modules/@angular/router';
import { AuthService } from '../../auth';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html'
})
export class SignInComponent implements OnInit {
  signInForm: FormGroup;
  signInError: string;

  constructor(private router: Router, private authService: AuthService, private fb: FormBuilder) { }

  ngOnInit() {
    if (this.authService.isAuthorised()) {
      this.router.navigate(['/']);
    }

    this.signInForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  signIn() {
    this.signInError = null;
    this.authService.authenticate(this.signInForm.value).then(() => {
      this.router.navigate(['/']);
    }, (error: Error) => {
      this.signInError = error.message;
    });
  }

}
