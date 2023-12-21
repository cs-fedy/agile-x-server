using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Labels.Queries.GetLabel;

public abstract record GetLabelQuery(Guid LabelId, Guid UserId) : IRequest<Result<Label>>;
