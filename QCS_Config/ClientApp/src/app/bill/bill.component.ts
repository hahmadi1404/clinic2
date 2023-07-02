import { Component } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { RequestService } from '../Services/request.service';
import { LoginService } from '../Services/login.service';

@Component({
  selector: 'app-bill',
  templateUrl: './bill.component.html',
  styleUrls: ['./bill.component.css']
})
export class BillComponent {
  AllData:any=[];

  // ClinicId: number=-1;
  constructor(private confirmationService: ConfirmationService,private reqService:RequestService,private loginService: LoginService) {
    // this.ClinicId=Number(this.loginService.getCookie('permission'));
    this.ClinicId=Number(this.loginService.getCookie('permission'));
    this.img = new Image();
      this.img.src = '/assets/DragImage.png';
    this.getData();
    

  }

  getData(){
    this.reqService.GetAllBill().subscribe((data:any)=>{
      this.AllData=data;
    });
  }


  currentData:any={drId:-1, drName:"",drSpecialty:"",days:""};
  edit(data:any){
    this.activeIndex=0;
    console.log(data);

    this.currentData=Object.assign({}, data);
    this.reqService.GetGalleryList("BillGallery",data.billId).subscribe((list:any)=>{
      this.GaleryFiles=list;

    });
    this.visible = true;
  }
  remove(e:any,data:any){
    e.stopPropagation()
    this.confirmationService.confirm({
      target: e.target,
      message: 'آیا از خذف کردن این صورتحساب مطمئنید؟',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
          this.reqService.RemoveBill(data).subscribe(a=>{
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
    this.reqService.UpdateBill(this.currentData).subscribe(()=>{
      
      this.loading=false;
      this.getData();
      this.visible=false;
    
    }); 
  }

  activeIndex=0;
  img:any;
  dragMode=false;
  ClinicId: number=-1;
  ImageUrl="";
  GaleryFiles:string[]=[];
  addGallery(input:any){
    var file =input.target.files[0];
    let formdata=new FormData();
    for (let index = 0; index < input.target.files.length; index++) {
      const element = input.target.files[index];
      formdata.append("file",element)
    }
    this.reqService.AddGallery("BillGallery",this.currentData.billId, formdata).subscribe((e:any)=>{
      this.reqService.GetGalleryList("BillGallery",this.currentData.billId).subscribe((list:any)=>{

        this.GaleryFiles=list;
      });
    });
  }


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
    this.reqService.RemoveImage("BillGallery",this.currentData.billId, item).subscribe(()=>{
      const index = this.GaleryFiles.indexOf(item);
      if (index > -1) { 
        this.GaleryFiles.splice(index, 1); 
      }
    });

  }
  allowDrop(e:any){
    e.preventDefault();
  }
  Gallery(e:any,data:any){
    e.stopPropagation();
    this.currentData=Object.assign({}, data);
    this.reqService.GetGalleryList("BillGallery",data.billId).subscribe((list:any)=>{
      this.GaleryFiles=list;

    });
   
    this.activeIndex=1;
    this.visible=true;
  }

}
