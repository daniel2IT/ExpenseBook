import { Component } from '@angular/core';

import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { of } from 'rxjs';

interface AppState { 
  message: string;
}

@Component({
  selector: 'events-app',
  template: `
  <nav-bar></nav-bar>
  <router-outlet></router-outlet>
`})


export class EventsAppComponent {
  title = 'BigPic';

  message$: any;

  constructor(private store: Store<AppState>){
     this.message$ = this.store.select('message')
  }

  lithuanianMessage(){
    this.store.dispatch({type: 'LITHUANIA'})
  }

  russianMessage(){
    this.store.dispatch({type: 'RUSSIA'})
  }
  
}

