import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnswersInputComponent } from './answers-input.component';

describe('AnswersInputComponent', () => {
  let component: AnswersInputComponent;
  let fixture: ComponentFixture<AnswersInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnswersInputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnswersInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
