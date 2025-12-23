import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { LoansService } from '../../../app/services/loans.service';
import { environment } from '../../../environments/environment';
import { LoanDto } from '../../../app/dtos/loan.dto';

describe('LoansService', () => {
  let service: LoansService;
  let httpMock: HttpTestingController;
  const apiUrl = `${environment.loanApiBaseUrl}/loans`;

  beforeEach(() => {
    TestBed.configureTestingModule({
        providers: [
          provideHttpClient(),
          provideHttpClientTesting()
        ],
    });

    service = TestBed.inject(LoansService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should retrieve all loans from the API', () => {
    const mockLoans: LoanDto[] = [
      {
        id: '1',
        amount: 5000,
        currentBalance: 2000,
        applicantName: 'Alice',
        status: 'ACTIVE',
      },
    ];

    service.getAll().subscribe((loans) => {
      expect(loans).toEqual(mockLoans);
    });

    const request = httpMock.expectOne(apiUrl);
    expect(request.request.method).toBe('GET');
    request.flush(mockLoans);
  });
});