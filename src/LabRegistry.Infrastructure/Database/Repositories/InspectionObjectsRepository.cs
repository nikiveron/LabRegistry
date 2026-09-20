using LabRegistry.Domain.Entities;
using LabRegistry.Domain.Enums;
using LabRegistry.Infrastructure.Database.Context;
using LabRegistry.Infrastructure.Tools;
using Microsoft.EntityFrameworkCore;

namespace LabRegistry.Infrastructure.Database.Repositories;

public class InspectionObjectsRepository(AppDbContext appDbContext) : IInspectionObjectsRepository
{
    public async Task<Guid> Create(InspectionObject inspectionObject, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<InspectionObject?> Read(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<List<InspectionObject>> ReadList(string? namePart, ProductType? productType, ProductResult? productResult, CancellationToken ct)
    {
        var query = appDbContext.InspectionObjects.AsQueryable();
        return await QueryFilterBuilder(query, namePart, productType, productResult).ToListAsync(ct);
    }

    public async Task Update(InspectionObject inspectionObject, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    private static IQueryable<InspectionObject> QueryFilterBuilder(IQueryable<InspectionObject> query, string? namePart, ProductType? productType, ProductResult? productResult)
    {
        if (!string.IsNullOrEmpty(namePart))
        {
            query = query.Where(io => EF.Functions.ILike(io.Name, $"%{namePart}%"));
        }

        if (productType.HasValue)
        {
            query = query.Where(io => io.ProductType == EnumExtensions.GetEnumMemberValue(productType.Value));
        }

        if (productResult.HasValue)
        {
            query = query.Where(io => io.ProductResult == EnumExtensions.GetEnumMemberValue(productResult.Value));
        }

        return query;
    }
}
