import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoanDto } from '../dtos/loan.dto';
import { environment } from '../../environments/environment';


@Injectable({ providedIn: 'root' })
export class LoansService {
  private readonly apiUrl = `${environment.loanApiBaseUrl}/loans`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<LoanDto[]> {
    return this.http.get<LoanDto[]>(this.apiUrl);
  }
}