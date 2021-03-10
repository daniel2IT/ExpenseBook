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

  ModalTitle!: string;
  ActivateAddEditDepComp: boolean = false;
  dep:any;

  ngOnInit(): void {
    this.refreshBookList();
  }

  addClick(){
    this.dep={
      No:0,
      EmployeerName:"",
      EmployeeName:"",
      Project:"TestProjectName",
      Date:"1999/04/30",
      Spent:"40",
      VAT:"50",
      Total:"20",
      Comment:"TestComment"
    }
    this.ModalTitle="Add Book";
    this.ActivateAddEditDepComp=true;
  }

  editClick(item: any){
    this.dep = item;
    this.ModalTitle = "Edit Book";
    this.ActivateAddEditDepComp=true;
  }

  deleteClick(item: any){
    if(confirm("Are you sure?")){
      this.service.deleteBook(item.No).subscribe(data =>{
        alert(data.toString());
        this.refreshBookList();
      })
    }
  }

  closeClick(){
    this.ActivateAddEditDepComp = false;
    this.refreshBookList();
  }

  refreshBookList(){
    this.service.getBooksList().subscribe(data => {
        this.bookList = data;
    });
    }

}
