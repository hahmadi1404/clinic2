import { Component } from '@angular/core';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-user-main-page',
  templateUrl: './user-main-page.component.html',
  styleUrls: ['./user-main-page.component.css']
})
export class UserMainPageComponent {

  constructor(public  reqService:RequestService,private loginService:LoginService) {
    this.reqService.myGender=Number(this.loginService.getCookie('gender'));
    this.reqService.myName=this.loginService.getCookie('name');
    this.reqService.myClinicId=Number(this.loginService.getCookie('permission'));
    this.reqService.myNationalCode=this.loginService.getCookie('nationalCode');
    this.reqService.myPhoneNumber=this.loginService.getCookie('phoneNumber');
    this.reqService.myDosierId=this.loginService.getCookie('dosierId');

    reqService.GetInsurance().subscribe((data:any)=>{
      console.log(data);
      let myInsurance=Number(this.loginService.getCookie('insuranceId'));
      reqService.myInsuranceId=myInsurance;
      reqService.Insurances==data;   
      reqService.myInsurance.push(data.filter((f:any)=>f.id==Number(myInsurance))[0].insuranceName)
    });

    reqService.GetDepartment().subscribe((data:any)=>{
      console.log(data);
      reqService.departments=data;   
      console.log(reqService.departments);
    });
    reqService.GetShift().subscribe((data:any)=>{
      console.log(data);
      reqService.shifts=data;   
      console.log(reqService.shifts);
    });

  }
  visibleSidebar=false;
}
