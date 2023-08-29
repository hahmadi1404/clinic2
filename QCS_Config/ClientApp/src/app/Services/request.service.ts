import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {LoginService} from "./login.service";
import {Observable, EMPTY} from "rxjs";
import {EMPTY_OBSERVER} from "rxjs/internal/Subscriber";

@Injectable({
  providedIn: 'root'
})
export class RequestService {
  myGender=-1;
  myName:string="";
  myClinicId:number=-1;
  myNationalCode:string="";
  myPhoneNumber:string="";
  myDosierId:string="";
  // dataSource: MatTableDataSource<DataResultModel> = new MatTableDataSource();

  constructor(private http: HttpClient, private loginService: LoginService) {
  }
  token="";

  //Images
  AddImage(type:string,id:number,fileName:string, data:any) {
    return this.http.post(`./images/${type}/${id}/${fileName}`, data, {headers: {'Authorization': `Bearer ${this.token}`}});
  }
  GetGalleryList(type:string,id:number) {
    return this.http.get(`./images/${type}/${id}`, {headers: {'Authorization': `Bearer ${this.token}`}});
  }
  AddGallery(type:string,id:number,data:any) {
    return this.http.post(`./images/${type}/${id}`, data, {headers: {'Authorization': `Bearer ${this.token}`}});
  }
  RemoveImage(type:string,id:number,fileName:string) {
    return this.http.delete(`./images/${type}/${id}/${fileName}`,  {headers: {'Authorization': `Bearer ${this.token}`}});
  }


//Intro
  GetClinicIntro() {
    return this.http.get('./api/Intro/'+this.loginService.getCookie('permission'), {headers: {'Authorization': `Bearer ${this.token}`}});
  }
  UpdateClinicIntro(data:any) {
    return this.http.put('./api/Intro/'+this.loginService.getCookie('permission'), data, {headers: {'Authorization': `Bearer ${this.token}`}});
  }


//Service
  GetServices() {
    return this.http.get('./api/Service', {headers: {'Authorization': `Bearer ${this.token}`}});
  }
  AddService(data:any) {
    return this.http.post('./api/Service', data, {headers: {'Authorization': `Bearer ${this.token}`}});
  }
  UpdateService(data:any) {
    return this.http.put('./api/Service/'+data.serviceId, data, {headers: {'Authorization': `Bearer ${this.token}`}});
  }
  RemoveService(data:any) {
    return this.http.delete('./api/Service/'+data.serviceId, {headers: {'Authorization': `Bearer ${this.token}`}});
  }

  
//Doctors
GetDoctors() {
  return this.http.get('./api/Doctor', {headers: {'Authorization': `Bearer ${this.token}`}});
}
AddDoctor(data:any) {
  return this.http.post('./api/Doctor', data, {headers: {'Authorization': `Bearer ${this.token}`}});
}
UpdateDoctor(data:any) {
  return this.http.put('./api/Doctor/'+data.drId, data, {headers: {'Authorization': `Bearer ${this.token}`}});
}
RemoveDoctor(data:any) {
  return this.http.delete('./api/Doctor/'+data.drId, {headers: {'Authorization': `Bearer ${this.token}`}});
}



//Reserve
GetReserves() {
  return this.http.get('./api/Reserve', {headers: {'Authorization': `Bearer ${this.token}`}});
}

AddReserve(data:any) {
  console.log(data);
  return this.http.post('./api/Reserve', data, {headers: {'Authorization': `Bearer ${this.token}`}});
}

UpdateReserve(data:any) {
  console.log(data);
  return this.http.put('./api/Reserve/'+data.reserveId, data, {headers: {'Authorization': `Bearer ${this.token}`}});
}
RemoveReserve(data:any) {
  return this.http.delete('./api/Reserve/'+data.reserveId, {headers: {'Authorization': `Bearer ${this.token}`}});
}

//ReserveStatus
GetReserveStatus() {
  return this.http.get('./api/ReserveStatus', {headers: {'Authorization': `Bearer ${this.token}`}});
}


//Graphy
GetAllGraphy() {
  return this.http.get('./api/Graphy', {headers: {'Authorization': `Bearer ${this.token}`}});
}
UpdateGraphy(data:any) {
  return this.http.put('./api/Graphy/'+data.graphyId, data, {headers: {'Authorization': `Bearer ${this.token}`}});
}
RemoveGraphy(data:any) {
  return this.http.delete('./api/Graphy/'+data.graphyId, {headers: {'Authorization': `Bearer ${this.token}`}});
}

//Bill
GetAllBill() {
  return this.http.get('./api/Bill', {headers: {'Authorization': `Bearer ${this.token}`}});
}
UpdateBill(data:any) {
  return this.http.put('./api/Bill/'+data.graphyId, data, {headers: {'Authorization': `Bearer ${this.token}`}});
}
RemoveBill(data:any) {
  return this.http.delete('./api/Bill/'+data.graphyId, {headers: {'Authorization': `Bearer ${this.token}`}});
}

//Agreement
GetAgreement() {
  return this.http.get('./api/Agreement');
}


//Insurance
Insurances:any=null;
myInsurance:string[]=[];
myInsuranceId=-1;
GetInsurance() {
  return this.http.get('./api/Insurance');
}
AddInsurance(data:any) {
  console.log(data);
  return this.http.post('./api/Insurance', data, {headers: {'Authorization': `Bearer ${this.token}`}});
}
UpdateInsurance(data:any) {
  return this.http.put('./api/Insurance/'+data.id, data, {headers: {'Authorization': `Bearer ${this.token}`}});
}
RemoveInsurance(data:any) {
  return this.http.delete('./api/Insurance/'+data.id, {headers: {'Authorization': `Bearer ${this.token}`}});
}



//Department
departments:any=null;
GetDepartment() {
  return this.http.get('./api/Department');
}



//Shift
shifts:any=null;
GetShift() {
  return this.http.get('./api/Shift');
}
AddShift(data:any) {
  console.log(data);
  return this.http.post('./api/Shift', data, {headers: {'Authorization': `Bearer ${this.token}`}});
}
UpdateShift(data:any) {
  return this.http.put('./api/Shift/'+data.id, data, {headers: {'Authorization': `Bearer ${this.token}`}});
}
RemoveShift(data:any) {
  return this.http.delete('./api/Shift/'+data.id, {headers: {'Authorization': `Bearer ${this.token}`}});
}



//Patient
GetAllPatient() {
  return this.http.get('./api/Patient', {headers: {'Authorization': `Bearer ${this.token}`}});
}
AddPatient(data:any) {
  return this.http.post('./api/Patient', data);
}
// UpdatePatient(data:any) {
//   return this.http.put('./api/Patient/'+data.Id, data, {headers: {'Authorization': `Bearer ${this.token}`}});
// }
// RemovePatient(data:any) {
//   return this.http.delete('./api/Patient/'+data.Id, {headers: {'Authorization': `Bearer ${this.token}`}});
// }

GetFirstDate(shiftId:any,insuranceId:any){
  return this.http.get(`./Action/GetFirstReserveDate?shiftId=${shiftId}&insuranceId=${insuranceId}`, {headers: {'Authorization': `Bearer ${this.token}`}});
}

}
