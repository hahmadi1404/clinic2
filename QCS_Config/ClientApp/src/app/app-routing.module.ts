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
import { UserLoginComponent } from './userPages/user-login/user-login.component';
import { RegisterComponent } from './userPages/register/register.component';
import { UserMainPageComponent } from './userPages/user-main-page/user-main-page.component';
import { MyHomePageComponent } from './userPages/my-home-page/my-home-page.component';
import { MyReserveComponent } from './userPages/my-reserve/my-reserve.component';
import { NewEditReserveComponent } from './userPages/new-edit-reserve/new-edit-reserve.component';
import { adminAuthGuard } from './Services/adminAuth-guard.service';
import { UserLogoutComponent } from './userPages/user-login/logout/user-logout.component';
import { ShiftComponent } from './Pages/shift/shift.component';
import { InsuranceComponent } from './Pages/insurance/insurance.component';

const routes: Routes = [
  { path: 'adminLogout',  component: LogoutComponent },
  {path:'adminLogin',component:LoginComponent},
  {path:'',redirectTo:'/pages/home',pathMatch:'full'},
  { path: 'adminPages', component: MainPageComponent ,children:[
    {path:'',component: HomePageComponent},
    {path:'Introduction',component: IntroductionPageComponent},
    {path:'Services',component: ServicesPageComponent},
    {path:'Shifts',component: ShiftComponent},
    {path:'Doctors',component: DoctorsComponent},
    {path:'Insurance',component: InsuranceComponent},
    {path:'Reserve',component: ReserveComponent},
    {path:'Graphy',component: GraphyComponent},
    {path:'Bill',component: BillComponent},
  ] ,canActivate: [adminAuthGuard]},
  { path: 'pages', component: UserMainPageComponent ,children:[
    {path:'home',component: MyHomePageComponent},
    {path:'Introduction',component: IntroductionPageComponent},
    {path:'Services',component: ServicesPageComponent},
    {path:'Doctors',component: DoctorsComponent},
    {path:'reserve/new',component: NewEditReserveComponent},
    {path:'reserve/edit',component: NewEditReserveComponent},
    {path:'reserve',component: MyReserveComponent},
    {path:'Graphy',component: GraphyComponent},
    {path:'Bill',component: BillComponent},

   ] ,canActivate: [AuthGuard]},
  {path: 'logout',  component: UserLogoutComponent },
  {path:'login',component:UserLoginComponent},
  {path:'register',component:RegisterComponent}
  // {path:'login',component:LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
