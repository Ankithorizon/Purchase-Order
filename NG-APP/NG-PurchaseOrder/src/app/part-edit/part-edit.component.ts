import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocalDataService } from '../services/local-data.service';
import { Location } from '@angular/common';
import PartEditDTO from '../models/partEditDTO';
import { HttpEventType, HttpResponse } from '@angular/common/http';
 
@Component({
  selector: 'app-part-edit',
  templateUrl: './part-edit.component.html',
  styleUrls: ['./part-edit.component.css']
})
export class PartEditComponent implements OnInit {

  // remote part-code check
  remotePartCodeCheckResponse = '';

  // part file upload
  currentFile?: File;
  progress = 0;
  modelErrors = [];
  apiMessage = '';
  apiError = false;
  fileName = 'Select File';
  fileInfos?: Observable<any>;  
  partEdit = new PartEditDTO();


  myState;
  selectedPart = {
    partMasterId: 0,
    partDetailId: 0,
    partCode: '',
    partName: '',
    partDesc: '',
    partDrgFile: '',
  };
  selectedPartFileName = '';

  partMasterId = 0;
  partDetailId = 0;

  partEditForm: FormGroup;
  submitted = false;
  partModel = { 
    partMasterId: 0,
    partDetailId : 0,
    partCode: '',
    partName: '',
    partDesc: '',
    partDrgFile: '',
  };
  previousPartDrgFile = '';
  previousPartCode = '';

  constructor(private location: Location,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    public localDataService: LocalDataService,
    public dataService: DataService,
    private router: Router) {
  }


  ngOnInit(): void {
    this.myState = this.location.getState();

    if (this.myState.selectedPart == null || this.myState.selectedPart==undefined)
      this.router.navigate(['/engineering']);
    else {
      this.selectedPart = this.myState.selectedPart.data;

      this.partMasterId = this.selectedPart.partMasterId;
      this.partDetailId = this.selectedPart.partDetailId;

      this.partEditForm = this.fb.group({
        partCode: ['',  [
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(20)
          ]],
        partName: ['', Validators.required],
        partDesc: ['', Validators.required],
      });

      // patch form values
      this.partEditForm.setValue({
                partName: this.selectedPart.partName,
                partCode: this.selectedPart.partCode,
                partDesc: this.selectedPart.partDesc,                
      });
      this.selectedPartFileName = this.selectedPart.partDrgFile;
      this.previousPartCode = this.selectedPart.partCode;
      this.previousPartDrgFile = this.selectedPart.partDrgFile;

      console.log(this.partEditForm);
    }    
  }

  get f() {
    return this.partEditForm.controls;
  }


  onSubmit(): void {
    this.apiError = false;
    this.apiMessage = '';
    this.modelErrors = [];

    this.submitted = true;
    if (this.partEditForm.valid) {

      this.partModel.partCode = this.partEditForm.value["partCode"];
      this.partModel.partName = this.partEditForm.value["partName"];
      this.partModel.partDesc = this.partEditForm.value["partDesc"];
      this.partModel.partDrgFile = this.selectedPartFileName;

      this.partEdit.partCode = this.partEditForm.value["partCode"];
      this.partEdit.partName = this.partEditForm.value["partName"];
      this.partEdit.partDesc = this.partEditForm.value["partDesc"];
      this.partEdit.partMasterId = Number(this.partMasterId);
      this.partEdit.partDetailId = Number(this.partDetailId);
      this.partEdit.previousPartCode = this.previousPartCode;
      this.partEdit.previousPartDrgFile = this.previousPartDrgFile;


      this.partEdit.partFile = this.currentFile;
        
      console.log(this.partEdit);
      this.dataService.partEditPost(this.partEdit).subscribe(
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


  resetPartFile() {
    this.currentFile = undefined;
    this.fileName = 'Select File';
  }

  remoteCheckPartCode() {
    var newPartCode = this.partEditForm.value["partCode"];
    var currentPartCode = this.previousPartCode;

    console.log(currentPartCode, newPartCode);
    this.dataService.remoteCheckPartCode(currentPartCode, newPartCode)
      .subscribe(
        data => {          
          console.log(data);    
          if (data.responseCode < 0) {
            this.remotePartCodeCheckResponse = data.responseMessage;
          }   
          else {
            this.remotePartCodeCheckResponse = '';
          }
        },
        error => {
          console.log(error);     
        });
  }
}
