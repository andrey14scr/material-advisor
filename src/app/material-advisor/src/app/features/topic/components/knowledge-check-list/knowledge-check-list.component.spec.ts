import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KnowledgeChecksComponent } from './knowledge-check-list.component';

describe('KnowledgeCheckComponent', () => {
  let component: KnowledgeChecksComponent;
  let fixture: ComponentFixture<KnowledgeChecksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [KnowledgeChecksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(KnowledgeChecksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
