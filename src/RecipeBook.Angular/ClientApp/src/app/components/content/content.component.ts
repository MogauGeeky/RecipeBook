import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-content',
  template: `
  <div class="container">
    <app-navigation></app-navigation>
    <div class="row">
        <div class="col-12 px-3">
          <router-outlet></router-outlet>
        </div>
    </div>
</div>`
})
export class ContentComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
