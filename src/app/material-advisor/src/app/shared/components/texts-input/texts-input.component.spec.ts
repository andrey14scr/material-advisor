import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextsInputComponent } from './texts-input.component';

describe('TextsInputComponent', () => {
  let component: TextsInputComponent;
  let fixture: ComponentFixture<TextsInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TextsInputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TextsInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
