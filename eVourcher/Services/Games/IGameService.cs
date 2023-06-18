using eVoucher.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface IGameService
    {
        Task<IList<Game>> GetGames();
        Task<bool> CreateGame(Game game);
        Task<bool> UpdateGame(Game game);        
        Task<bool> DeleteGame(Guid id);
        Task<bool> CreateVoucher(Voucher voucher);
    }
}
