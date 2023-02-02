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
      // searchString = '';
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
}
