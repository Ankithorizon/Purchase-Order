import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocalDataService } from '../services/local-data.service';
import ResumeUpload from '../models/partEditDTO';
import PartEditDTO from '../models/partEditDTO';
import PartCreateDTO from '../models/partCreateDTO';
@Injectable({
  providedIn: 'root'
})
export class DataService {

  public PurchaseOrderApi = 'https://localhost:44366/api';
  public EngineeringApi =  `${this.PurchaseOrderApi}/engineering`;
  public WarehouseApi =  `${this.PurchaseOrderApi}/warehouse`;
  
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
  // edit part
  // upload part file with edited part information
  partEditPost(partEditDto: PartEditDTO): Observable<HttpEvent<any>> {
    const formData: FormData = new FormData();
    // var formData: FormData = new FormData();
    formData.append('partFile', partEditDto.partFile);
    formData.append('partMasterId', partEditDto.partMasterId.toString());
    formData.append('partDetailId', partEditDto.partDetailId.toString());
    
    // model state check @ api
    formData.append('partName', partEditDto.partName);
    formData.append('partCode', partEditDto.partCode);    
    formData.append('partDesc', partEditDto.partDesc);
    
    // formData = null;
    
    // formData.append('jobApplicationId', "invalid-object-property");
    const req = new HttpRequest('POST', `${this.EngineeringApi}/partEditPost`, formData, {
      reportProgress: true,
      responseType: 'json'
    });
    return this.http.request(req);
  }
  // remote check for part-code
  remoteCheckPartCode(currentPartCode: string, newPartCode: string): Observable<any> {
    return this.http.get<any>(this.EngineeringApi + '/remoteCheckPartCode?previousPartCode=' + currentPartCode + '&partCode='+newPartCode);
  }
  // create part
  // upload part file with new part information
  partCreatePost(partCreateDto: PartCreateDTO): Observable<HttpEvent<any>> {
    const formData: FormData = new FormData();
    // var formData: FormData = new FormData();
    formData.append('partFile', partCreateDto.partFile);

    // model state check @ api
    formData.append('partName', partCreateDto.partName);
    formData.append('partCode', partCreateDto.partCode);
    formData.append('partDesc', partCreateDto.partDesc);
    formData.append('partDrgFile', partCreateDto.partDrgFile);
    
    // formData = null;
   
    const req = new HttpRequest('POST', `${this.EngineeringApi}/partCreatePost`, formData, {
      reportProgress: true,
      responseType: 'json'
    });
    return this.http.request(req);
  }


  // warehouse
  getWarehouseOrders(): Observable<Array<any>> {
    return this.http.get<Array<any>>(this.WarehouseApi + '/getWarehouseOrders');
  }
  searchParts(searchString): Observable<Array<any>> {
    return this.http.get<Array<any>>(this.WarehouseApi + '/getWarehouseOrders?searchString='+searchString);
  }
  searchAndOrderByParts(sortOrder,searchString): Observable<Array<any>> {
    return this.http.get<Array<any>>(this.WarehouseApi + '/getWarehouseOrders?sortOrder='+sortOrder+'&searchString='+searchString);
  }
  getOrderDetails(selectedOrderId: number): Observable<any> {
    return this.http.get<any>(this.WarehouseApi + '/getOrderDetails/' + selectedOrderId);
  }
  // get
  editOrder(selectedOrderId: number): Observable<any> {
    return this.http.get<any>(this.WarehouseApi + '/orderEdit/' + selectedOrderId);
  }
}
