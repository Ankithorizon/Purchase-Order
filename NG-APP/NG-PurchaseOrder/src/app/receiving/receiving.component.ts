import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocalDataService } from '../services/local-data.service';

import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

import { ToastService } from '../services/toast.service';

@Component({
  selector: 'app-receiving',
  templateUrl: './receiving.component.html',
  styleUrls: ['./receiving.component.css']
})
export class ReceivingComponent implements OnInit {

  closeResult: string = '';
  receiveOrderView;

  term: string;
  orderBySettings = {
    columnName: '',
    columnOrder: ''
  };
  orderByParam = '';

  rOrders: Array<any>;
  constructor(
    private toastService: ToastService,
    private modalService: NgbModal,
    public localDataService: LocalDataService,
    public dataService: DataService,
    private router: Router) { }

  ngOnInit(): void {
    this.getReceivedOrders(null, null);  
  }

  getReceivedOrders(searchString, sortOrder) {
    if (searchString == null && sortOrder==null) {
      this.dataService.getReceivedOrders()
      .subscribe(
        data => {          
          console.log(data);
          this.rOrders = data;
        },
        error => {
          console.log(error);      
      });
    }
    else if(searchString!=null && sortOrder==null) {
  
    }  
    else {
   
    }
  }


  // view receiving order detail
  // Modal
  open(content, selectedOrder) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      console.log(this.closeResult);
    });
  } 
  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return  `with: ${reason}`;
    }
  }

  reloadReceivedOrders() {
    this.getReceivedOrders(null, null);  
    this.term = null;
  }
}
