import { AfterViewInit, Component } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Jalali } from 'jalali-ts';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-new-edit-reserve',
  templateUrl: './new-edit-reserve.component.html',
  styleUrls: ['./new-edit-reserve.component.css']
})
export class NewEditReserveComponent implements AfterViewInit {
  public dateValue = new FormControl();

  action="new";

  formData: any = {};
  departments: string[] = ['بخش اول', 'بخش دوم', 'بخش سوم'];
  shifts: string[] = ['شیفت صبح', 'شیفت عصر', 'شیفت شب'];
  doctors: string[] = [];
  constructor(public reqService:RequestService,private loginService:LoginService,private router: Router, private route: ActivatedRoute) {

    
  }
  ngAfterViewInit(): void {
    console.log("this.reqService.shifts");
    console.log(this.reqService.shifts);
    console.log("this.reqService.departments");
    console.log(this.reqService.departments);
  }

  submitForm(form:NgForm) {
    if (form.valid) {
      console.log(form); // نمایش آبجکت در کنسول
      let phone:string=form.value.phone_number;
      let nationalCode:string=form.value.national_code;
// this.reqService.AddReserve()
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

      alert("لطفا مقادیر خواسته شده را درست وارد نمایید") ;return
    }
  }
 

  onDateChange(e:any){
console.log(e)
// this.dateValue1=e.shamsi;
  }
  cancelForm() {
    this.formData = {};
  }
}
