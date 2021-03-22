import { Component, Input, OnInit } from '@angular/core';
import { SharedService } from '../shared/books.service';
import { DatePipe } from '@angular/common';


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

  constructor(private service:SharedService, private datePipe: DatePipe)
  {
   this.service.getWorkerList().subscribe(data => {
    this.workerList = data;
  });

  setInterval(() => {
  this.Date = datePipe.transform(Date.now(),'yyyy/MM/dd HH:mm:ss');
}, 1000);  
}


getEmployeeeList()
{ 
  this.employeesFiltered = this.workerList.filter((d: { EmployerRefId: string; })=>d.EmployerRefId===this.selectedEmployerGUID);
  return this.employeesFiltered;
}

onChangeEmployer(event: any)
{
  this.EmployerId = this.workerList[event.target.value].EmployerId;
  console.log("EmployerID " + this.EmployerId);

  this.selectedEmployerGUID = this.workerList[event.target.value].EmployerId;
}

onChangeEmployee(event: any)
{
  this.EmployeeId = this.employeesFiltered[event.target.value].EmployeeId;
  console.log("EmployeeIndex " + this.EmployeeId); // 0
}

  ngOnInit(): void
  {
    this.No = this.dep.No;
    this.Project = this.dep.Project;
    this.Spent = this.dep.Spent;
    this.VAT = 21;
    this.Total = this.totalValue(this.dep.Spent);
    this.Comment = this.dep.Comment;
  }

  modelChanged(newObj : any)
  {
    this.Total = this.totalValue(newObj);
  }
  
  totalValue(Spent: any)
  {
    var vat = this.Spent *  .21;
    return Number(this.Spent) + Number(vat);
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
