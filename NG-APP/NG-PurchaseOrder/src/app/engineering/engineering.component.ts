import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocalDataService } from '../services/local-data.service';

import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-engineering',
  templateUrl: './engineering.component.html',
  styleUrls: ['./engineering.component.css']
})
export class EngineeringComponent implements OnInit {

  closeResult: string = '';
  part;
  errorResponse;


  term: string;
  
  parts: Array<any>;
  
  page: number = 1;
  count: number = 0;
  tableSize: number = 10;
  tableSizes: any = [10,20,30];
  
  constructor(private modalService: NgbModal, public localDataService: LocalDataService, public dataService: DataService, private router: Router) { }

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

  // edit part
  editPart(selectedPart) {
     this.dataService.editPart(Number(selectedPart.partMasterId))
      .subscribe(
        data => {          
          console.log(data);            
          this.router.navigate(['/part-edit'], { state: { selectedPart: {data} } });          
        },
        error => {
          console.log(error);          
       
        });
  } 

  // view part detail
  // Modal
  open(content, selectedPart) {
    this.errorResponse = '';
    this.dataService.getPartDetails(Number(selectedPart.partMasterId))
      .subscribe(
        data => {          
          console.log(data);
          this.part = data;

          this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
            this.closeResult = `Closed with: ${result}`;
          }, (reason) => {
            this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
            console.log(this.closeResult);
          });
        },
        error => {
          console.log(error);             
          if (error.status === 400 || error.status===500) {
            this.part = null;
            this.errorResponse = error.error;
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

  // create new part
  createNewPart() {
    this.router.navigate(['/part-create']);
  }
}
