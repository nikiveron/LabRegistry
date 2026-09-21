using LabRegistry.Domain.Enums;
using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Database.Repositories;
using MediatR;

namespace LabRegistry.Application.InspectionObjects.UpdateInspectionObject;

public record UpdateInspectionObjectCommand(Guid Id, ProductResult? ProductResult, string? Comment) : IRequest;

public class UpdateInspectionObjectHandler(
    IInspectionObjectsRepository inspectionObjectsRepository
) : IRequestHandler<UpdateInspectionObjectCommand>
{
    public async Task Handle(UpdateInspectionObjectCommand request, CancellationToken cancellationToken)
    {
        _ = await inspectionObjectsRepository.Read(request.Id, cancellationToken)
            ?? throw new HttpErrorException(ExceptionMessagesConsts.InspectionObjectNotFound, System.Net.HttpStatusCode.NotFound);

        await inspectionObjectsRepository.Update(request.Id, request.ProductResult, request.Comment, cancellationToken);
    }
}
