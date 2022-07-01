using System.Security.AccessControl;

namespace ECommerce.Application.Features.Queries.ProductImageFile.GetProductImage;

public class GetProductImagesQueryResponse
{

    public string Path { get; set; }

    public string FileName { get; set; }
}