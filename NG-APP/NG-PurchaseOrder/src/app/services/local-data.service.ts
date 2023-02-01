import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalDataService {

  constructor() { }


  private parts = [];
  public GetParts() {
    return this.parts;
  }
  public SetParts(value) {
    this.parts = [...value];
  }


  // 400
  display400andEx(error, componentName): string[] {
    var errors = [];
    if (error != null) {
      for (var key in error) {
        errors.push(error[key]);
      }
    } else {
      errors.push('[' + componentName + '] Bad Request !');
    }
    return errors;
  }

}
