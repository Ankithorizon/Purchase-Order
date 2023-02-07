import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocalDataService } from '../services/local-data.service';
import { Location } from '@angular/common';
import { HttpEventType, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-order-create',
  templateUrl: './order-create.component.html',
  styleUrls: ['./order-create.component.css']
})
export class OrderCreateComponent implements OnInit {

  parts = [];

  // form
  orderCreateForm: FormGroup;
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
    this.getPartMastList();

    // form
    this.orderCreateForm = this.fb.group({
      partMasterId: ['', [
        Validators.required,
      ]],
      orderQuantity: ['', [Validators.required,Validators.pattern("^[0-9]*$")]],
    });
  }
  
  reset() {
    this.orderCreateForm.reset();
    this.submitted = false;

    // disable browser back button
    history.pushState(null, '');
  }  

  goBack() {
    this.router.navigate(['/warehouse']);
  }
  
  getPartMastList() {
    this.dataService.getPartMasterList()
      .subscribe(
        data => {          
          console.log(data);

          data.forEach(part => {
            this.parts.push({              
              value: part.value,
              text: part.text
            });
          });
        },
        error => {
          console.log(error);      
      });
  }

  get f() {
    return this.orderCreateForm.controls;
  }

  onSubmit(): void {  
    this.apiError = false;
    this.apiMessage = '';
    this.modelErrors = [];

    this.submitted = true;
    if (this.orderCreateForm.valid) {
      var orderMaster = {
        partMasterId: Number(this.orderCreateForm.value["partMasterId"]),
        orderQuantity: Number(this.orderCreateForm.value["orderQuantity"])
      };
      console.log(orderMaster);


      this.dataService.createOrder(orderMaster)
        .subscribe(
          response => {
            console.log(response);   
            if(response.responseCode===0)
              this.apiMessage = 'Order Created Successfully ! Confirmation : #' + response.responseMessage;
            
         
            setTimeout(() => {
              this.apiMessage = '';
              this.reset();
            }, 3000);  
          },
          err => {         
            console.log(err);     
            this.apiError = true;
         
            if (err.status === 400) {
              if (err.error) {
                this.modelErrors = this.localDataService.display400andEx(err.error, 'Warehouse-Order-Create');
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
