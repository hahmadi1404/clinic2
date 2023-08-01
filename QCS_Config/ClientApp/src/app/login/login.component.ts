import { Component } from '@angular/core';
import { FormControl,FormGroup,Validators,FormBuilder, NgForm } from '@angular/forms';
import { LoginService } from '../Services/login.service';
import { ActivatedRoute, Router } from '@angular/router';
import { sha512 } from 'js-sha512';
import { adminLoginService } from '../Services/adminLogin.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private loginService: adminLoginService, private router: Router, private route: ActivatedRoute) {
  }


  submit(form: NgForm){
    console.log(form);
    if(form.valid==false){
      alert("نام کاربری و کلمه عبور را وارد نمایید");
      return;
    }
    this.loginService.login(form.value.userName, sha512.hmac("my HashKey", form.value.password)).subscribe((e:any) => {
      if (e != null) {
          this.loginService.setCookie('adminUser', e.name, 100000);
          this.loginService.setCookie('adminUsername', e.username, 100000);
          this.loginService.setCookie('adminToken', e.pass, 100000);
          this.loginService.setCookie('adminPermission', e.permission.toString(), 100000);
          this.loginService.isLogin = true;
          this.router.navigate(['/adminPages/home']);
      } else {
        alert("نام کاربری یا کلمه عبور اشتباه می باشد");
      }}
      , (error:any) => {

        alert("نام کاربری یا کلمه عبور اشتباه می باشد");
      });
    
  }
}
