import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  constructor(private router: Router,private reqService:RequestService) {
    reqService.GetInsurance().subscribe((data:any)=>{
      this.insurances=data;
      console.log(data);
    });
    reqService.GetAgreement().subscribe((data:any)=>{
      this.terms=data[0].text;
      console.log(data);
    });

  }
  insurances:any=[{insuranceName:"بیمه 1", id:0}];
  termsConditions=false;
  terms="";
  agree=false;
  setAgree(){
    this.termsConditions=true;

  }
  onKeyPress(e:any) {
    var charCode = (e.which) ? e.which : e.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      e.preventDefault();
    }
  }


  formData: any = {}; // آبجکت برای ذخیره مقادیر فرم

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.formData.dosierID = form.value.DosierID;      
      this.formData.name = form.value.name;
      this.formData.lastName = form.value.last_name;
      this.formData.nationalCode = form.value.national_code;
      this.formData.phoneNumber = form.value.phone_number;
      this.formData.insuranceId =Number( form.value.insurance);
      this.formData.gender = Number(form.value.gender);

      console.log(this.formData); // نمایش آبجکت در کنسول
      console.log(form.value); // نمایش آبجکت در کنسول

      this.reqService.AddPatient(this.formData).subscribe(a=>{
        this.router.navigate(['login']);
      })
    }else{
      alert("لطفا اطلاعات خواسته شده را درست وارد نمایید") ;
    }
  }
}
