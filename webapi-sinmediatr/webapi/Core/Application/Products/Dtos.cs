namespace webapi.Core.Application.Products;

public record ProductDto(int Id, string Name, string Description);
public record CreateProductDto(string Name, string Description);
public record UpdateProductDto(int Id, string Name, string Description);