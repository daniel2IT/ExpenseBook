import { Component, OnInit } from '@angular/core';
import { SharedService } from './shared/expense.service'

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html'
})

export class BooksComponent implements OnInit 
{

  constructor(private service:SharedService){}

  expenseList: any = [];
  ModalTitle!: string;
  ActivateAddEditComp: boolean = false;
  expenseData:any;
  page: any;
  
  ngOnInit(): void
  {
    this.refreshBookList();
  }

  addClick()
  {
    this.expenseData =
    {
      No:0,
      Project:"TestProjectName",
      Spent:"40",
      Comment:"TestComment"
    }
    
    this.ModalTitle="Add Book";
    this.ActivateAddEditComp=true;
  }

  editClick(item: any)
  {
    this.expenseData = item;
    this.ModalTitle = "Edit Book";
    this.ActivateAddEditComp=true;
  }

  deleteClick(item: any)
  {
    if(confirm("Are you sure?"))
    {
      this.service.deleteExpense(item.No).subscribe(data =>
      {
        alert(data.toString());
        this.refreshBookList();
      })
    }
  }

  closeClick(){
    this.ActivateAddEditComp = false;
    this.refreshBookList();
  }

  refreshBookList()
  {
    this.service.getExpenseList().subscribe(data => 
    {
        this.expenseList = data;
    });
  }
}
