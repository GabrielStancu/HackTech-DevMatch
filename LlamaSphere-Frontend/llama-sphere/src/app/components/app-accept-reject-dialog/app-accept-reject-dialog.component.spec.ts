import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppAcceptRejectDialogComponent } from './app-accept-reject-dialog.component';

describe('AppAcceptRejectDialogComponent', () => {
  let component: AppAcceptRejectDialogComponent;
  let fixture: ComponentFixture<AppAcceptRejectDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AppAcceptRejectDialogComponent]
    });
    fixture = TestBed.createComponent(AppAcceptRejectDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
