import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NavBarComponent } from './nav/navbar.component';
import { appRoutes } from './routes';
import { RouterModule } from '@angular/router';
import { ExpenseComponent } from './books/expense.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AddEditExpenseComponent } from './books/add-edit-expense/add-edit-expense.component';
import { EventsAppComponent } from './events-app.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { DatePipe } from '@angular/common';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    FormsModule, 
    NgxPaginationModule,
    MatDatepickerModule,
    BsDatepickerModule.forRoot(),
    BrowserAnimationsModule
  ],
  declarations: [
    EventsAppComponent,
    NavBarComponent,
    ExpenseComponent,
    AddEditExpenseComponent,
  ],
  providers: [ DatePipe ], 
  bootstrap: [ EventsAppComponent ],
})
export class AppModule { }
