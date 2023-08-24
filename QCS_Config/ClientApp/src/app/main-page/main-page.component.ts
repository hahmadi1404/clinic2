import { Component } from '@angular/core';
import { RequestService } from '../Services/request.service';
import { LoginService } from '../Services/login.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {
  
  i=3;
  constructor(public  reqService:RequestService,private loginService:LoginService) {
    
    this.reqService.token=this.loginService.getCookie('adminToken');
    reqService.GetInsurance().subscribe((data:any)=>{
      this.i--;
      console.log(data);
      reqService.Insurances=data;   
  
    });

    reqService.GetDepartment().subscribe((data:any)=>{
      this.i--;
      console.log(data);
      reqService.departments=data;   
      console.log(reqService.departments);
    });
    reqService.GetShift().subscribe((data:any)=>{
      this.i--;
      console.log(data);
      reqService.shifts=data;   
      console.log(reqService.shifts);
  });
  }

}
