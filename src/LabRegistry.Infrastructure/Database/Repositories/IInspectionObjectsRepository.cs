using LabRegistry.Domain.Entities;
using LabRegistry.Domain.Enums;

namespace LabRegistry.Infrastructure.Database.Repositories;

public interface IInspectionObjectsRepository
{
    public Task<Guid> Create(InspectionObject inspectionObject, CancellationToken ct);
    public Task<InspectionObject?> Read(Guid id, CancellationToken ct);
    public Task Update(InspectionObject inspectionObject, CancellationToken ct);
    public Task Delete(Guid id, CancellationToken ct);
    public Task<List<InspectionObject>> ReadList(string? namePart, ProductType? productType, ProductResult? productResult, CancellationToken ct);
}
