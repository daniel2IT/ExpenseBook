import { Routes } from '@angular/router'
import { BooksComponent } from './books/books.component';

export const appRoutes: Routes = [
    { path: 'expense', component: BooksComponent},
    { path: '', redirectTo: '/expense', pathMatch:'full'}
]