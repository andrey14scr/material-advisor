import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnswerGroupsInputComponent } from './answer-groups-input.component';

describe('AnswerGroupsInputComponent', () => {
  let component: AnswerGroupsInputComponent;
  let fixture: ComponentFixture<AnswerGroupsInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnswerGroupsInputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnswerGroupsInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
