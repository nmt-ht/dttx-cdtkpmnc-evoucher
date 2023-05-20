using eVoucher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface IPartnerService
    {
        Task<IList<Partner>> GetPartners();
        Task<bool> CreatePartner(Partner partner);
        Task<bool> UpdatePartner(Partner partner);
        Task<bool> DeletePartner(Partner partner);
    }
}
