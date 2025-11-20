import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CheckInComponent } from './check-in/check-in.component';

const routes: Routes = [
  { path: 'checkin', component: CheckInComponent },
  { path: '', redirectTo: 'checkin', pathMatch: 'full' },
  { path: '**', redirectTo: 'checkin' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }