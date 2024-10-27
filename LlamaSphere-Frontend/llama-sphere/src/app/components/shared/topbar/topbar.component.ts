import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UserDisplayData } from 'src/app/models/user-display.model';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.css']
})
export class TopbarComponent implements OnInit{
  public collapsedSidebar = true;
  @Output() sidebarChanged = new EventEmitter<{collapsed: boolean}>();
  public displayUser?: UserDisplayData;

  ngOnInit(): void {
    const userName = localStorage.getItem("llm-firstName") ?? 'User';
    this.displayUser = new UserDisplayData(userName, "./../../../../assets/profile.png");
  }

  toggleSidebar(): void {
    this.collapsedSidebar = !this.collapsedSidebar;
    this.sidebarChanged.emit({
      collapsed: this.collapsedSidebar
    });
  }
}
