import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocalDataService } from '../services/local-data.service';
import { Location } from '@angular/common';
import PartEditDTO from '../models/partEditDTO';
import { HttpEventType, HttpResponse } from '@angular/common/http';

import { ToastService } from '../services/toast.service';

@Component({
  selector: 'app-order-edit',
  templateUrl: './order-edit.component.html',
  styleUrls: ['./order-edit.component.css']
})
export class OrderEditComponent implements OnInit {

  myState;
  selectedOrder = {
    orderMasterId: 0,
    orderQuantity: 0,
    partMasterId : 0,
    partMasterSelectList : [],
  };
  partList = [];

  // form
  orderEditForm: FormGroup;
  submitted = false;
  apiMessage = '';
  apiError = false;
  modelErrors = [];

  constructor(private toastService: ToastService,
    private location: Location,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    public localDataService: LocalDataService,
    public dataService: DataService,
    private router: Router) {
  }
  
  ngOnInit(): void {
    this.myState = this.location.getState();

    if (this.myState.selectedOrder == null || this.myState.selectedOrder==undefined)
      this.router.navigate(['/warehouse']);
    else {
      this.selectedOrder = this.myState.selectedOrder.data;

      this.selectedOrder.partMasterSelectList.forEach(part => {
        this.partList.push({
          selected: this.selectedOrder.partMasterId===Number(part.value) ? true : false,
          value: part.value,
          text: part.text
        });
      });

      // form
      this.orderEditForm = this.fb.group({
        partMasterId: ['',  [
            Validators.required,
          ]],
        orderQuantity: ['',  [Validators.pattern("^[0-9]*$")]],
      });
      // patch form values
      this.orderEditForm.setValue({
        partMasterId: this.selectedOrder.partMasterId,
        orderQuantity: this.selectedOrder.orderQuantity,
      });
  
      console.log(this.orderEditForm);
    }    
  }

    
  goBack(){
    this.router.navigate(['/warehouse']);
  }
  onSubmit(): void {  
    this.apiError = false;
    this.apiMessage = '';
    this.modelErrors = [];

    this.submitted = true;
    if (this.orderEditForm.valid) {
      var orderMasterEditVM = {
        orderMasterId: this.selectedOrder.orderMasterId,
        partMasterId: Number(this.orderEditForm.value["partMasterId"]),
        orderQuantity: Number(this.orderEditForm.value["orderQuantity"])
      };
      console.log(orderMasterEditVM);


      this.dataService.editOrderPost(orderMasterEditVM)
        .subscribe(
          response => {
            console.log(response);   
            this.apiError = false;
            if(response.responseCode===0)
              this.toastService.showSuccess('',response.responseMessage);
          },
          err => {
            this.apiError = true;
            console.log(err);
            if (err.status === 400) {
              if (err.error) {
                this.modelErrors = this.localDataService.display400andEx(err.error, 'Warehouse-Order-Edit');
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
  
  get f() {
    return this.orderEditForm.controls;
  }

  
}
