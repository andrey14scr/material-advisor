import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionSingleSelectComponent } from './question-single-select.component';

describe('QuestionSingleSelectComponent', () => {
  let component: QuestionSingleSelectComponent;
  let fixture: ComponentFixture<QuestionSingleSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuestionSingleSelectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuestionSingleSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
