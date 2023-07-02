import { Component } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-doctors',
  templateUrl: './doctors.component.html',
  styleUrls: ['./doctors.component.css']
})
export class DoctorsComponent {
  Doctors:any=[];

  constructor(private confirmationService: ConfirmationService,private reqService:RequestService,private loginService: LoginService) {
    this.ClinicId=Number(this.loginService.getCookie('permission'));
  
    this.getDoctors();
  }
  getDoctors(){
    this.reqService.GetDoctors().subscribe((data:any)=>{
      this.Doctors=data;
    });
  }

currentDoctor:any={drId:-1, drName:"",drSpecialty:"",days:""};
editMode=false;
  editDoctor(doctor:any){

    console.log(doctor);
    this.editMode=true;
    this.currentDoctor=Object.assign({}, doctor);
    this.ImageUrl=`url('/images/${this.ClinicId}/Dr/${doctor.drId}/image')`;
    this.visible = true;
  }
  removeDoctor(e:any,doctor:any){
    this.confirmationService.confirm({
      target: e.target,
      message: 'آیا از خذف کردن این پزشک مطمئنید؟',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
          this.reqService.RemoveDoctor(doctor).subscribe(a=>{
            this.getDoctors();
          });
      }
  });

  }

  visible:Boolean=false;
  addDoctor(){
    this.ImageUrl="";
    this.editMode=false;
    this.currentDoctor={drName:"",drSpecialty:"",days:""};
    this.visible = true;
  }



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
          t.ImageUrl= 'url('+e.target.result +')';
         
        }
        // reader.readAsText(file,"UTF-8");
        reader.readAsDataURL(file);    
    }
  }
  GaleryFiles:string[]=[];
  dragMode=false;
GalleryVisible=false;
  ClinicId: number=-1;
  ImageUrl="";
  description="";
  title="";
  loading:boolean=false;
  CreateDoctor(){
    
    this.loading=true;
    this.reqService.AddDoctor(this.currentDoctor).subscribe((data:any)=>{
      console.log("data");
      console.log(data);
      if(this.file){
      let formdata=new FormData();
      formdata.append("file",this.file)
      this.reqService.AddImage("Dr",data.drId,"Image", formdata).subscribe(()=>{
        this.loading=false;
        this.getDoctors();
        this.visible=false;
      });
    }else{
      this.loading=false;
      this.getDoctors();
      this.visible=false;
    }
    });
    
  }


  UpdateDoctor(){
    
    this.loading=true;
    this.reqService.UpdateDoctor(this.currentDoctor).subscribe(()=>{
      if(this.file){
      let formdata=new FormData();
      formdata.append("file",this.file)
      this.reqService.AddImage("Dr",this.currentDoctor.drId,"Image", formdata).subscribe(()=>{
        this.loading=false;
        this.getDoctors();
        this.visible=false;
      });
    }else{
      this.loading=false;
      this.getDoctors();
      this.visible=false;
    }
    }); 
  }
}
