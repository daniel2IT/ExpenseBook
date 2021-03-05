import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowAddEditBookComponent } from './show-add-edit-book.component';

describe('ShowAddEditBookComponent', () => {
  let component: ShowAddEditBookComponent;
  let fixture: ComponentFixture<ShowAddEditBookComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowAddEditBookComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowAddEditBookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
