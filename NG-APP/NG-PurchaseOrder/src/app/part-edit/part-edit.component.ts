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

  // part file upload
  currentFile?: File;
  progress = 0;
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
        partCode: ['', Validators.required],
        partName: ['', Validators.required],
        partDesc: ['', Validators.required],
      });

      // path form values
      this.partEditForm.setValue({
                partName: this.selectedPart.partName,
                partCode: this.selectedPart.partCode,
                partDesc: this.selectedPart.partDesc,                
      });
      this.selectedPartFileName = this.selectedPart.partDrgFile;

      console.log(this.partEditForm);
    }    
  }

  get f() {
    return this.partEditForm.controls;
  }


  onSubmit(): void {
    this.apiError = false;
    this.apiMessage = '';

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

      this.partEdit.partFile = this.currentFile;
        
      console.log(this.partEdit);
      this.dataService.upload(this.partEdit).subscribe(
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
            this.apiMessage = err.error;
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
}



// check for duplicate [part-code]
// maximum 20 characters [part-code]