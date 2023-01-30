import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocalDataService } from '../services/local-data.service';
import ResumeUpload from '../models/partEditDTO';
import PartEditDTO from '../models/partEditDTO';
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
  getPartDetails(selectedPartId: number): Observable<any> {
    return this.http.get<any>(this.EngineeringApi + '/getPartDetails/' + selectedPartId);
  }
  // get
  editPart(selectedPartId: number): Observable<any> {
    return this.http.get<any>(this.EngineeringApi + '/partEdit/' + selectedPartId);
  }
  // upload part file
  upload(partEditDto: PartEditDTO): Observable<HttpEvent<any>> {
    const formData: FormData = new FormData();
    // var formData: FormData = new FormData();
    formData.append('partFile', partEditDto.partFile);
    formData.append('partMasterId', partEditDto.partMasterId.toString());
    formData.append('partDetailId', partEditDto.partDetailId.toString());
    formData.append('partName', partEditDto.partName);
    formData.append('partCode', partEditDto.partCode);
    formData.append('partDesc', partEditDto.partDesc);
    
    // formData = null;
    
    // formData.append('jobApplicationId', "invalid-object-property");
    const req = new HttpRequest('POST', `${this.EngineeringApi}/upload`, formData, {
      reportProgress: true,
      responseType: 'json'
    });
    return this.http.request(req);
  }

}
