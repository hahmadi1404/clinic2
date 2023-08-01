import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class adminLoginService {

  isLogin= false;
  constructor(private router: Router,private http:HttpClient) { }

  checkLogin(){
    setInterval(()=>{
      if(this.isLogin) {
        const token = this.getCookie('adminToken');
        if (token == null || token == "") {
          this.isLogin = false;
          this.router.navigate(['adminLogin']);
        }
      }
    },60000)
    const promise =new Promise((resolve,reject)=>{
      const token=this.getCookie('adminToken');

      if(token==null || token==""){
        resolve(false);
      }
      else{
        this.http.get( './Authenticate/AuthenticateCheck' ,{headers:{"Authorization":`Bearer ${token}`}}).subscribe(e=>{

          resolve(true);
        },(err)=>{
          console.log(err);
          resolve(false);
        });
      }
    })
    return promise;
  }

  login( userName: string , password: string ) {
    let req:any={Username:userName, Password:password};
    // console.log(req);
    return this.http.post(( './Authenticate/adminAuthenticate') ,req);
  }
  ssoLogin( token: string  ) {
    return this.http.get(( './Authenticate/adminAuthenticateToken?token='+token) );
  }

  logout() {
    this.isLogin = false;
    this.deleteCookie('adminUser');
    this.deleteCookie('adminToken');
    this.router.navigate(['adminLogin']);
  }

  getVer():any{
    return this.http.get( './api/GetVer' ,{});
  }

  public getCookie(name: string) {
    let ca: Array<string> = document.cookie.split(';');
    let caLen: number = ca.length;
    let cookieName = `${name}=`;
    let c: string;

    for (let i: number = 0; i < caLen; i += 1) {
      c = ca[i].replace(/^\s+/g, '');
      if (c.indexOf(cookieName) == 0) {
        return c.substring(cookieName.length, c.length);
      }
    }
    return '';
  }

  public deleteCookie(name:string) {
    this.setCookie(name, '', -1);
  }
  public setCookie(name: string, value: string, expireDays: number, path: string = '') {
    let d:Date = new Date();
    d.setTime(d.getTime() + 23 * 60 * 60 * 1000);
    let expires:string = `expires=${d.toUTCString()}`;
    let cpath:string = path ? `; path=${path}` : '';
    document.cookie = `${name}=${value}; ${expires}${cpath}`;
  }
}
