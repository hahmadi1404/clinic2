import { Component } from '@angular/core';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-my-reserve',
  templateUrl: './my-reserve.component.html',
  styleUrls: ['./my-reserve.component.css']
})
export class MyReserveComponent {
  reserves:any=[];

  constructor(public  reqService:RequestService,private loginService:LoginService) {

    
    reqService.GetReserves().subscribe((data:any)=>{
      console.log(this.reqService.departments.filter((f:any)=>f.id==1)[0]?.departmentName);
      data.forEach((element:any) => {
        console.log(element.sectionId);
        console.log(this.reqService.departments.filter((f:any)=>f.id==element.sectionId));
        element.department=this.reqService.departments.filter((f:any)=>f.id==element.sectionId)[0]?.departmentName;
        element.shift=this.reqService.shifts.filter((f:any)=>f.id==element.shiftId)[0]?.shiftName;
        // element.doctor=this.reqService.doctorS.filter((f:any)=>f.id==element.drId)[0]?.DrName;
        this.reserves.push(element);
        console.log(element);
      });
      // this.reserves=data;

    });

  }
}
