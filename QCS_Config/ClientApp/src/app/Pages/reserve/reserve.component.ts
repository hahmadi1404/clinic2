import { Component } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-reserve',
  templateUrl: './reserve.component.html',
  styleUrls: ['./reserve.component.css']
})
export class ReserveComponent {
  dataLoading=true;
  AllData:any=[];
  ReserveStatus=[];
  ReserveStatusOptions:any[]=[

];
  selectReserveStatus="";
  // ClinicId: number=-1;
  constructor(private confirmationService: ConfirmationService,private reqService:RequestService,private loginService: LoginService) {
    // this.ClinicId=Number(this.loginService.getCookie('permission'));
  
    this.getData();
    this.GetReserveStatus();

  }

  getData(){
    this.dataLoading=true;
    this.reqService.GetReserves().subscribe((data:any)=>{
      this.AllData=data;
      this.dataLoading=false;
    });
  }
  GetReserveStatus(){
    this.reqService.GetReserveStatus().subscribe((data:any)=>{
      this.ReserveStatus=data;
      // this.ReserveStatusOptions=[];
      data.forEach((element:any) => {
        this.ReserveStatusOptions.push({ name: element.statusName, value: element.statusName,label:element.statusName })
        
      });
    });

  }

  currentData:any={drId:-1, drName:"",drSpecialty:"",days:""};
  edit(data:any){

    console.log(data);
    this.currentData=Object.assign({}, data);
    this.visible = true;
  }
  remove(e:any,data:any){
    e.stopPropagation()
    this.confirmationService.confirm({
      
      target: e.target,
      message: 'آیا از خذف کردن این رزرو وقت مطمئنید؟',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
          this.reqService.RemoveReserve(data).subscribe(a=>{
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
    this.reqService.UpdateReserve(this.currentData).subscribe(()=>{
      
      this.loading=false;
      this.getData();
      this.visible=false;
    
    }); 
  }


}
