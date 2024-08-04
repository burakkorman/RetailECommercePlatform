using System.Text.Json.Serialization;
using MediatR;

namespace RetailECommercePlatform.Data.RequestModels.Command.Product;

public class UpdateProductCommand : CreateProductCommand, IRequest<bool>
{
    [JsonIgnore] public string? Id { get; set; }
}