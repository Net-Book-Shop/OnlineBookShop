using OnlineBookShop.Dto;
using OnlineBookShop.Model;

namespace OnlineBookShop.Service
{
    public interface IUserService
    {
        Task RegistorUser(UserRegistorRequestDTO userRegistor);
    }
}
