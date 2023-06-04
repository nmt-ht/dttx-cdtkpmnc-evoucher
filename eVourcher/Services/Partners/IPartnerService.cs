using eVoucher.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface IPartnerService
    {
        Task<IList<Partner>> GetPartners();
        Task<Partner> GetPartnerById(Guid id);
        Task<bool> CreatePartner(Partner partner);
        Task<bool> UpdatePartner(Partner partner);        
        Task<bool> DeletePartner(Guid id);
    }
}
