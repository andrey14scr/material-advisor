import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnswersVerificationComponent } from './answers-verification.component';

describe('AnswersVerificationComponent', () => {
  let component: AnswersVerificationComponent;
  let fixture: ComponentFixture<AnswersVerificationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnswersVerificationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnswersVerificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
