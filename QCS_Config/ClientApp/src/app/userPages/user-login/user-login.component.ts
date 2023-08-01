import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent {

constructor(public reqService:RequestService, private loginService: LoginService, private router: Router, private route: ActivatedRoute) {
 
  
}
onKeyPress(e:any) {
  var charCode = (e.which) ? e.which : e.keyCode;
  if (charCode > 31 && (charCode < 48 || charCode > 57)) {
    e.preventDefault();
  }
}

onSubmit(form: NgForm) {
  if (form.valid) {
    console.log(form); // نمایش آبجکت در کنسول
    let phone:string=form.value.phone_number;
    let nationalCode:string=form.value.national_code;
    
    if(this.validateIranianNationalCode(nationalCode)==false) {alert("کد ملی صحیحی وارد نمایید");return}


    this.loginService.login(nationalCode,phone).subscribe((e:any) => {
      if (e != null) {
          this.loginService.setCookie('user', e.name, 100000);
          this.loginService.setCookie('username', e.username, 100000);
          this.loginService.setCookie('token', e.pass, 100000);
          let userData=JSON.parse(e.comment);
          
          // this.reqService.name=userData.Name;
          this.loginService.setCookie('name', userData.Name, 100000);
          this.loginService.setCookie('gender', userData.Gender, 100000);
          this.loginService.setCookie('insuranceId', userData.InsuranceId, 100000);
          // this.loginService.setCookie('userData', userData, 100000);
          // this.loginService.setCookie('permission', e.permission.toString(), 100000);
          this.loginService.isLogin = true;
          this.router.navigate(['']);
      } else {
        alert("نام کاربری یا کلمه عبور اشتباه می باشد");
      }}
      , (error:any) => {
        alert("نام کاربری یا کلمه عبور اشتباه می باشد");
      });



  }else{
    let a:any=form.controls;
    if(a.phone_number.valid==false)  {alert("لطفا شماره تلفن همراه خواسته شده را درست وارد نمایید مثال: 09123456789") ;return}
    if(a.national_code.valid==false)  {alert("لطفا کد ملی خواسته شده را درست وارد نمایید") ;return}
  }
}



  validateIranianNationalCode(input:any) {
            if (!/^\d{10}$/.test(input))
                return false;
            let check = parseInt(input[9]);
            let sum = 0;
            let i;
            for (i = 0; i < 9; ++i) {
                sum += parseInt(input[i]) * (10 - i);
            }
            sum %= 11;
            return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
  }
}
