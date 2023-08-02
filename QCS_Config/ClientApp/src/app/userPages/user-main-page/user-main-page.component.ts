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
