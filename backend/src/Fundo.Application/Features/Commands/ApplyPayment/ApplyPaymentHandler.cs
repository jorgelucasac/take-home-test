using Fundo.Application.Features.Shared;
using Fundo.Application.Results;
using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
using Fundo.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fundo.Application.Features.Commands.ApplyPayment;

public class ApplyPaymentHandler : IRequestHandler<ApplyPaymentCommand, Result<LoanResponse>>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApplyPaymentHandler> _logger;

    public ApplyPaymentHandler(
        ILoanRepository loanRepository,
        IUnitOfWork unitOfWork,
        ILogger<ApplyPaymentHandler> logger)
    {
        _loanRepository = loanRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<LoanResponse>> Handle(ApplyPaymentCommand request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdForUpdateAsync(request.Id, cancellationToken);
        if (loan == null)
        {
            _logger.LogWarning("Loan with ID {LoanId} not found.", request.Id);
            return Result.Failure<LoanResponse>("Loan not found.");
        }

        var result = ApplyPayment(loan, request);

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to apply payment to Loan ID {LoanId}: {ErrorMessage}", request.Id, result.Error!.Message);
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
            return Result.Failure("Loan is already paid.");
        }

        if (request.Amount > loan.CurrentBalance)
        {
            return Result.Failure("Payment cannot be greater than current balance.");
        }

        loan.CurrentBalance -= request.Amount;

        if (loan.CurrentBalance == 0)
            loan.Status = LoanStatus.Paid;

        return Result.Success();
    }
}