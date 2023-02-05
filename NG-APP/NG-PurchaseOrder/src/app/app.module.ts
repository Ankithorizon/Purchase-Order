import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';
import { Ng2SearchPipeModule } from 'ng2-search-filter';

import { ToastrModule } from 'ngx-toastr';

// services
import { DataService } from './services/data.service';
import { LocalDataService } from './services/local-data.service';
import { ToastService } from './services/toast.service';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { HeaderComponent } from './header/header.component';
import { EngineeringComponent } from './engineering/engineering.component';
import { PartEditComponent } from './part-edit/part-edit.component';
import { PartCreateComponent } from './part-create/part-create.component';
import { WarehouseComponent } from './warehouse/warehouse.component';
import { OrderEditComponent } from './order-edit/order-edit.component';



@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    HeaderComponent,
    EngineeringComponent,
    PartEditComponent,
    PartCreateComponent,
    WarehouseComponent,
    OrderEditComponent,
  ],
  imports: [
    BrowserModule,
    NgbModule,
    AppRoutingModule,
    FormsModule,    
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxPaginationModule,
    Ng2SearchPipeModule,
    ToastrModule.forRoot({
      timeOut: 5000, // 5 seconds
      closeButton: true,
      progressBar: true,
    }),
  ],
  providers: [LocalDataService, DataService, ToastService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
