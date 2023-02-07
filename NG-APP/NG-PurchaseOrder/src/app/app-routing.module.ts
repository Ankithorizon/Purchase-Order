import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


import { HomeComponent } from './home/home.component';
import { EngineeringComponent } from './engineering/engineering.component';
import { PartEditComponent } from './part-edit/part-edit.component';
import { PartCreateComponent } from './part-create/part-create.component';
import { WarehouseComponent } from './warehouse/warehouse.component';
import { OrderEditComponent } from './order-edit/order-edit.component';
import { OrderCreateComponent } from './order-create/order-create.component';
import { ReceivingComponent } from './receiving/receiving.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'engineering', component: EngineeringComponent },
  { path: 'part-edit', component: PartEditComponent },
  { path: 'part-create', component: PartCreateComponent },
  { path: 'warehouse', component: WarehouseComponent },
  { path: 'order-edit', component: OrderEditComponent },
  { path: 'order-create', component: OrderCreateComponent },
  { path: 'receiving', component: ReceivingComponent },
  { path: '**', redirectTo: '/home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
