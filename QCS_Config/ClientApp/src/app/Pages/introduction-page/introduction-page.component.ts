import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api/menuitem';

import { LoginService } from 'src/app/Services/login.service';
import { RequestService } from 'src/app/Services/request.service';

@Component({
  selector: 'app-introduction-page',
  templateUrl: './introduction-page.component.html',
  styleUrls: ['./introduction-page.component.css']
})
export class IntroductionPageComponent implements OnInit{

  Data:any={};
  img:any;
  constructor(private reqService:RequestService,private loginService: LoginService) {

    this.Data.description="";
    this.img = new Image();
    this.img.src = '/assets/DragImage.png';
  }

  // @ViewChild('imageUpload') image: ElementRef;
  items: MenuItem[]=[ {
    label: 'Remove',
    icon: 'pi pi-fw pi-power-off',
    command(event) {
        alert("asdasd");
    },
}];
dragMode=false;
  ClinicId: number=-1;
  ImageUrl="";
  GaleryFiles:string[]=[];
  ngOnInit(): void {
    this.ClinicId=Number(this.loginService.getCookie('permission'));
    this.ImageUrl=`url('/images/${this.ClinicId}/Intro/0/image')`;
    this.reqService.GetClinicIntro().subscribe((d:any)=>{

        this.Data=d;

        this.reqService.GetGalleryList("IntroGallery",0).subscribe((list:any)=>{

          this.GaleryFiles=list;
        });
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
  loading:boolean=false;
  Save(){
    console.log(this.Data);
    this.loading=true;
    this.reqService.UpdateClinicIntro(this.Data).subscribe(()=>{
      if(this.file){
      let formdata=new FormData();
      formdata.append("file",this.file)
      this.reqService.AddImage("/images/Intro",0,"Image", formdata).subscribe(()=>{
        this.loading=false;
      });
    }else{
      this.loading=false;
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
    this.reqService.AddGallery("IntroGallery",0, formdata).subscribe((e:any)=>{
      this.reqService.GetGalleryList("IntroGallery",0).subscribe((list:any)=>{

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
    this.reqService.RemoveImage("IntroGallery",0, item).subscribe(()=>{
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
