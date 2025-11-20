import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'BuildingEntryRegistration.Web';

  constructor() {
    if (!localStorage.getItem('token')) {
      localStorage.setItem('token', 'dpworld');
    }
  }
}
