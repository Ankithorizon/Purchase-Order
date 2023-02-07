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

  orderQty;
  orderQtyMessage = '';

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

    this.orderQty = 0;
    this.orderQtyMessage = '';

    // disable browser back button
    history.pushState(null, '');
  }  

  goBack() {
    this.router.navigate(['/receiving']);
  }

  get f() {
    return this.orderReceiveForm.controls;
  }

  getOrderQuantity() {
    var refCode = this.orderReceiveForm.value["refCode"];
    this.dataService.getOrderQuantity(refCode)
      .subscribe(
        data => {          
          console.log(data);    
          this.orderQty = data.qty;
          this.orderQtyMessage = data.status;     
        },
        error => {
          console.log(error);     
        });
   }
  
  onSubmit(): void {  
    this.apiError = false;
    this.apiMessage = '';
    this.modelErrors = [];

    this.submitted = true;
    if (this.orderReceiveForm.valid) {
      var receivePartView = {
        refCode: this.orderReceiveForm.value["refCode"],
        receiveQuantity: Number(this.orderReceiveForm.value["receiveQuantity"])
      };
      console.log(receivePartView);

      
      this.dataService.receiveOrder(receivePartView)
        .subscribe(
          response => {
            console.log(response);   
            if (response.responseCode === 0) {
              this.apiMessage = response.responseMessage;              
              setTimeout(() => {
                this.apiMessage = '';
                this.reset();
              }, 3000);
            }
            else if (response.responseCode === -1) {
              this.apiError = true;
              this.apiMessage = response.responseMessage;
            }
          },
          err => {         
            console.log(err);     
            this.apiError = true;
         
            if (err.status === 400) {
              if (err.error) {
                this.modelErrors = this.localDataService.display400andEx(err.error, 'Receiving-Order');
              }
              else {
                this.apiMessage = '400 : Error !';  
              }            
            }
            else if(err.status===500) {
              this.apiMessage = err.error;
            }
            else {
              this.apiMessage = 'Error!';
            }
          }
      );
    }
    else {
      console.log('form in-valid!');
    }
  }
}
