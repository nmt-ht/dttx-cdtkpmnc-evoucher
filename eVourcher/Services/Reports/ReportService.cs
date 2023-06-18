using eVoucher.Handlers;
using eVoucher.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public class ReportService : IReportService
    {
        public ReportService() { }
        public async Task<IList<ReportCampaignByDateDto>> GetTotalOfCampaignByDate(ReportCampaignRequest reportCampaignRequest)
        {
            string requestURL = "/api/report/get-total-of-campaign-by-date";

            var response = await RestClient.APIClient.PostAsync(requestURL, reportCampaignRequest);

            if (response != null && response.Success && response.Data != null)
            {
                return JsonConvert.DeserializeObject<List<ReportCampaignByDateDto>>(response.Data.ToString());
            }
            return null;
        }

        public async Task<IList<VoucherReportDto>> GetTotalOfVouchers()
        {
            string requestURL = "/api/report/get-total-of-vouchers";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                return JsonConvert.DeserializeObject<List<VoucherReportDto>>(response.Data.ToString());
            }
            return null;
        }
    }
}
