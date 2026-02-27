namespace PurseAccounting.Mobile.Infrastructure.TransactionCategories;

public record TransactionCategoryDto
{
    public required long ID { get; init; }

    public required string Name { get; init; }

    public required bool IsActive { get; init; }

    public required bool IsDefault { get; init; }

    public byte ColorID { get; init; }
}
