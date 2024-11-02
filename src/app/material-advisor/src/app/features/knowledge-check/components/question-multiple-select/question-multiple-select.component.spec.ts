import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionMultipleSelectComponent } from './question-multiple-select.component';

describe('QuestionMultipleSelectComponent', () => {
  let component: QuestionMultipleSelectComponent;
  let fixture: ComponentFixture<QuestionMultipleSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuestionMultipleSelectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuestionMultipleSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
