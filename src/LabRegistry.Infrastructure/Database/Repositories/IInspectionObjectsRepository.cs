using LabRegistry.Domain.Entities;
using LabRegistry.Domain.Enums;

namespace LabRegistry.Infrastructure.Database.Repositories;

public interface IInspectionObjectsRepository
{
    public Task<InspectionObject> Create(string name, string version, ProductType productType, DateTimeOffset recieptDate, string? comment, CancellationToken ct);
    public Task<InspectionObject?> Read(Guid id, CancellationToken ct);
    public Task Update(Guid id, ProductResult? productResult, string? comment, CancellationToken ct);
    public Task Delete(Guid id, CancellationToken ct);
    public Task<(List<InspectionObject> Items, int TotalCount)> ReadList(string? namePart, ProductType? productType, ProductResult? productResult, int page, int pageSize, CancellationToken ct);
}
