import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportGenerationDialogComponent } from './report-generation-dialog.component';

describe('ReportGenerationDialogComponent', () => {
  let component: ReportGenerationDialogComponent;
  let fixture: ComponentFixture<ReportGenerationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReportGenerationDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReportGenerationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
