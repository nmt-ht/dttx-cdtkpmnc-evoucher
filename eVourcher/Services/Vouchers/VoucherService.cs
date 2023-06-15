using eVoucher.Handlers;
using eVoucher.Models;
using eVoucher.Services.Vouchers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Voucher = eVoucher.Models.Voucher;

namespace eVourcher.Services
{
    public class VoucherService : IVoucherService
    {
        public VoucherService() { }

        public async Task<IList<Voucher>> GetVoucher()
        {
            IList<Voucher> vouchers = new List<Voucher>();

            string requestURL = "/api/vouchers";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                vouchers = JsonConvert.DeserializeObject<IList<Voucher>>(response.Data.ToString());
            }

            return vouchers;
        }
        public async Task<bool> CreateVoucher(Voucher voucher)
        {
            var result = false;
            string requestURL = "/api/vouchers/create";

            var response = await RestClient.APIClient.PostAsync(requestURL, voucher);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            }
            return result;
        }

        public async Task<Voucher> GetVoucherById(Guid id)
        {
            var vouchers = new Voucher();
            string requestURL = $"/api/vouchers/vouchers/{id}";
            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                vouchers = JsonConvert.DeserializeObject<Voucher>(response.Data.ToString());
            }

            return vouchers;
        }
        public async Task<IList<Voucher>> GetVouchers()
        {
            IList<Voucher> vouchers = new List<Voucher>();

            string requestURL = "/api/vouchers";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                vouchers = JsonConvert.DeserializeObject<IList<Voucher>>(response.Data.ToString());
            }

            return vouchers;
        }

        
    }
}
