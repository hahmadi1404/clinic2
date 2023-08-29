import { Component } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-shift',
  templateUrl: './shift.component.html',
  styleUrls: ['./shift.component.css']
})
export class ShiftComponent {
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
    this.reqService.GetShift().subscribe((data:any)=>{
      this.AllData=data;
      this.dataLoading=false;
    });
  }
  editMode=false;
  add(){
    this.editMode=false;
    this.currentData={shiftName:"",departementId:0,clinicId:this.ClinicId};
    this.visible = true
    ;
  }
  currentData:any={id:-1,shiftName:"",departementId:0};

  Create(){
    this.loading=true;
    this.reqService.AddShift(this.currentData).subscribe((data:any)=>{
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
      message: 'آیا از خذف کردن این شیفت مطمئنید؟',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
          this.reqService.RemoveShift(data).subscribe(a=>{
            this.getData();
          });
      }
  });

  }

  visible:Boolean=false;


  file:any;
  readURL(input:any) {
    let t=this;
    var file =input.target.files[0];
    if (file) {
      t.file=file;
      console.log("file");
      console.log(file);
        var reader = new FileReader();
        reader.onload = function(e:any) {
          console.log(e);
         
        }
        // reader.readAsText(file,"UTF-8");
        reader.readAsDataURL(file);    
    }
  }

  loading:boolean=false;
  Update(){
    
    this.loading=true;
    this.reqService.UpdateShift(this.currentData).subscribe(()=>{
      
      this.loading=false;
      this.getData();
      this.visible=false;
    
    }); 
  }
  activeIndex=0;
}
