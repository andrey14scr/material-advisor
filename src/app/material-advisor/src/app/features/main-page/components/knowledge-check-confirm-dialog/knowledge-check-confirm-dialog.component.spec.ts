import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KnowledgeCheckConfirmDialogComponent } from './knowledge-check-confirm-dialog.component';

describe('KnowledgeCheckConfirmDialogComponent', () => {
  let component: KnowledgeCheckConfirmDialogComponent;
  let fixture: ComponentFixture<KnowledgeCheckConfirmDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [KnowledgeCheckConfirmDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(KnowledgeCheckConfirmDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
