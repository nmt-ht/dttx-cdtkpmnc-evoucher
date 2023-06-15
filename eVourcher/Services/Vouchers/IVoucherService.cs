using eVoucher.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVoucher.Services.Vouchers
{
    public interface IVoucherService
    {
        Task<IList<Voucher>> GetVouchers();
        Task<Voucher> GetVoucherById(Guid id);
        Task<bool> CreateVoucher(Voucher voucher);
                
    }
}
