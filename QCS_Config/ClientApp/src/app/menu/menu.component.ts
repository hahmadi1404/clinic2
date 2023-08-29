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
                      routerLink: '/adminPages/Introduction'
                      // command: () => {
                      //     // this.update();
                      // }
                  },
                  {
                      label: 'معرفی خدمات',
                      routerLink: '/adminPages/Services'
                  },
                  {
                      label: 'معرفی شیفت ها',
                      routerLink: '/adminPages/Shifts'
                  },
                  {
                    label: 'معرفی پزشکان',
                    routerLink: '/adminPages/Doctors'
                  },
                  {
                    label: 'بیمه ها',
                    routerLink: '/adminPages/Insurance'
                  },
                  {
                    label: 'مدیریت رزرو وقت',
                    routerLink: '/adminPages/Reserve'
                  }
                  ,
                  {
                    label: 'مدیریت تصاویر گرافی',
                    routerLink: '/adminPages/Graphy'
                  }
                  ,
                  {
                    label: 'مدیریت صورتحساب',
                    routerLink: '/adminPages/Bill'
                  }
                  // ,
                  // {
                  //   label: 'انتقاد و پیشنهادات',
                  //   routerLink: '/pages/Reserve'
                  // }
                  ,
                  {
                    label: 'خروج از حساب کاربری',
                    routerLink: '/adminLogout'
                  }
              ]
          }
      ];
  }
}
