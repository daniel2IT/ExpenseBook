import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NavBarComponent } from './nav/navbar.component';
import { appRoutes } from './routes';
import { RouterModule } from '@angular/router';
import { BooksComponent } from './books/books.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AddEditBookComponent } from './books/add-edit-book/add-edit-book.component';
import { EventsAppComponent } from './events-app.component';
import { NgxPaginationModule } from 'ngx-pagination';
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
