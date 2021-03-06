import { Component, OnInit } from '@angular/core'
import { EventService } from './shared/event.service';
import { ToastrService } from '../common/toastr.service'


@Component({
 template: `

<div>
  <h1>Upcoming Angular Events</h1>

  <hr/>
  <div class="row"> 
    <div *ngFor="let event of event1" class="col-md-5">
      <event-thumbnail (click)="handleThumbnailClick(event.name)" [eventOtherOne]="event"></event-thumbnail>
    </div>
  </div>
</div>
`
})

export class EventsListComponent implements OnInit{
  event1:any;
  constructor(private eventService: EventService, private toastr:
    ToastrService){
  }

  ngOnInit(){
    setTimeout(() => {
      this.event1 = this.eventService.getEvents();
    }, 1000)
  }

  handleThumbnailClick(eventName: any){
    this.toastr.success(eventName);
  }
}
