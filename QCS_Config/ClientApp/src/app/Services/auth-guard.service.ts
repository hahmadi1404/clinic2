import { Injectable } from '@angular/core';
import {LoginService} from "./login.service";
import {CanActivate, Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate{

  constructor( private loginService: LoginService, private router: Router) { }

  canActivate(_route: import('@angular/router').ActivatedRouteSnapshot, _state: import('@angular/router').RouterStateSnapshot): boolean | import('@angular/router').UrlTree | import('rxjs').Observable<boolean | import('@angular/router').UrlTree> | Promise<boolean | import('@angular/router').UrlTree> {
    return this.loginService.checkLogin().then(e=>{

      if(e==true) {
        return true;
      } else {
        this.router.navigate(['login']);
        return false;
      }
    });

  }
}
