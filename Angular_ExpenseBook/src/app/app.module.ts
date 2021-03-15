import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';

import { NavBarComponent } from './nav/navbar.component';
import { appRoutes } from './routes';
import { RouterModule } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreRouterConnectingModule } from '@ngrx/router-store';
import { BooksComponent } from './books/books.component';
import { FormsModule } from '@angular/forms';

import { HttpClientModule } from '@angular/common/http';
import { AddEditBookComponent } from './books/add-edit-book/add-edit-book.component';
import { EventsAppComponent } from './events-app.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DatePipe } from '@angular/common';


@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    FormsModule, 
    NgxPaginationModule
  ],
  declarations: [
    EventsAppComponent,
    NavBarComponent,
    BooksComponent,
    AddEditBookComponent,
  ],
  providers: [ DatePipe ], 
  bootstrap: [ EventsAppComponent ],
})
export class AppModule { }
