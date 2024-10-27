import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { SidebarOption } from 'src/app/models/sidebar-option.model';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavigationComponent {
  constructor(private router: Router) { }

  @Input() collapsed: boolean = false;
  public sidebarOptions: SidebarOption[] = [];

  ngOnInit(): void {
    this.initSidebarOptions();
  }

  initSidebarOptions(): void {
    const userType = localStorage.getItem('llm-userType');
    if (userType === 'developer') {
      this.initDeveloperSidebarMenu();
    } else {
      this.initRecruiterSidebarMenu();
    }
  }

  onOptionMouseOver(sidebarOption: SidebarOption): void {
    this.sidebarOptions.forEach(option => option.isActive = false);
    sidebarOption.isActive = true;
  }

  onOptionClick(sidebarOption: SidebarOption): void {
    if (sidebarOption.isLogOutOption) {
      localStorage.removeItem('llm-token');
      localStorage.removeItem('llm-id');
      localStorage.removeItem('llm-userType');
      localStorage.removeItem('llm-user');
      localStorage.removeItem('llm-firstName');
      localStorage.removeItem('llm-lastName');
    }
    this.router.navigate([sidebarOption.componentName]);
  }

  private initDeveloperSidebarMenu(): void {
    this.sidebarOptions = [
      new SidebarOption('NVision', 'logo-apple', '/developer'),
      new SidebarOption('Dashboard', 'home', '/developer', false, true),
      new SidebarOption('Account', 'person', '/'),
      new SidebarOption('Matchings', 'business_center', '/find-jobs'),
      new SidebarOption('Log Out', 'power_off', '/login', true)
    ];
  }

  private initRecruiterSidebarMenu(): void {
    this.sidebarOptions = [
      new SidebarOption('NVision', 'logo-apple', '/recruiter'),
      new SidebarOption('Dashboard', 'home', '/recruiter', false, true),
      new SidebarOption('Account', 'person', '/'),
      new SidebarOption('Matchings', 'business_center', '/find-devs'),
      new SidebarOption('Log Out', 'power_off', '/login', true)
    ];
  }
}
