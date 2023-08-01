import { Component } from '@angular/core';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-new-edit-reserve',
  templateUrl: './new-edit-reserve.component.html',
  styleUrls: ['./new-edit-reserve.component.css']
})
export class NewEditReserveComponent {

  action="new";

  formData: any = {};
  departments: string[] = ['بخش اول', 'بخش دوم', 'بخش سوم'];
  shifts: string[] = ['شیفت صبح', 'شیفت عصر', 'شیفت شب'];
  doctors: string[] = [];
  insurances: string[] =[];

  myInsurance=-1;
  constructor(private reqService:RequestService,private loginService:LoginService) {
    this.formData.insurance=-1;

      this.myInsurance=Number(this.loginService.getCookie('insuranceId'));
      console.log(this.myInsurance);
      console.log(this.myInsurance);
    this.insurances.push(data.filter((f:any)=>f.id==Number(this.myInsurance))[0].insuranceName);
    
  }

  submitForm() {
    // اجرای عملیات ارسال درخواست
  }

  cancelForm() {
    // تنظیم مقادیر به عدم انتخاب و لغو فرم
    this.formData = {};
  }
}
