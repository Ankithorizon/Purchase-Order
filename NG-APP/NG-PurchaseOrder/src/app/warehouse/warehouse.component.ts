import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocalDataService } from '../services/local-data.service';

import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-warehouse',
  templateUrl: './warehouse.component.html',
  styleUrls: ['./warehouse.component.css']
})
export class WarehouseComponent implements OnInit {

  closeResult: string = '';
  orderMasterView;


  term: string;
  orderBySettings = {
    columnName: '',
    columnOrder: ''
  };
  orderByParam = '';
  
  orders: Array<any>;

  constructor(private modalService: NgbModal, public localDataService: LocalDataService, public dataService: DataService, private router: Router) { }

  ngOnInit(): void {
    this.getWarehouseOrders(null, null);  
  }

  getWarehouseOrders(searchString, sortOrder) {
    if (searchString == null && sortOrder==null) {
      this.dataService.getWarehouseOrders()
      .subscribe(
        data => {          
          console.log(data);
          this.orders = data;
        },
        error => {
          console.log(error);      
      });
    }
    else if(searchString!=null && sortOrder==null) {
      this.dataService.searchParts(searchString)
      .subscribe(
        data => {          
          console.log(data);
          this.orders = data;
        },
        error => {
          console.log(error);      
      });
    }  
    else {
      console.log(sortOrder, searchString);
      if(searchString==undefined)
        searchString = '';
      this.dataService.searchAndOrderByParts(sortOrder,searchString)
      .subscribe(
        data => {          
          console.log(data);
          this.orders = data;
        },
        error => {
          console.log(error);      
      });
    }
  }

  searchPart() {
    this.getWarehouseOrders(this.term, null);
  }

  reloadOrders() {
    this.getWarehouseOrders(null, null);  
    this.term = null;
  }

  orderBy(param) {
    if (this.orderBySettings.columnName == param) {
      if (this.orderBySettings.columnOrder == 'asc') {
        this.orderBySettings.columnOrder = 'desc';
        this.orderByParam = param.toLowerCase() + '_' + this.orderBySettings.columnOrder;
      }
      else {
        this.orderBySettings.columnOrder = 'asc';
        this.orderByParam = param;
      }
    }
    else {
      this.orderBySettings.columnName = param;
      this.orderBySettings.columnOrder = 'asc';
      this.orderByParam = param;
    }    
    
    console.log(this.orderByParam);
    this.getWarehouseOrders(this.term, this.orderByParam);
  }

  // view order detail
  // Modal
  open(content, selectedOrder) {
    this.dataService.getOrderDetails(Number(selectedOrder.orderMasterId))
      .subscribe(
        data => {          
          console.log(data);
          this.orderMasterView = data;

          this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
            this.closeResult = `Closed with: ${result}`;
          }, (reason) => {
            this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
            console.log(this.closeResult);
          });
        },
        error => {
          // console.log(error);             
          if (error.status == 400) {
            this.orderMasterView = null;
          }
          this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
            this.closeResult = `Closed with: ${result}`;
          }, (reason) => {
            this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
            console.log(this.closeResult);
          });
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


  // edit order
  editOrder(selectedOrder) {
    this.dataService.editOrder(Number(selectedOrder.orderMasterId))
      .subscribe(
        data => {          
          console.log(data);            
          this.router.navigate(['/order-edit'], { state: { selectedOrder: {data} } });          
        },
        error => {
          console.log(error);       
        });
  }
}
