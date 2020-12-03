using System.Collections.Generic;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Models;

namespace AdminProject.Services.Interface
{
    public interface IBasketService
    {
        void Delete(int basketId);
        void Delete(int userId, int basketId);
        void Edit(int id, Basket basketRequest);
        void EditUnitChange(int basketId, int unit);
        void Add(Basket basket);
        void AddOrChange(Basket request);
        BasketProductList GetUserBasketList(int userId);
        void SessionSetLoginUser(string sessionId, int userId);
        void EditUnitChange(int userId, int basketId, int unit);
        List<ProductDto> GetUserBasketList(BasketSearchDto request);
        BasketListDto GetTopBasketList(string sessionId, int userId);
        BasketProductList GetUserBasketList(string sessionId, int userId);
        void Delete(string sessionId, int basketId);
        void EditUnitChange(string sessionId, int basketId, int unit);
        void UserDeleteBaskets(int userId);
    }
}