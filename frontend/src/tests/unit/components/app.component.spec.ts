import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { By } from '@angular/platform-browser';
import { AppComponent } from '../../../app/app.component';
import { LoansService } from '../../../app/services/loans.service';
import { LoanDto } from '../../../app/dtos/loan.dto';

describe(AppComponent.name, () => {
    let fixture: ComponentFixture<AppComponent>;
    let component: AppComponent;
    let loansService: jasmine.SpyObj<LoansService>;

    beforeEach(async () => {
        loansService = jasmine.createSpyObj<LoansService>('LoansService', ['getAll']);

        await TestBed.configureTestingModule({
            imports: [AppComponent],
            providers: [{ provide: LoansService, useValue: loansService }],
        }).compileComponents();

        fixture = TestBed.createComponent(AppComponent);
        component = fixture.componentInstance;
    });

    it('should load loans on init (success)', () => {
        const mockLoans: LoanDto[] = [
            {
                id: 1,
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

        fixture.detectChanges();

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

        fixture.detectChanges();

        expect(loansService.getAll).toHaveBeenCalledTimes(1);
        expect(component.loans).toEqual([]);
        expect(component.loading).toBeFalse();
        expect(component.error).toBe('Failed to load loans.');
        expect(consoleErrorSpy).toHaveBeenCalled();
    });

    it('should render table and rows when loaded successfully', () => {
        const mockLoans: LoanDto[] = [
            {
                amount: 25000,
                currentBalance: 18750,
                applicantName: 'John Doe',
                status: 'active',
            } as LoanDto,
            {
                amount: 15000,
                currentBalance: 0,
                applicantName: 'Jane Smith',
                status: 'paid',
            } as LoanDto,
        ];

        loansService.getAll.and.returnValue(of(mockLoans));

        fixture.detectChanges();

        // loading and error messages should be gone
        const text = fixture.nativeElement.textContent as string;
        expect(text).not.toContain('loading...');
        expect(text).toContain('Loan Management');

        // table exists
        const table = fixture.debugElement.query(By.css('table[mat-table]'));
        expect(table).not.toBeNull();

        // should render 2 data rows (mat-row)
        const rows = fixture.debugElement.queryAll(By.css('tr.mat-mdc-row, tr.mat-row'));
        expect(rows.length).toBe(2);

        // should show applicant names in the DOM
        expect(text).toContain('John Doe');
        expect(text).toContain('Jane Smith');
    });

    it('should show error message and hide table when service fails', () => {
        const consoleErrorSpy = spyOn(console, 'error');
        loansService.getAll.and.returnValue(throwError(() => new Error('Network error')));

        fixture.detectChanges();

        const text = fixture.nativeElement.textContent as string;
        expect(text).not.toContain('loading...');
        expect(text).toContain('Failed to load loans.');

        // table should NOT exist
        const table = fixture.debugElement.query(By.css('table[mat-table]'));
        expect(table).toBeNull();

        expect(consoleErrorSpy).toHaveBeenCalled();
    });
});
