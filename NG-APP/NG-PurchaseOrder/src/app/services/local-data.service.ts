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
}
