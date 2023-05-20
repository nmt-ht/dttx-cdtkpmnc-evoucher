using eVoucher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface ICampaignService
    {
        Task<IList<Campaign>> GetCampaigns();
        Task<bool> CreateCampaign(Campaign Campaign);
        Task<bool> UpdateCampaign(Campaign Campaign);
        Task<bool> DeleteCampaign(Campaign Campaign);
    }
}
