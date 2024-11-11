using System.Text.Json;
using System.Text.Json.Serialization;

namespace RikaWebApp.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Metod för att hämta produktlistan från GraphQL-API:t
    public async Task<List<Product>> GetProductsAsync()
    {
        try
        {
            // Define the GraphQL query
            var query = new
            {
                query = @"
                query {
                    getProducts {
                        id
                        title
                        price
                        description
                        productImage
                        categories {
                            categoryName
                        }
                    }
                }",
                variables = new { }
            };

            // Send the request
            var response = await _httpClient.PostAsJsonAsync("https://productprovider-rika.azurewebsites.net/api/graphql?code=Da4aZa9Xnh8jmwu-Srgxk8wI7NpUBsKbMxQa9hoS0kp9AzFueYwEOg%3D%3D", query);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: Received {response.StatusCode} from GraphQL API.");
                return new List<Product>();
            }

            // Read the content as a string
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {content}"); // Log the response content

            // Deserialize the response
            var result = JsonSerializer.Deserialize<GraphQLResponse<ProductData>>(content);

            // Lägg till en loggning för att verifiera antalet produkter i svaret
            Console.WriteLine($"Antal produkter i API-svaret: {result?.Data?.GetProducts?.Count ?? 0}");

            if (result?.Data?.GetProducts == null)
            {
                Console.WriteLine("Warning: No products found in the response.");
                return new List<Product>();
            }

            // Return the list of products
            return result.Data.GetProducts;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching products: {ex.Message}");
            return new List<Product>();
        }
    }
}

public class GraphQLResponse<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}

public class ProductData
{
    [JsonPropertyName("getProducts")]
    public List<Product>? GetProducts { get; set; }
}

public class Product
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("productImage")]
    public string? ProductImage { get; set; }

    [JsonPropertyName("categories")]
    public List<Category>? Categories { get; set; }
}

public class Category
{
    [JsonPropertyName("categoryName")]
    public string? CategoryName { get; set; }
}