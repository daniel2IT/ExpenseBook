import { Component, OnInit } from '@angular/core';
import { SharedService } from './shared/books.service'

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css']
})
export class BooksComponent implements OnInit {

  constructor(private service:SharedService) { }

  bookList: any = [];

  ngOnInit(): void {
    this.refreshBookList();
  }

  refreshBookList(){
    this.service.getBooksList().subscribe(data => {
        this.bookList = data;
    });

    }

}
