import { Component, Input, EventEmitter, Output, ChangeDetectorRef } from '@angular/core';
import { faHome } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private cdref: ChangeDetectorRef) { }
  @Input() public iconLink = faHome

}
