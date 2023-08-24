import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ButtonModule } from 'primeng/button';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { MenuModule } from 'primeng/menu';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomePageComponent } from './Pages/home-page/home-page.component';
import { LoginComponent } from './login/login.component';
import { MainPageComponent } from './main-page/main-page.component';
import { CardModule } from 'primeng/card';
import { IntroductionPageComponent } from './Pages/introduction-page/introduction-page.component';
import { ServicesPageComponent } from './Pages/services-page/services-page.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import { LogoutComponent } from './login/logout/logout.component';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { TabViewModule } from 'primeng/tabview';
import { ImageModule } from 'primeng/image';
import { ContextMenuModule } from 'primeng/contextmenu';
import { TableModule } from 'primeng/table';
import { ServiceDialogComponent } from './Pages/services-page/service-dialog/service-dialog.component';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { DoctorsComponent } from './Pages/doctors/doctors.component';
import { ReserveComponent } from './Pages/reserve/reserve.component';

import { SelectButtonModule } from 'primeng/selectbutton';
import { GraphyComponent } from './Pages/graphy/graphy.component';
import { BillComponent } from './bill/bill.component';
import { TagModule } from 'primeng/tag';
import { MultiSelectModule } from 'primeng/multiselect';
import { DropdownModule } from 'primeng/dropdown';
import { UserLoginComponent } from './userPages/user-login/user-login.component';
import { RegisterComponent } from './userPages/register/register.component';
import { MyReserveComponent } from './userPages/my-reserve/my-reserve.component';
import { UserMainPageComponent } from './userPages/user-main-page/user-main-page.component';
import { SidebarModule } from 'primeng/sidebar';
import { MyHomePageComponent } from './userPages/my-home-page/my-home-page.component';
import { NewEditReserveComponent } from './userPages/new-edit-reserve/new-edit-reserve.component';
import { LoginService } from './Services/login.service';
import { RequestService } from './Services/request.service';

import { NgPersianDatepickerModule } from 'ng-persian-datepicker';
import { UserLogoutComponent } from './userPages/user-login/logout/user-logout.component';

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    HomePageComponent,
    LoginComponent,
    MainPageComponent,
    IntroductionPageComponent,
    ServicesPageComponent,
    LogoutComponent,
    ServiceDialogComponent,
    DoctorsComponent,
    ReserveComponent,
    GraphyComponent,
    BillComponent,
    UserLoginComponent,
    RegisterComponent,
    MyReserveComponent,
    UserMainPageComponent,
    MyHomePageComponent,
    NewEditReserveComponent,
    UserLogoutComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ButtonModule ,
    MenuModule,
    ToastModule,
    BrowserAnimationsModule,
    CardModule,
    FormsModule,
    HttpClientModule,
    InputTextareaModule,
    TabViewModule,
    ImageModule,
    ContextMenuModule,
    TableModule,
    DialogModule,
    InputTextModule,
    ConfirmPopupModule,
    SelectButtonModule,
    TagModule,
    MultiSelectModule,
    DropdownModule,
    SidebarModule,
    NgPersianDatepickerModule
  ],
  providers: [MessageService,ConfirmationService,LoginService,RequestService],
  bootstrap: [AppComponent]
})
export class AppModule { }
