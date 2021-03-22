import { Routes } from '@angular/router'
import { ExpenseComponent } from './books/expense.component';

export const appRoutes: Routes = [
    { path: 'expense', component: ExpenseComponent},
    { path: '', redirectTo: '/expense', pathMatch:'full'}
]