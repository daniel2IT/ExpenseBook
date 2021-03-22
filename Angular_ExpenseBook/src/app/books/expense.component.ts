import { Component, OnInit } from '@angular/core';
import { SharedService } from './shared/expense.service'

@Component({
  selector: 'app-expense',
  templateUrl: './expense.component.html'
})

export class ExpenseComponent implements OnInit 
{

  constructor(private service:SharedService){}

  expenseList: any = [];
  ModalTitle!: string;
  ActivateAddEditComp: boolean = false;
  expenseData:any;
  page: any;
  
  ngOnInit(): void
  {
    this.refreshExpenseList();
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
    
    this.ModalTitle="Add Expense";
    this.ActivateAddEditComp=true;
  }

  editClick(item: any)
  {
    this.expenseData = item;
    this.ModalTitle = "Edit Expense";
    this.ActivateAddEditComp=true;
  }

  deleteClick(item: any)
  {
    if(confirm("Are you sure?"))
    {
      this.service.deleteExpense(item.No).subscribe(data =>
      {
        alert(data.toString());
        this.refreshExpenseList();
      })
    }
  }

  closeClick()
  {
    this.ActivateAddEditComp = false;
    this.refreshExpenseList();
  }

  refreshExpenseList()
  {
    this.service.getExpenseList().subscribe(data => 
    {
        this.expenseList = data;
    });
  }
}
