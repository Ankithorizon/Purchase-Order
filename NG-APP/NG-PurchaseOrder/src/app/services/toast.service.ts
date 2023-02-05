import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private toastr: ToastrService) { }
   
  showSuccess(message, title) {
    console.log('in the toaster', message, title);

    this.toastr.success(message, title)
  }
   
  showError(message, title) {
    console.log('in the toaster', message, title);
    this.toastr.error(message, title)
  }   
}
