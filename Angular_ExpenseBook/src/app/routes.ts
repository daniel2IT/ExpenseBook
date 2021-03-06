import { Routes } from '@angular/router'
import { BooksComponent } from './books/books.component';
import { EventDetailsComponent } from "./event-details/event-details.component";
import { EventsListComponent } from "./events/events-list.component";

export const appRoutes: Routes = [
    { path: 'events', component: EventsListComponent},
    { path: 'events/:id', component: EventDetailsComponent},
    { path: 'books', component: BooksComponent},
    { path: '', redirectTo: '/books', pathMatch:'full'}
]