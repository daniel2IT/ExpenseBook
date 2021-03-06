import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { EventsAppComponent } from './events-app.component';
import { EventsListComponent } from './events/events-list.component';
import { EventThumbnaulComponent } from './events/event-thumbnail.component';
import { NavBarComponent } from './nav/navbar.component';
import {  EventService } from './events/shared/event.service'
import { ToastrService } from './common/toastr.service';
import { EventDetailsComponent } from './event-details/event-details.component';
import { appRoutes } from './routes';
import { RouterModule } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreRouterConnectingModule } from '@ngrx/router-store';
import { BooksComponent } from './books/books.component';

import { HttpClientModule } from '@angular/common/http';

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    StoreModule.forRoot({}),
    EffectsModule.forRoot([]),
    StoreRouterConnectingModule.forRoot()
  ],
  declarations: [
    EventsAppComponent,
    EventsListComponent,
    EventThumbnaulComponent,
    EventDetailsComponent,
    NavBarComponent,
    BooksComponent,
  ],
  providers: [EventService, ToastrService],
  bootstrap: [EventsAppComponent]
})
export class AppModule { }
