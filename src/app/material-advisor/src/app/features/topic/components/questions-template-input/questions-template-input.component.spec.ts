import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionsTemplateInputComponent } from './questions-template-input.component';

describe('QuestionsTemplateInputComponent', () => {
  let component: QuestionsTemplateInputComponent;
  let fixture: ComponentFixture<QuestionsTemplateInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuestionsTemplateInputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuestionsTemplateInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
