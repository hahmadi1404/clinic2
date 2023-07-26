import { Component,OnInit  } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  items!: MenuItem[];

  constructor() {}
  
  ngOnInit() {
      this.items = [
          {
              label: 'منو',
              items: [
                  {
                      label: 'معرفی مجموعه',
                      // icon: 'pi pi-refresh',
                      routerLink: '/pages/Introduction'
                      // command: () => {
                      //     // this.update();
                      // }
                  },
                  {
                      label: 'معرفی خدمات',
                      routerLink: '/pages/Services'
                  },
                  {
                    label: 'معرفی پزشکان',
                    routerLink: '/pages/Doctors'
                  },
                  {
                    label: 'مدیریت رزرو وقت',
                    routerLink: '/pages/Reserve'
                  }
                  ,
                  {
                    label: 'مدیریت تصاویر گرافی',
                    routerLink: '/pages/Graphy'
                  }
                  ,
                  {
                    label: 'مدیریت صورتحساب',
                    routerLink: '/pages/Bill'
                  }
                  // ,
                  // {
                  //   label: 'انتقاد و پیشنهادات',
                  //   routerLink: '/pages/Reserve'
                  // }
              ]
          }
      ];
  }
}
