import { Injectable } from '@angular/core';
import { HttpClient } from '../../../node_modules/@angular/common/http';
import { environment } from '../../environments/environment';
import { SignIn, SignUp } from './models';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenString: string;
  private userInfo: any;
  private helper = new JwtHelperService();

  constructor(private http: HttpClient) {}

  isAuthorised(): boolean {
    if (!this.tokenString) {
      this.tokenString = localStorage.getItem('access_token');
      if (this.tokenString) {
        const expirationDate = this.helper.getTokenExpirationDate(this.tokenString);
        if (expirationDate < new Date()) {
          localStorage.removeItem('access_token');
          this.tokenString = null;
          return false;
        } else {
          this.getUserInfo(this.tokenString);
          return true;
        }
      }

      return false;
    } else {
      return this.tokenString.length > 0;
    }
  }

  getUserFullName(): string {
    if (!this.tokenString) {
      return '';
    } else {
      return this.userInfo.fullName;
    }
  }

  getUserId(): string {
    if (!this.tokenString) {
      return '';
    } else {
      return this.userInfo.userId;
    }
  }

  getToken(): string {
    return this.tokenString;
  }

  authenticate(signIn: SignIn): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http
        .post(`${environment.apiLocation}/auth/session/authorize`, signIn)
        .toPromise()
        .then(
          (response: any) => {
            this.tokenString = response.token;
            localStorage.setItem('access_token', this.tokenString);
            this.getUserInfo(this.tokenString);
            resolve();
          },
          error => {
            reject(error);
          }
        );
    });
  }

  signUp(signUp: SignUp): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http
        .post(`${environment.apiLocation}/auth/session/signup`, signUp)
        .toPromise()
        .then(
          (response: any) => {
            this.tokenString = response.token;
            localStorage.setItem('access_token', this.tokenString);
            this.getUserInfo(this.tokenString);
            resolve();
          },
          error => {
            reject(error);
          }
        );
    });
  }

  signOut() {
    this.tokenString = '';
    localStorage.removeItem('access_token');
  }

  private getUserInfo(token: string) {
    const decodedToken = this.helper.decodeToken(token);

    this.userInfo = {
      userId: null,
      username: null,
      fullName: null
    };

    Object.keys(decodedToken).forEach((claim, idx) => {
      if (claim.toString().indexOf('sid') !== -1) {
        this.userInfo.userId = decodedToken[claim];
      }
      if (claim.toString().indexOf('name') !== -1) {
        this.userInfo.username = decodedToken[claim];
      }
      if (claim.toString().indexOf('givenname') !== -1) {
        this.userInfo.fullName = decodedToken[claim];
      }
    });
  }
}
