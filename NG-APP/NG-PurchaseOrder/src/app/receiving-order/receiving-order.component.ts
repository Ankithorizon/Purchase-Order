import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocalDataService } from '../services/local-data.service';
import { Location } from '@angular/common';
import { HttpEventType, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-receiving-order',
  templateUrl: './receiving-order.component.html',
  styleUrls: ['./receiving-order.component.css']
})
export class ReceivingOrderComponent implements OnInit {

  // form
  orderReceiveForm: FormGroup;
  submitted = false;
  apiMessage = '';
  apiError = false;
  modelErrors = [];

  constructor(
    private location: Location,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    public localDataService: LocalDataService,
    public dataService: DataService,
    private router: Router) {
  }

 ngOnInit(): void {
    // form
    this.orderReceiveForm = this.fb.group({
      refCode: ['', [
        Validators.required,
      ]],
      receiveQuantity: ['', [Validators.required,Validators.pattern("^[0-9]*$")]],
    });
  }
  
  reset() {
    this.orderReceiveForm.reset();
    this.submitted = false;

    // disable browser back button
    history.pushState(null, '');
  }  

  goBack() {
    this.router.navigate(['/receiving']);
  }

  get f() {
    return this.orderReceiveForm.controls;
  }

  onSubmit(): void {  
    this.apiError = false;
    this.apiMessage = '';
    this.modelErrors = [];

    this.submitted = true;
    if (this.orderReceiveForm.valid) {
      var orderReceive = {
        refCode: this.orderReceiveForm.value["refCode"],
        receiveQuantity: Number(this.orderReceiveForm.value["receiveQuantity"])
      };
      console.log(orderReceive);
    }
    else {
      console.log('form in-valid!');
    }
  }
}
