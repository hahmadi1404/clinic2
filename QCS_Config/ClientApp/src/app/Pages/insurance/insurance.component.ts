import { Component } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-insurance',
  templateUrl: './insurance.component.html',
  styleUrls: ['./insurance.component.css']
})
export class InsuranceComponent {
  AllData:any=[];
  dataLoading=true;
  ClinicId: number=-1;
  // ClinicId: number=-1;
  constructor(private confirmationService: ConfirmationService,private reqService:RequestService,private loginService: LoginService) {
    // this.ClinicId=Number(this.loginService.getCookie('permission'));
    this.ClinicId=Number(this.loginService.getCookie('permission'));
    this.getData();
    

  }

  getData(){
    this.dataLoading=true;
    this.reqService.GetInsurance().subscribe((data:any)=>{
      this.AllData=data;
      this.dataLoading=false;
    });
  }
  editMode=false;
  add(){
    this.editMode=false;
    this.currentData={insuranceName:"",departementId:0,clinicId:this.ClinicId};
    this.visible = true
    ;
  }
  currentData:any={id:-1,insuranceName:"",departementId:0};

  Create(){
    this.loading=true;
    this.reqService.AddInsurance(this.currentData).subscribe((data:any)=>{
      console.log("data");
      console.log(data);
      this.getData();
      this.loading=false;
      this.visible=false;
    });
  }
  edit(data:any){
    this.editMode=true;
    this.activeIndex=0;
    console.log(data);

    this.currentData=Object.assign({}, data);
    this.visible = true;
  }
  remove(e:any,data:any){
    e.stopPropagation()
    this.confirmationService.confirm({
      target: e.target,
      message: 'آیا از خذف کردن این  بیمه مطمئنید؟',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
          this.reqService.RemoveInsurance(data).subscribe(a=>{
            this.getData();
          });
      }
  });

  }

  visible:Boolean=false;

  loading:boolean=false;
  Update(){
    
    this.loading=true;
    this.reqService.UpdateInsurance(this.currentData).subscribe(()=>{
      
      this.loading=false;
      this.getData();
      this.visible=false;
    
    }); 
  }
  activeIndex=0;
}
