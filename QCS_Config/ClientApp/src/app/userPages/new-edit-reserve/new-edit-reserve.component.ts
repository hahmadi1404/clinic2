import { AfterViewInit, Component } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Jalali } from 'jalali-ts';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-new-edit-reserve',
  templateUrl: './new-edit-reserve.component.html',
  styleUrls: ['./new-edit-reserve.component.css']
})
export class NewEditReserveComponent implements AfterViewInit {
  
  action="new";

  formData: any = {};
  departments: string[] = ['بخش اول', 'بخش دوم', 'بخش سوم'];
  shifts: string[] = ['شیفت صبح', 'شیفت عصر', 'شیفت شب'];
  doctors: string[] = [];
  constructor(public reqService:RequestService,private loginService:LoginService,private router: Router, private route: ActivatedRoute) {

    
  }
  ngAfterViewInit(): void {
    console.log("this.reqService.shifts");
    console.log(this.reqService.shifts);
    console.log("this.reqService.departments");
    console.log(this.reqService.departments);
    this.dateValue=this.myDate.value;
    this.formData.department=1;
    this.formData.shift=1;

  }
  firstDateLoading=false;
  getFirstDate(){
    this.firstDateLoading=true;
    this.reqService.GetFirstDate(this.formData.shift,this.reqService.myInsuranceId).subscribe(a=>{
      console.log(a);
      console.log(this.myDate);
      this.myDate.value=a;
      this.dateValue=a.toString();
      this.firstDateLoading=false;
    });
  }
  SendLoading=false;
  submitForm(form:NgForm) {
    this.SendLoading=true;
    if (form.valid) {
      console.log(form); // نمایش آبجکت در کنسول
      let data={
        "mobile": this.reqService.myPhoneNumber,
        "nationalCode": this.reqService.myNationalCode,
        "dosierIdReq": this.reqService.myDosierId,
        "fullNameReq": this.reqService.myName,
        "nationalCodeReq": this.reqService.myNationalCode,
        "mobileReq": this.reqService.myPhoneNumber,
        "genderReq": this.reqService.myGender,
        // "ageReq": 0,
        "reserveDatePersian": this.dateValue,
        // "status": "string",
        "shiftId":this.formData.shift,
        // "drId": 0,
        "insuranceId": this.reqService.myInsuranceId,
        "sectionId": this.formData.department,
        "createDatePersian": Jalali.now().format('YYYY/MM/DD'),
      };

    
// this.reqService.AddReserve()
      this.reqService.AddReserve(data).subscribe((e:any) => {
        // if (e != null) {
           console.log(e);
        // } else {
          this.SendLoading=false;
          alert("درخواست شما به دررستی ثبت شد");
          this.router.navigate(['/pages/reserve']);
        // }
      }
        , (error:any) => {
          this.SendLoading=false;
          console.log(error);
          alert(error.error.detail);
        });
  
  
  
    }else{
      this.SendLoading=false;
      alert("لطفا مقادیر خواسته شده را درست وارد نمایید") ;return
    }
    
  }
  // dateValue = new FormControl();
  goBack() {
    window.history.back();
  }
  myDate:any= new FormControl();
  dateValue="";
  onDateChange(e:any){
console.log(e)
this.dateValue=e.shamsi;
  }

}
