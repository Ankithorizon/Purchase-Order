import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocalDataService } from '../services/local-data.service';

@Component({
  selector: 'app-engineering',
  templateUrl: './engineering.component.html',
  styleUrls: ['./engineering.component.css']
})
export class EngineeringComponent implements OnInit {

  term: string;
  
  parts: Array<any>;
  
  page: number = 1;
  count: number = 0;
  tableSize: number = 10;
  tableSizes: any = [10,20,30];
  
  constructor(public localDataService: LocalDataService, public dataService: DataService, private router: Router) { }

  ngOnInit(): void {
    this.getAllParts();  
  }

  onTableDataChange(event: any) {
    this.page = event;
    if (this.parts != null && this.parts.length > 0) {      
    }
    else {
      this.getAllParts();
    }
  }
  onTableSizeChange(event: any): void {
    this.tableSize = event.target.value;
    this.page = 1;
    if (this.parts != null && this.parts.length > 0) {      
    }
    else {
      this.getAllParts();
    }
  }


  getAllParts(){
    this.dataService.allParts()
      .subscribe(
        data => {          
          console.log(data);
          this.parts = data;
          this.localDataService.SetParts(data);
        },
        error => {
          console.log(error);      
      });
  }

  editPart(part) {
    
  }
  detailPart(part) {
    
  }

}
