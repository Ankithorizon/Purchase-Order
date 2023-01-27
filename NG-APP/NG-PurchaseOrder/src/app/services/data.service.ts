import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocalDataService } from '../services/local-data.service';
@Injectable({
  providedIn: 'root'
})
export class DataService {

  public PurchaseOrderApi = 'https://localhost:44366/api';
  public EngineeringApi =  `${this.PurchaseOrderApi}/engineering`;
  
  constructor(
    private http: HttpClient,
    public localDataService: LocalDataService)
  { }

  // engineering
  // partmaster, partdetail
  allParts(): Observable<Array<any>> {
    return this.http.get<Array<any>>(this.EngineeringApi + '/allParts');
  }

}
