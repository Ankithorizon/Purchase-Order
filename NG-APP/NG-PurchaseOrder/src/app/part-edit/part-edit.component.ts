import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocalDataService } from '../services/local-data.service';
import { Location } from '@angular/common';
 
@Component({
  selector: 'app-part-edit',
  templateUrl: './part-edit.component.html',
  styleUrls: ['./part-edit.component.css']
})
export class PartEditComponent implements OnInit {

  myState;
  selectedPart = {
    partMasterId: 0,
    partDetailId: 0,
    partCode: '',
    partName: '',
    partDesc: '',
    partDrgFile: '',
  };

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
        partDrgFile: ['', Validators.required],
      });

      // path form values
      this.partEditForm.setValue({
                partName: this.selectedPart.partName,
                partCode: this.selectedPart.partCode,
                partDesc: this.selectedPart.partDesc,
                partDrgFile: this.selectedPart.partDrgFile,
      });
      console.log(this.partEditForm);
    }    
  }

  get f() {
    return this.partEditForm.controls;
  }


  onSubmit(): void {
    this.submitted = true;
    if (this.partEditForm.valid) {
      console.log('form valid!');
    }
    else {
      console.log('form in-valid!');
    }
  }
  
  goBack(){
    this.router.navigate(['/engineering']);
  }
}
