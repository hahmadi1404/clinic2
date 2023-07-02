import { Component } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-services-page',
  templateUrl: './services-page.component.html',
  styleUrls: ['./services-page.component.css']
})
export class ServicesPageComponent {
  Services:any=[];

  constructor(private confirmationService: ConfirmationService,private reqService:RequestService,private loginService: LoginService) {
    this.ClinicId=Number(this.loginService.getCookie('permission'));
  
    this.getServices();
  }
  getServices(){
    this.reqService.GetServices().subscribe((data:any)=>{
      this.img = new Image();
      this.img.src = '/assets/DragImage.png';
      this.Services=data;
    });
  }

currentService:any={serviceId:-1, description:"",title:""};
editMode=false;
  editService(service:any){

    console.log(service);
    this.editMode=true;
    this.currentService=Object.assign({}, service);
    this.ImageUrl=`url('/images/${this.ClinicId}/Service/${service.serviceId}/image')`;
    this.visible = true;
  }
  removeService(e:any,service:any){
    this.confirmationService.confirm({
      target: e.target,
      message: 'آیا از خذف کردن این خدمت مطمئنید؟',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
          this.reqService.RemoveService(service).subscribe(a=>{
            this.getServices();
          });
      }
  });

  }

  visible:Boolean=false;
  addService(){
    this.ImageUrl="";
    this.editMode=false;
    this.currentService={description:"",title:""};
    this.visible = true;
  }
  Gallery(service:any){
    this.reqService.GetGalleryList("ServiceGallery",service.serviceId).subscribe((list:any)=>{
      this.GaleryFiles=list;
      this.GalleryVisible=true;
      this.currentService=service;
    });
    
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
  CreateService(){
    
    this.loading=true;
    this.reqService.AddService(this.currentService).subscribe((data:any)=>{
      console.log("data");
      console.log(data);
      if(this.file){
      let formdata=new FormData();
      formdata.append("file",this.file)
      this.reqService.AddImage("Service",data.serviceId,"Image", formdata).subscribe(()=>{
        this.loading=false;
        this.getServices();
        this.visible=false;
      });
    }else{
      this.loading=false;
      this.getServices();
      this.visible=false;
    }
    });
    
  }


  UpdateService(){
    
    this.loading=true;
    this.reqService.UpdateService(this.currentService).subscribe(()=>{
      if(this.file){
      let formdata=new FormData();
      formdata.append("file",this.file)
      this.reqService.AddImage("Service",this.currentService.serviceId,"Image", formdata).subscribe(()=>{
        this.loading=false;
        this.getServices();
        this.visible=false;
      });
    }else{
      this.loading=false;
      this.getServices();
      this.visible=false;
    }
    }); 
  }
  addGallery(input:any){
    var file =input.target.files[0];
    let formdata=new FormData();
    for (let index = 0; index < input.target.files.length; index++) {
      const element = input.target.files[index];
      formdata.append("file",element)
    }
    this.reqService.AddGallery("ServiceGallery",this.currentService.serviceId, formdata).subscribe((e:any)=>{
      this.reqService.GetGalleryList("ServiceGallery",this.currentService.serviceId).subscribe((list:any)=>{

        this.GaleryFiles=list;
      });
    });
  }

  img:any;
  setItem(e:any, item:string){
    
    e.dataTransfer.setData("Text", item);
    this.dragMode=item!='';
    let xx=(e.offsetX/150)*48;
    let yy=(e.offsetY/150)*48;
    e.dataTransfer.setDragImage(this.img, xx, yy);
  }
  drop(e:any){
    e.preventDefault();
    let item = e.dataTransfer.getData("Text");
    console.log(item);
    this.reqService.RemoveImage("ServiceGallery",this.currentService.serviceId, item).subscribe(()=>{
      const index = this.GaleryFiles.indexOf(item);
      if (index > -1) { 
        this.GaleryFiles.splice(index, 1); 
      }
    });

  }
  allowDrop(e:any){
    e.preventDefault();
  }
}
