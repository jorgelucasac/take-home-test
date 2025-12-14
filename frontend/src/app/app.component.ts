import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { LoansService } from './services/loans.service';
import { LoanDto } from './dtos/loan.dto';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {

  loans: LoanDto[] = [];
  loading = true;
  error: string | null = null;

  constructor(private service: LoansService) { }
  ngOnInit(): void {
    this.service.getAll().subscribe({
      next: (data) => {
        this.loans = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Erro ao carregar loans (verifique CORS/HTTPS).';
        this.loading = false;
        console.error(err);
      },
    });
  }
  displayedColumns: string[] = [
    'loanAmount',
    'currentBalance',
    'applicant',
    'status',
  ];
}
