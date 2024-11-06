using RikaWebApp.Models;

namespace RikaWebApp.Repository;

public interface ICartRepositroy
{
    bool Save(CartList cartItem);
    CartList Get();
}
