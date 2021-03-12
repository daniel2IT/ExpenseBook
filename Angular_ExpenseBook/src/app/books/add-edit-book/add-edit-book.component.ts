import { Component, Input, OnInit } from '@angular/core';
import { SharedService } from '../shared/books.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-add-edit-book',
  templateUrl: './add-edit-book.component.html',
  styleUrls: ['./add-edit-book.component.css']
})
export class AddEditBookComponent implements OnInit {

  
  bookList: any = [];
  bookList2: any = [];
  datePipeString : string | any;

  @Input() dep:any;
  No: string  | any;
  EmployerName: string  | any;
  EmployeeName: string  | any;
  Project: string  | any;
  Date: string  | any;
  Spent: Number  | any;
  VAT: Number  | any;
  Total: Number  | any;
  Comment: string  | any;
  
  constructor(private service:SharedService, private datePipe: DatePipe) {
      this.service.getEmployeeList().subscribe(data => {
        this.bookList = data;
    });
    this.service.getEmployerList().subscribe(data => {
      this.bookList2 = data;
  });
  setInterval(() => {
  this.Date = datePipe.transform(Date.now(),'yyyy/MM/dd HH:mm:ss');
}, 1000);  
}


  ngOnInit(): void {
    this.No = this.dep.No;
    this.EmployeeName = this.dep.EmployeerName;
    this.EmployeeName = this.dep.EmployeeName;
    this.Project = this.dep.Project;
    this.Spent = this.dep.Spent;
    this.VAT = .21;
    this.Total = this.totalValue(this.dep.Spent);
    this.Comment = this.dep.Comment;
  }

  modelChanged(newObj : any) {
    this.Total = this.totalValue(newObj);
}

  
  totalValue(Spent: any){
    var vat = this.Spent *  .21;
    return Number(this.Spent) + Number(vat);
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
