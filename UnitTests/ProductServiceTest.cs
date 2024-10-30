using RikaWebApp.Services;

namespace UnitTests;

public class ProductServiceTest
{
    [Fact]
    public async Task GetProductsAsync_ShouldReturnCorrectNumberOfProducts()
    {
        // Arrange
        var productService = new ProductService();

        // Act
        var products = await productService.GetProductsAsync();

        // Assert
        Assert.Equal(3, products.Count);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnProductsWithUniqueIds()
    {
        // Arrange
        var productService = new ProductService();

        // Act
        var products = await productService.GetProductsAsync();
        var productIds = products.Select(p => p.Id);

        // Assert
        Assert.Equal(productIds.Count(), productIds.Distinct().Count());
    }

    [Fact]
    public async Task GetProductAsync_ShouldReturnProductWithCorrectData()
    {
        //Arrange
        var productService = new ProductService();

        // Act
        var products = await productService.GetProductsAsync();

        // Assert

        Assert.Collection(products, product =>
        {
            Assert.Equal(1, product.Id);
            Assert.Equal("Nike T-shirt", product.Title);
            Assert.Equal(100, product.Price);
            Assert.Equal("This is an Ingress", product.Ingress);
            Assert.Equal("/images/Nike-T-shirt.jpg", product.ImageUrl);
            Assert.Equal("Nike Sportswear", product.Category);
        },
        product =>
        {
            Assert.Equal(2, product.Id);
            Assert.Equal("AIR FORCE 1", product.Title);
            Assert.Equal(200, product.Price);
            Assert.Equal("This is an Ingress", product.Ingress);
            Assert.Equal("/images/AIR-FORCE-1.jpg", product.ImageUrl);
            Assert.Equal("Nike Sportswear", product.Category);
        },
        product =>
        {
            Assert.Equal(3, product.Id);
            Assert.Equal("HANS T-shirt", product.Title);
            Assert.Equal(999, product.Price);
            Assert.Equal("This is an Ingress", product.Ingress);
            Assert.Equal("/images/HANS-T-shirt.jpg", product.ImageUrl);
            Assert.Equal("Hans Sportswear", product.Category);
        }
        );
    }

    [Fact]
    public async Task GetProductsAsync_EachProductShouldHaveRequiredProperties()
    {
        // Arrange
        var productService = new ProductService();

        // Act

        var products = await productService.GetProductsAsync();

        // Assert

        foreach (var product in products)
        {
            Assert.False(string.IsNullOrEmpty(product.Title));
            Assert.True(product.Price > 0); // Price posivtive because 0
            Assert.False(string.IsNullOrEmpty(product.ImageUrl));
            Assert.False(string.IsNullOrEmpty(product.Category));
        }
    }
}
