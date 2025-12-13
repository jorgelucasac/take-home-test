using Fundo.Application.Handlers.Results;
using Fundo.Application.Handlers.Shared;
using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
using Fundo.Domain.Repositories;
using MediatR;

namespace Fundo.Application.Handlers.Commands.CreateLoan;

public class CreateLoanHandler : IRequestHandler<CreateLoanCommand, Result<LoanResponse>>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLoanHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoanResponse>> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = new Loan(request.Amount, request.CurrentBalance, request.ApplicantName, LoanStatus.Active);
        await _loanRepository.AddAsync(loan, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        var response = new LoanResponse(
            loan.Id,
            request.Amount,
            request.CurrentBalance,
            request.ApplicantName,
           nameof(LoanStatus.Active)
        );

        return Result.Success(response);
    }
}