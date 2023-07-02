import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MenuComponent } from './menu/menu.component';
import { AppComponent } from './app.component';
import { HomePageComponent } from './Pages/home-page/home-page.component';
import { LoginComponent } from './login/login.component';
import { MainPageComponent } from './main-page/main-page.component';
import { IntroductionPageComponent } from './Pages/introduction-page/introduction-page.component';
import { ServicesPageComponent } from './Pages/services-page/services-page.component';
import { AuthGuard } from './Services/auth-guard.service';
import { LogoutComponent } from './login/logout/logout.component';
import { DoctorsComponent } from './Pages/doctors/doctors.component';
import { ReserveComponent } from './Pages/reserve/reserve.component';
import { GraphyComponent } from './Pages/graphy/graphy.component';
import { BillComponent } from './bill/bill.component';

const routes: Routes = [
  { path: 'logout',  component: LogoutComponent },
  {path:'',redirectTo:'/pages',pathMatch:'full'},
  { path: 'pages', component: MainPageComponent ,children:[
    {path:'',component: HomePageComponent},
    {path:'Introduction',component: IntroductionPageComponent},
    {path:'Services',component: ServicesPageComponent},
    {path:'Doctors',component: DoctorsComponent},
    {path:'Reserve',component: ReserveComponent},
    {path:'Graphy',component: GraphyComponent},
    {path:'Bill',component: BillComponent},
  ] ,canActivate: [AuthGuard]},
  {path:'login',component:LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
