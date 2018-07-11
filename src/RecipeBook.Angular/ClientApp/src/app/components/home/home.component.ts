import { Component } from '@angular/core';
import { Router } from '../../../../node_modules/@angular/router';
import { AuthService } from '../../auth';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private router: Router, private authService: AuthService) {}

  signIn() {
    if (this.authService.isAuthorised()) {
      this.authService.signOut();
      this.router.navigate(['/']);
    } else {
      this.router.navigate(['/signin']);
    }
  }

  isAuthorized() {
    return this.authService.isAuthorised();
  }

  userFullName() {
    return this.authService.getUserFullName();
  }
}
