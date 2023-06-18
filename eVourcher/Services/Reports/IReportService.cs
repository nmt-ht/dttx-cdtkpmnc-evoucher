using eVoucher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface IReportService
    {
        Task<IList<ReportCampaignByDateDto>> GetTotalOfCampaignByDate(ReportCampaignRequest reportCampaignRequest);
        Task<IList<VoucherReportDto>> GetTotalOfVouchers();
    }
}
