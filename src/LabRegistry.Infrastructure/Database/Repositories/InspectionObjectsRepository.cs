using LabRegistry.Domain.Entities;
using LabRegistry.Domain.Enums;
using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Database.Context;
using LabRegistry.Infrastructure.Tools;
using Microsoft.EntityFrameworkCore;

namespace LabRegistry.Infrastructure.Database.Repositories;

public class InspectionObjectsRepository(AppDbContext appDbContext) : IInspectionObjectsRepository
{
    public async Task<InspectionObject> Create(
        string name,
        string version,
        ProductType productType,
        DateTimeOffset recieptDate,
        string? comment,
        CancellationToken ct)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(version))
        {
            throw new ArgumentNullException(ExceptionMessagesConsts.InspectionObjectMustHaveNameAndVersion);
        }

        var newInspectionObject = new InspectionObject
        {
            Name = name,
            Version = version,
            ProductType = EnumExtensions.GetEnumMemberValue(productType),
            ReceiptDate = recieptDate,
            ProductResult = EnumExtensions.GetEnumMemberValue(ProductResult.InProgress),
            Comment = comment
        };

        appDbContext.InspectionObjects.Add(newInspectionObject);
        await appDbContext.SaveChangesAsync(ct);
        return newInspectionObject;
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<InspectionObject?> Read(Guid id, CancellationToken ct)
    {
        return await appDbContext.InspectionObjects.FirstOrDefaultAsync(io => io.Id == id, ct);
    }

    public async Task<List<InspectionObject>> ReadList(string? namePart, ProductType? productType, ProductResult? productResult, CancellationToken ct)
    {
        var query = appDbContext.InspectionObjects.AsQueryable();
        return await QueryFilterBuilder(query, namePart, productType, productResult).ToListAsync(ct);
    }

    public async Task Update(Guid id, ProductResult? productResult, string? comment, CancellationToken ct)
    {
        var inspectionObject = await appDbContext.InspectionObjects.FirstOrDefaultAsync(io => io.Id == id, ct);

        if (inspectionObject == null) return;

        if (productResult != null) inspectionObject.ProductResult = EnumExtensions.GetEnumMemberValue(productResult.Value);
        if (!string.IsNullOrEmpty(comment)) inspectionObject.Comment = comment;

        await appDbContext.SaveChangesAsync(ct);
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
