namespace Catalogo.Api.Controllers.Products;

public sealed record ProductRequest(string Nombre,decimal Precio,string Descripcion,Guid CategoryId);