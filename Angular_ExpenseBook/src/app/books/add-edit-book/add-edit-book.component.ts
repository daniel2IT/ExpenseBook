import { Component, Input, OnInit  } from '@angular/core';
import { SharedService } from '../shared/books.service';

@Component({
  selector: 'app-add-edit-book',
  templateUrl: './add-edit-book.component.html',
})

export class AddEditBookComponent implements OnInit
{
  workerList: any = [];
  employeesFiltered: any = [];
  
  selectedEmployerGUID : any;

  @Input() dep:any;
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
   this.service.getWorkerList().subscribe(data => {
    this.workerList = data;
  });
}

onChangeEmployer(event: any)
{
  this.EmployerId = event.target.value;
  this.selectedEmployerGUID = this.EmployerId;
}

onChangeEmployee(event: any)
{
  this.EmployeeId = event.target.value;
  console.log("EmployeeID " + this.EmployeeId);
}

getEmployeeeList()
{
    this.employeesFiltered = this.workerList.filter((d: { EmployerRefId: string; })=>d.EmployerRefId===this.selectedEmployerGUID);
    return this.employeesFiltered;
}

  ngOnInit(): void
  {
    // Set Default Preview Side 
    this.EmployeeName = this.dep.EmployeeId;
    this.EmployerName = this.dep.EmployerId;
    this.No = this.dep.No;
    this.Project = this.dep.Project;
    this.Spent = this.dep.Spent;
    this.VAT = this.totalVAT(this.dep.Spent);
    this.Total = this.totalValue(this.dep.Spent);
    this.Comment = this.dep.Comment;

    // Default Preview AllEmployees For Selected Employer 
    this.selectedEmployerGUID = this.dep.EmployerId;
    
    // Set Real Default Data 
    this.EmployerId = this.dep.EmployerId;
    this.EmployeeId = this.dep.EmployeeId;
  }

  SpentValueChanged(newObj : any)
  {
    this.Total = this.totalValue(newObj);
    this.VAT = this.totalVAT(newObj);
  }
  
  totalValue(Spent: any)
  {
    var vat = this.Spent *  .21;
    return Number(this.Spent) + Number(vat);
  }

  totalVAT(Spent: any)
  {
    var vat = this.Spent *  .21;
    return vat;
  }

  addBook()
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

    this.service.addBook(val).subscribe(res=>
    {
      alert(res.toString());
    });
  }

  updateBook()
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

      this.service.updateBook(val).subscribe(res=>{
        alert(res.toString());
      });
  }
}
