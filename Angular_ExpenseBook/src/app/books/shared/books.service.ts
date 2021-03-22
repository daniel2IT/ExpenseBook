import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  readonly APIUrl = "https://localhost:44388/api";

  constructor(private http:HttpClient) {}
    
    getWorkerList():Observable<any[]>
    {
      return this.http.get<any>(this.APIUrl + '/worker');
    }

    getExpenseList():Observable<any[]>
    {
      return this.http.get<any>(this.APIUrl + '/expense');
    }

    addExpense(val:any)
    {
      return this.http.post(this.APIUrl+'/expense',val);
    }
  
    updateExpense(val:any)
    {
      return this.http.put(this.APIUrl+'/expense',val);
    }
  
    deleteExpense(valNo:any)
    {
      return this.http.delete(this.APIUrl+'/expense/'+ valNo);
    }
}