import {
  throwError as observableThrowError,
  Observable,
  BehaviorSubject
} from "rxjs";
import { Injectable, Injector } from "@angular/core";
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse,
  HttpSentEvent,
  HttpHeaderResponse,
  HttpProgressEvent,
  HttpResponse,
  HttpUserEvent
} from "@angular/common/http";
import "rxjs/add/operator/catch";
import { AuthService } from "../auth-service.service";

@Injectable()
export class HttpAuthInterceptor implements HttpInterceptor {
  constructor(private inject: Injector) {}

  /**
   *
   * @param request http request
   * @param next http request handler
   */
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<
    | any
    | HttpSentEvent
    | HttpHeaderResponse
    | HttpProgressEvent
    | HttpResponse<any>
    | HttpUserEvent<any>
  > {
    return next.handle(this.addToken(request));
  }

  private addToken(request: HttpRequest<any>): HttpRequest<any> {
    try {
      if (this.inject.get(AuthService).isAuthorised()) {
        const token = localStorage.getItem("access_token");
        if (token) {
          request = this.addTokenHeader(request, token);
        }
      }
    } catch (error) {
    } finally {
      return request;
    }
  }

  private addTokenHeader(
    request: HttpRequest<any>,
    token: string
  ): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
}
