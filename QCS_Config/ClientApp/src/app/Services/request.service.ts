import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {LoginService} from "./login.service";
import {Observable, EMPTY} from "rxjs";
import {EMPTY_OBSERVER} from "rxjs/internal/Subscriber";

@Injectable({
  providedIn: 'root'
})
export class RequestService {


  // dataSource: MatTableDataSource<DataResultModel> = new MatTableDataSource();

  constructor(private http: HttpClient, private loginService: LoginService) {
  }


  //Images
  AddImage(type:string,id:number,fileName:string, data:any) {
    return this.http.post(`./images/${type}/${id}/${fileName}`, data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
  }
  GetGalleryList(type:string,id:number) {
    return this.http.get(`./images/${type}/${id}`, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
  }
  AddGallery(type:string,id:number,data:any) {
    return this.http.post(`./images/${type}/${id}`, data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
  }
  RemoveImage(type:string,id:number,fileName:string) {
    return this.http.delete(`./images/${type}/${id}/${fileName}`,  {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
  }


//Intro
  GetClinicIntro() {
    return this.http.get('./api/Intro/'+this.loginService.getCookie('permission'), {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
  }
  UpdateClinicIntro(data:any) {
    return this.http.put('./api/Intro/'+this.loginService.getCookie('permission'), data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
  }


//Service
  GetServices() {
    return this.http.get('./api/Service', {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
  }
  AddService(data:any) {
    return this.http.post('./api/Service', data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
  }
  UpdateService(data:any) {
    return this.http.put('./api/Service/'+data.serviceId, data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
  }
  RemoveService(data:any) {
    return this.http.delete('./api/Service/'+data.serviceId, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
  }

  
//Doctors
GetDoctors() {
  return this.http.get('./api/Doctor', {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}
AddDoctor(data:any) {
  return this.http.post('./api/Doctor', data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}
UpdateDoctor(data:any) {
  return this.http.put('./api/Doctor/'+data.drId, data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}
RemoveDoctor(data:any) {
  return this.http.delete('./api/Doctor/'+data.drId, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}



//Reserve
GetReserves() {
  return this.http.get('./api/Reserve', {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}
UpdateReserve(data:any) {
  console.log(data);
  return this.http.put('./api/Reserve/'+data.reserveId, data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}
RemoveReserve(data:any) {
  return this.http.delete('./api/Reserve/'+data.reserveId, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}

//ReserveStatus
GetReserveStatus() {
  return this.http.get('./api/ReserveStatus', {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}


//Graphy
GetAllGraphy() {
  return this.http.get('./api/Graphy', {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}
UpdateGraphy(data:any) {
  return this.http.put('./api/Graphy/'+data.graphyId, data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}
RemoveGraphy(data:any) {
  return this.http.delete('./api/Graphy/'+data.graphyId, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}

//Bill
GetAllBill() {
  return this.http.get('./api/Bill', {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}
UpdateBill(data:any) {
  return this.http.put('./api/Bill/'+data.graphyId, data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}
RemoveBill(data:any) {
  return this.http.delete('./api/Bill/'+data.graphyId, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}

//Agreement
GetAgreement() {
  return this.http.get('./api/Agreement');
}


//Insurance
GetInsurance() {
  return this.http.get('./api/Insurance');
}


//Patient
GetAllPatient() {
  return this.http.get('./api/Patient', {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
}
AddPatient(data:any) {
  return this.http.post('./api/Patient', data);
}
// UpdatePatient(data:any) {
//   return this.http.put('./api/Patient/'+data.Id, data, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
// }
// RemovePatient(data:any) {
//   return this.http.delete('./api/Patient/'+data.Id, {headers: {'Authorization': `Bearer ${this.loginService.getCookie('token')}`}});
// }



}
