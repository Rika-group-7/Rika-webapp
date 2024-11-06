using Microsoft.AspNetCore.Mvc;
using RikaWebApp.Models;
using RikaWebApp.Repository;

namespace RikaWebApp.Controller;

public class CartController(CartRepository cartRepository) : ICartRepositroy
{
    private readonly CartRepository _cartRepository = cartRepository;


    [HttpPost]
    public CartList Get()
    {
        throw new NotImplementedException();
    }

    public bool Save(CartList cartItem)
    {
        throw new NotImplementedException();
    }
}
