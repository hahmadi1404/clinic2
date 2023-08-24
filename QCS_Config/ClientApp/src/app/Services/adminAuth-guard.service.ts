import { Injectable } from '@angular/core';
import {LoginService} from "./login.service";
import {CanActivate, Router} from "@angular/router";
import { adminLoginService } from './adminLogin.service';

@Injectable({
  providedIn: 'root'
})
export class adminAuthGuard implements CanActivate{

  constructor( private loginService: adminLoginService, private router: Router) { }

  canActivate(_route: import('@angular/router').ActivatedRouteSnapshot, _state: import('@angular/router').RouterStateSnapshot): boolean | import('@angular/router').UrlTree | import('rxjs').Observable<boolean | import('@angular/router').UrlTree> | Promise<boolean | import('@angular/router').UrlTree> {
    return this.loginService.checkLogin().then(e=>{

      if(e==true) {
        return true;
      } else {
        this.router.navigate(['adminLogin']);
        return false;
      }
    });

  }
}
