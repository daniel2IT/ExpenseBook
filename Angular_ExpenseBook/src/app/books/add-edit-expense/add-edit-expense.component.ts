import { Component, Input, OnInit  } from '@angular/core';
import { SharedService } from '../shared/expense.service';

@Component({
  selector: 'app-add-edit-expense',
  templateUrl: './add-edit-expense.component.html'
})

export class AddEditExpenseComponent implements OnInit
{
  workerList: any = [];
  employeesFiltered: any = [];
  
  @Input() expense:any;
  No: string  | any;
  EmployerName: string  | any;
  EmployerId : string | any;
  EmployeeName: string  | any;
  EmployeeId: string | any;
  Project: string  | any;
  Date: Date  | any;
  Spent: Number  | any;
  VAT: Number  | any;
  Total: Number  | any;
  Comment: string  | any;

  constructor(private service:SharedService)
  {
    this.service.getWorkerList().subscribe(data =>
    {
      this.workerList = data;
    });
  }

  onChangeEmployer(event: any)
  {
    this.EmployerId = event.target.value;
    console.log("EmployerID " + this.EmployerId);
  }

  onChangeEmployee(event: any)
  {
    this.EmployeeId = event.target.value;
    console.log("EmployeeID " + this.EmployeeId);
  }

  getEmployeeeList()
  {
    this.employeesFiltered = this.workerList.filter((d: { EmployerRefId: string; })=>d.EmployerRefId===this.EmployerId);
    return this.employeesFiltered;
  }

  ngOnInit(): void
  {
    // Set Default Preview Side 
    this.EmployeeName = this.expense.EmployeeId;
    this.EmployerName = this.expense.EmployerId;
    this.No = this.expense.No;
    this.Project = this.expense.Project;
    this.Spent = this.expense.Spent;
    this.VAT = this.totalVAT(this.expense.Spent);
    this.Total = this.totalSpent(this.expense.Spent);
    this.Comment = this.expense.Comment;
    this.Date = this.expense.Date;

    // Set Real Default Data 
    this.EmployerId = this.expense.EmployerId;
    this.EmployeeId = this.expense.EmployeeId;

    // Just For Test:
    console.log("EmployeeID " + this.EmployeeId);
    console.log("EmployerID " + this.EmployerId);

  }

  SpentValueChanged(newObj : any)
  {
    this.Total = this.totalSpent(newObj);
    this.VAT = this.totalVAT(newObj);
  }
  
  totalSpent(Spent: any)
  {
    var vat = this.Spent *  .21;
    return Number(this.Spent) + Number(vat);
  }

  totalVAT(Spent: any)
  {
    var vat = this.Spent *  .21;
    return vat;
  }

  addExpense()
  {
    var val = 
    {
      No:this.No,
      EmployerName:this.EmployerName,
      EmployerId:this.EmployerId,
      EmployeeName:this.EmployeeName,
      EmployeeId:this.EmployeeId,
      Project:this.Project,
      Date:this.Date,
      Spent:this.Spent,
      VAT:this.VAT,
      Total:this.Total,
      Comment:this.Comment
    };

    this.service.addExpense(val).subscribe(res=>
    {
      alert(res.toString());
    });
  }

  updateExpense()
  {
    var val = 
    {
      No:this.No,
      EmployerName:this.EmployerName,
      EmployerId:this.EmployerId,
      EmployeeName:this.EmployeeName,
      EmployeeId:this.EmployeeId,
      Project:this.Project,
      Date:this.Date,
      Spent:this.Spent,
      VAT:this.VAT,
      Total:this.Total,
      Comment:this.Comment
    };

      this.service.updateExpense(val).subscribe(res=>{
        alert(res.toString());
      });
  }
}
