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
      this.selectInsurance=data[0].id;
      console.log(data);
    });
    reqService.GetAgreement().subscribe((data:any)=>{
      this.terms=data[0].text;
      console.log(data);
    });

  }
  insurances:any=[];
  termsConditions=false;
  selectInsurance:any;
  terms="";
  agree=false;
  setAgree(){
    this.termsConditions=true;

  }

  convertNumber(number:any) {
    // Convert the number to a string.
    var stringNumber = number.toString();
  
    // Replace all the Persian digits with English digits.
    stringNumber = stringNumber.replace(/۰/g, "0");
    stringNumber = stringNumber.replace(/۱/g, "1");
    stringNumber = stringNumber.replace(/۲/g, "2");
    stringNumber = stringNumber.replace(/۳/g, "3");
    stringNumber = stringNumber.replace(/۴/g, "4");
    stringNumber = stringNumber.replace(/۵/g, "5");
    stringNumber = stringNumber.replace(/۶/g, "6");
    stringNumber = stringNumber.replace(/۷/g, "7");
    stringNumber = stringNumber.replace(/۸/g, "8");
    stringNumber = stringNumber.replace(/۹/g, "9");
  


    stringNumber = stringNumber.replace(/۰/g, "0");
    stringNumber = stringNumber.replace(/۱/g, "1");
    stringNumber = stringNumber.replace(/۲/g, "2");
    stringNumber = stringNumber.replace(/۳/g, "3");
    stringNumber = stringNumber.replace(/۴/g, "4");
    stringNumber = stringNumber.replace(/۵/g, "5");
    stringNumber = stringNumber.replace(/۶/g, "6");
    stringNumber = stringNumber.replace(/۷/g, "7");
    stringNumber = stringNumber.replace(/۸/g, "8");
    stringNumber = stringNumber.replace(/۹/g, "9");

    
    // Return the converted number.
    return stringNumber;
  }
  onKeyPress(e:any) {
    var charCode = (e.which) ? e.which : e.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {

      e.preventDefault();
      // this.formData.phoneNumber=this.convertNumber(this.formData.phoneNumber);
    }
  }

  validateIranianNationalCode(input:any) {
    if (!/^\d{10}$/.test(input))
        return false;
    let check = parseInt(input[9]);
    let sum = 0;
    let i;
    for (i = 0; i < 9; ++i) {
        sum += parseInt(input[i]) * (10 - i);
    }
    sum %= 11;
    return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
}
  formData: any = {}; // آبجکت برای ذخیره مقادیر فرم

  onSubmit(form: NgForm) {
    form.value.phone_number=this.convertNumber(form.value.phone_number);
    if (form.valid) {
      this.formData.dosierID =this.convertNumber (form.value.DosierID);      
      this.formData.name = form.value.name;
      this.formData.lastName = form.value.last_name;
      this.formData.nationalCode = this.convertNumber(form.value.national_code);
      this.formData.phoneNumber = this.convertNumber(form.value.phone_number);
      this.formData.insuranceId =Number( this.selectInsurance);
      this.formData.gender = Number(form.value.gender);
      if(this.validateIranianNationalCode(this.formData.nationalCode)==false) {alert("کد ملی صحیحی وارد نمایید");return}
      console.log(this.formData); // نمایش آبجکت در کنسول
      console.log(form.value); // نمایش آبجکت در کنسول

      this.reqService.AddPatient(this.formData).subscribe(a=>{
        alert("ثبت نام شما با موفقیت انجام شد حالا می توانید وارد سیستم شوید");
        this.router.navigate(['login']);
      },(error:any)=>{
        console.log(error);
        if(error.error.title="One or more validation errors occurred."){
          alert(error.error.detail);
          return;
        }
        alert("ثبت نام با مشکل مواجه شد");
      })
    }else{
      let a:any=form.controls;
      if(a.phone_number.valid==false)  {alert("لطفا شماره تلفن همراه خواسته شده را درست وارد نمایید مثال: 09123456789") ;return}
      if(a.national_code.valid==false)  {alert("لطفا کد ملی خواسته شده را درست وارد نمایید") ;return}
      if(a.name.valid==false || a.last_name.valid==false )  {alert("لطفا نام و نام خانوادگی را وارد نمایید") ;return}
      if(a.insurance.valid==false)  {alert("لطفا نوع بیمه را انتخاب نمایید") ;return}
      if(a.gender.valid==false)  {alert("لطفا جنسیت را انتخاب نمایید") ;return}
      alert("لطفا اطلاعات خواسته شده را درست وارد نمایید") ;
    }
  }
}
