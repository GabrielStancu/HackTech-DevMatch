import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public collapsedSidebar = true;
  public showNavbars = true;

  title = 'llama-sphere';

  constructor(private router: Router) {
    this.router.events.subscribe(_ => {
      const fullUrl = window.location;
      const notLogin = !fullUrl.href.includes('/login')
      const notRegister = !fullUrl.href.includes('/register')
      const notEmptyUrl = fullUrl.pathname !== '/';

      this.showNavbars = notLogin && notRegister && notEmptyUrl;
    });
  }

  onSidebarChanged(sidebarOption: {collapsed: boolean}): void {
    this.collapsedSidebar = sidebarOption.collapsed;
  }
}
