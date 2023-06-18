using Blazorise.Charts;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVoucher.Pages;

public partial class Dashboard : ComponentBase
{
    [Inject] public IReportService ReportService { get; set; }
    private DateTime? CampaignCreatedFrom { get; set; }
    private DateTime? CampaignCreatedTo { get; set; }
    Chart<int> barChart;
    Chart<int> voucherReportChart;
    public IList<ReportCampaignByDateDto> ReportCampaignByDateDtos { get; set; } =new List<ReportCampaignByDateDto>();
    public IList<VoucherReportDto> VoucherReportDtos { get; set; } = new List<VoucherReportDto>();
    protected override async Task OnInitializedAsync()
    {
        await BindData();
        base.OnInitialized();
    }

    public async Task BindData()
    {
        ReportCampaignByDateDtos = await ReportService.GetTotalOfCampaignByDate(new ReportCampaignRequest
        {
            CreatedDateFrom = CampaignCreatedFrom,
            CreatedDateTo = CampaignCreatedTo,
        });

        VoucherReportDtos = await ReportService.GetTotalOfVouchers();

        await HandleReportCampaignRedraw();
        await HandleReportVoucherRedraw();

        StateHasChanged();
    }

    private async Task OnCampaignReportChanged()
    {
        await BindData();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await HandleReportCampaignRedraw();
            await HandleReportVoucherRedraw();
        }
    }

    #region Campaign Report
    async Task HandleReportCampaignRedraw()
    {
        await barChart.Clear();
        await barChart.AddLabelsDatasetsAndUpdate(ReportCampaignByDateDtos.Select(x => x.CreatedDate.ToShortDateString()).ToArray(), GetBarChartDatasetCampaignReport());
        StateHasChanged();
    }

    private BarChartDataset<int> GetBarChartDatasetCampaignReport()
    {
        return new()
        {
            Label = "# of campaigns",
            Data = ReportCampaignByDateDtos.Select(x => x.TotalCampaign).ToList(),
            BackgroundColor = backgroundColors,
            BorderColor = borderColors,
            BorderWidth = 1
        };
    }
    #endregion

    #region Voucher Report
    async Task HandleReportVoucherRedraw()
    {
        await voucherReportChart.Clear();
        await voucherReportChart.AddLabelsDatasetsAndUpdate(VoucherReportDtos.Select(x => x.Name).ToArray(), GetBarChartDatasetVoucherReport());
        StateHasChanged();
    }

    private BarChartDataset<int> GetBarChartDatasetVoucherReport()
    {
        return new()
        {
            Label = "# of vouchers",
            Data = VoucherReportDtos.Select(x => x.Total).ToList(),
            BackgroundColor = backgroundColors,
            BorderColor = borderColors,
            BorderWidth = 1
        };
    }
    #endregion

    List<string> backgroundColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f), ChartColor.FromRgba(54, 162, 235, 0.2f), ChartColor.FromRgba(255, 206, 86, 0.2f), ChartColor.FromRgba(75, 192, 192, 0.2f), ChartColor.FromRgba(153, 102, 255, 0.2f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
    List<string> borderColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };

    Task OnClicked(ChartMouseEventArgs e)
    {
        var model = e.Model as BarChartModel;
        return Task.CompletedTask;
    }
}