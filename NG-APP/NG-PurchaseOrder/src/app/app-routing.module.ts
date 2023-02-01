import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


import { HomeComponent } from './home/home.component';
import { EngineeringComponent } from './engineering/engineering.component';
import { PartEditComponent } from './part-edit/part-edit.component';
import { PartCreateComponent } from './part-create/part-create.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'engineering', component: EngineeringComponent },
  { path: 'part-edit', component: PartEditComponent },
  { path: 'part-create', component: PartCreateComponent },
  { path: '**', redirectTo: '/home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
