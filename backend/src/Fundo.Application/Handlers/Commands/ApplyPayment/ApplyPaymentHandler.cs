using Fundo.Application.Handlers.Results;
using Fundo.Application.Handlers.Shared;
using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
using Fundo.Domain.Repositories;
using MediatR;

namespace Fundo.Application.Handlers.Commands.ApplyPayment;

public class ApplyPaymentHandler : IRequestHandler<ApplyPaymentCommand, Result<LoanResponse>>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApplyPaymentHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoanResponse>> Handle(ApplyPaymentCommand request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdForUpdateAsync(request.Id, cancellationToken);
        if (loan == null)
        {
            var error = Error.NotFound("Loan not found.");
            return Result.Failure<LoanResponse>(error);
        }

        var result = ApplyPayment(loan, request);

        if (result.IsFailure)
        {
            return Result.Failure<LoanResponse>(result.Error!);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
        var response = new LoanResponse(loan.Id, loan.Amount, loan.CurrentBalance, loan.ApplicantName, loan.Status.ToString());
        return Result.Success(response);
    }

    public static Result ApplyPayment(Loan loan, ApplyPaymentCommand request)
    {
        if (loan.Status == LoanStatus.Paid)
        {
            return Result.Failure("Loan is already paid.", ErrorType.Validation);
        }

        if (request.Amount > loan.CurrentBalance)
        {
            return Result.Failure("Payment cannot be greater than current balance.", ErrorType.Validation);
        }

        loan.CurrentBalance -= request.Amount;

        if (loan.CurrentBalance == 0)
            loan.Status = LoanStatus.Paid;

        return Result.Success();
    }
}