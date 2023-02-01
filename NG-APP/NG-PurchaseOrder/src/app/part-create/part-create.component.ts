import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocalDataService } from '../services/local-data.service';
import { Location } from '@angular/common';
import PartCreateDTO from '../models/partCreateDto';
import { HttpEventType, HttpResponse } from '@angular/common/http';
 
@Component({
  selector: 'app-part-create',
  templateUrl: './part-create.component.html',
  styleUrls: ['./part-create.component.css']
})
export class PartCreateComponent implements OnInit {

  partCreateForm: FormGroup;
  submitted = false;
  partModel = { 
    partCode: '',
    partName: '',
    partDesc: '',
    partDrgFile: '',
  };

  // part file upload
  currentFile?: File;
  progress = 0;
  apiMessage = '';
  apiError = false;
  modelErrors = [];
  fileName = 'Select File';
  fileInfos?: Observable<any>;  
  partCreate = new PartCreateDTO();

  constructor(private location: Location,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    public localDataService: LocalDataService,
    public dataService: DataService,
    private router: Router) {
  }

  ngOnInit(): void {
    this.partCreateForm = this.fb.group({
      partCode: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(20)
      ]],
      partName: ['', Validators.required],
      partDesc: ['', Validators.required],
      partDrgFile: ['', Validators.required],
    });
  }

  get f() {
    return this.partCreateForm.controls;
  }

  goBack(){
    this.router.navigate(['/engineering']);
  }


  // part file upload
  selectFile(event: any): void {
    if (event.target.files && event.target.files[0]) {
      const file: File = event.target.files[0];
      this.currentFile = file;
      this.fileName = this.currentFile.name;
    } else {
      this.fileName = 'Select File';
    }
  }

  remoteCheckPartCode() {
    var newPartCode = this.partCreateForm.value["partCode"];
    
  }

  onSubmit(): void {
    this.apiError = false;
    this.apiMessage = '';
    this.modelErrors = [];

    this.submitted = true;
    if (this.partCreateForm.valid) {

      this.partCreate.partCode = this.partCreateForm.value["partCode"];
      this.partCreate.partName = this.partCreateForm.value["partName"];
      this.partCreate.partDesc = this.partCreateForm.value["partDesc"];
      this.partCreate.partDrgFile = this.fileName;
      this.partCreate.partFile = this.currentFile;
   
      console.log(this.partCreate);

      this.dataService.partCreatePost(this.partCreate).subscribe(
        (event: any) => {
          if (event.type === HttpEventType.UploadProgress) {
            this.progress = Math.round(100 * event.loaded / event.total);
          } else if (event instanceof HttpResponse) {
            this.apiMessage = event.body.responseMessage;
            this.apiError = false;
          }
        },
        (err: any) => {
          this.apiError = true;
          
          if (err.status == 400 || err.status == 500) {
            if (err.error.errors) {
              this.modelErrors = this.localDataService.display400andEx(err.error.errors, 'Part-Create');
            }
            else {
              this.apiMessage = err.error;  
            }            
          }
          else {
            this.apiMessage = err;
          }
        });
      /*
      if (this.currentFile) {     
      }
      */
    }
    else {
      console.log('form in-valid!');
    }
  }
}
