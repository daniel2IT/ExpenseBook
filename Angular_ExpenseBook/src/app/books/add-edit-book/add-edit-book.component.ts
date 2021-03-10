import { Component, Input, OnInit } from '@angular/core';
import { SharedService } from '../shared/books.service';



@Component({
  selector: 'app-add-edit-book',
  templateUrl: './add-edit-book.component.html',
  styleUrls: ['./add-edit-book.component.css']
})
export class AddEditBookComponent implements OnInit {

  bookList: any = [];
  bookList2: any = [];

  constructor(private service:SharedService) {
      this.service.getEmployeeList().subscribe(data => {
        this.bookList = data;
    });
    this.service.getEmployerList().subscribe(data => {
      this.bookList2 = data;
  });
   }

  @Input() dep:any;
  No: string  | undefined;
  EmployerName: string  | undefined;
  EmployeeName: string  | undefined;
  Project: string  | undefined;
  Date: string  | undefined;
  Spent: string  | undefined;
  VAT: string  | undefined;
  Total: string  | undefined;
  Comment: string  | undefined;

  ngOnInit(): void {
    this.No = this.dep.No;
    this.EmployeeName = this.dep.EmployeerName;
    this.EmployeeName = this.dep.EmployeeName;
    this.Project = this.dep.Project;
    this.Date = this.dep.Date;
    this.Spent = this.dep.Spent;
    this.VAT = this.dep.VAT;
    this.Total = this.dep.Total;
    this.Comment = this.dep.Comment;
  }

  addBook(){
    var val = {
      No:this.No,
      EmployeerName:this.EmployerName,
      EmployeeName:this.EmployeeName,
      Project:this.Project,
      Date:this.Date,
      Spent:this.Spent,
      VAT:this.VAT,
      Total:this.Total,
      Comment:this.Comment};

              this.service.addBook(val).subscribe(res=>{
              alert(res.toString());
            });
  }

  updateBook(){
    var val = {
      No:this.No,
      EmployeerName:this.EmployerName,
      EmployeeName:this.EmployeeName,
      Project:this.Project,
      Date:this.Date,
      Spent:this.Spent,
      VAT:this.VAT,
      Total:this.Total,
      Comment:this.Comment};

      this.service.updateBook(val).subscribe(res=>{
        alert(res.toString());
      });
  }
}
