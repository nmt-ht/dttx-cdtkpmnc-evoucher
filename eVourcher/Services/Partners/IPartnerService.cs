using eVoucher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface IPartnerService
    {
        Task<IList<Partner>> GetPartners();
        Task<bool> CreatePartner(Partner Partner);
        Task<bool> UpdatePartner(Partner Partner);
        Task<bool> DeletePartner(Partner Partner);
    }
}
