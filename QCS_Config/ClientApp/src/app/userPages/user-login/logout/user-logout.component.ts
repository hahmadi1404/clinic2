import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/Services/login.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class UserLogoutComponent implements OnInit {
  constructor(private loginService: LoginService) { }

  ngOnInit() {
    this.loginService.logout();
  }
}
