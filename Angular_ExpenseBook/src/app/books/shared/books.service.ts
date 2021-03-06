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

    getBooksList():Observable<any[]>{
      return this.http.get<any>(this.APIUrl + '/books');
    }


}