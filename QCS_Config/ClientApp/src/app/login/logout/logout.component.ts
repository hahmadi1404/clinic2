import { Component, OnInit } from '@angular/core';
import { adminLoginService } from 'src/app/Services/adminLogin.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {
  constructor(private loginService: adminLoginService) { }

  ngOnInit() {
    this.loginService.logout();
  }
}
