import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { AppComponent } from '../../app/app.component';
import { LoansService } from '../../app/services/loans.service';
import { LoanDto } from '../../app/dtos/loan.dto';

describe(AppComponent.name, () => {
  let fixture: ComponentFixture<AppComponent>;
  let component: AppComponent;
  let loansService: jasmine.SpyObj<LoansService>;

  beforeEach(async () => {
    loansService = jasmine.createSpyObj<LoansService>('LoansService', ['getAll']);

    await TestBed.configureTestingModule({
      imports: [AppComponent], // standalone component
      providers: [{ provide: LoansService, useValue: loansService }],
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
  });

  it('should load loans on init (success)', () => {
    const mockLoans: LoanDto[] = [
      {
        id: 'loan-123',
        amount: 25000,
        currentBalance: 18750,
        applicantName: 'John Doe',
        status: 'active',
      } as LoanDto,
    ];

    loansService.getAll.and.returnValue(of(mockLoans));

    expect(component.loading).toBeTrue();
    expect(component.error).toBeNull();
    expect(component.loans).toEqual([]);

    fixture.detectChanges(); // triggers ngOnInit

    expect(loansService.getAll).toHaveBeenCalledTimes(1);
    expect(component.loans).toEqual(mockLoans);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  });

  it('should set error message on init (failure)', () => {
    const consoleErrorSpy = spyOn(console, 'error');
    loansService.getAll.and.returnValue(
      throwError(() => new Error('Network error'))
    );

    fixture.detectChanges(); // triggers ngOnInit

    expect(loansService.getAll).toHaveBeenCalledTimes(1);
    expect(component.loans).toEqual([]);
    expect(component.loading).toBeFalse();
    expect(component.error).toBe('Failed to load loans (check CORS/HTTPS).');
    expect(consoleErrorSpy).toHaveBeenCalled();
  });
});
