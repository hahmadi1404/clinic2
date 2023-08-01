import { Component } from '@angular/core';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-user-main-page',
  templateUrl: './user-main-page.component.html',
  styleUrls: ['./user-main-page.component.css']
})
export class UserMainPageComponent {

  constructor(public  reqService:RequestService) {
    reqService.GetInsurance().subscribe((data:any)=>{
      console.log(data);
      reqService.Insurances==data;   
    });

  }
  visibleSidebar=false;
}
