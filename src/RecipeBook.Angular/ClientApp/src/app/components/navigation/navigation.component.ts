import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '../../../../node_modules/@angular/router';
import { AuthService } from '../../auth';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html'
})
export class NavigationComponent {
  constructor(private router: Router, private route: ActivatedRoute, private authService: AuthService) {}

  signInOrOut() {
    console.log(this.authService.isAuthorised());
    if (this.authService.isAuthorised()) {
      this.authService.signOut();
      this.router.navigate(['/']);
    } else {
      this.router.navigate(['../signin'],  {relativeTo: this.route});
    }
  }

  isAuthorized() {
    return this.authService.isAuthorised();
  }

  userFullName() {
    return this.authService.getUserFullName();
  }
}
