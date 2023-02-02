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
  
  orders: Array<any>;

  constructor(private modalService: NgbModal, public localDataService: LocalDataService, public dataService: DataService, private router: Router) { }

  ngOnInit(): void {
    this.getWarehouseOrders();  
  }

  getWarehouseOrders(){
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

}
