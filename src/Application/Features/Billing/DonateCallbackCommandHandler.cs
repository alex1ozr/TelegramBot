using MediatR;
using TelegramBot.Application.Features.Billing.Invoices;

namespace TelegramBot.Application.Features.Billing;

internal sealed class DonateCallbackCommandHandler : IRequestHandler<DonateCallbackCommand, Unit>
{
    private readonly IMediator _mediator;

    public DonateCallbackCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Unit> Handle(DonateCallbackCommand request, CancellationToken cancellationToken)
    {
        var donationOption = BillingOptions.DonationOptions.FirstOrDefault(x => x.Name == request.Arguments.FirstOrDefault());
        if (donationOption is not null)
        {
            await _mediator.Send(new SendDonationInvoiceCommand(request.Message, donationOption), cancellationToken);
        }

        return Unit.Value;
    }
}
