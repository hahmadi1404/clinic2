import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-my-home-page',
  templateUrl: './my-home-page.component.html',
  styleUrls: ['./my-home-page.component.css']
})

export class MyHomePageComponent implements OnInit {

  /**
   *
   */
  userData:any=null;
  constructor(public reqService:RequestService, public loginService: LoginService) {
  

  }
  ngOnInit(): void {
    // if(this.userData==null) this.userData=JSON.parse() ;
    this.gender=Number(this.loginService.getCookie('gender'));
    this.name=this.loginService.getCookie('name');
  }
  gender=0;
  name=""
}
