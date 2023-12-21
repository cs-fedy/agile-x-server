using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Labels.Commands.DeleteLabel;

public abstract record DeleteLabelCommand(Guid LabelId, Guid UserId)
    : IRequest<Result<SuccessMessage>>;
