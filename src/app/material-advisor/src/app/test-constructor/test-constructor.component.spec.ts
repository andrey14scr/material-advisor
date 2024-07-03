import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestConstructorComponent } from './test-constructor.component';

describe('TestConstructorComponent', () => {
  let component: TestConstructorComponent;
  let fixture: ComponentFixture<TestConstructorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestConstructorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestConstructorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
