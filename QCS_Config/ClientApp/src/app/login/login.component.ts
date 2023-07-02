import { Component } from '@angular/core';
import { FormControl,FormGroup,Validators,FormBuilder, NgForm } from '@angular/forms';
import { LoginService } from '../Services/login.service';
import { ActivatedRoute, Router } from '@angular/router';
import { sha512 } from 'js-sha512';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private loginService: LoginService, private router: Router, private route: ActivatedRoute) {
  }


  submit(form: NgForm){
    console.log(form);
    if(form.valid==false){
      alert("نام کاربری و کلمه عبور را وارد نمایید");
      return;
    }
    this.loginService.login(form.value.userName, sha512.hmac("my HashKey", form.value.password)).subscribe((e:any) => {
      if (e != null) {
          this.loginService.setCookie('user', e.name, 100000);
          this.loginService.setCookie('username', e.username, 100000);
          this.loginService.setCookie('token', e.pass, 100000);
          this.loginService.setCookie('permission', e.permission.toString(), 100000);
          this.loginService.isLogin = true;
          this.router.navigate(['/']);
      } else {
        alert("نام کاربری یا کلمه عبور اشتباه می باشد");
      }}
      , (error:any) => {

        alert("نام کاربری یا کلمه عبور اشتباه می باشد");
      });
    
  }
}
