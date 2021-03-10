import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http'
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  readonly APIUrl = "https://localhost:44388/api";
  readonly BooksUrl = "https://localhost:44388/api/books";

  constructor(private http:HttpClient) {}
    
    getEmployeeList():Observable<any[]>{
      return this.http.get<any>(this.APIUrl + '/employee');
    }

    getEmployerList():Observable<any[]>{
      return this.http.get<any>(this.APIUrl + '/employer');
    }

    getBooksList():Observable<any[]>{
      return this.http.get<any>(this.APIUrl + '/books');
    }

    addBook(val:any){
      return this.http.post(this.APIUrl+'/books',val);
    }
  
    updateBook(val:any){
      return this.http.put(this.APIUrl+'/books',val);
    }
  
    deleteBook(valNo:any){
      return this.http.delete(this.APIUrl+'/books/'+valNo);
    }
  

}