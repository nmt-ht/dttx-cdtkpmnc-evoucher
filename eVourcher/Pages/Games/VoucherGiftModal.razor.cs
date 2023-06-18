using Blazorise;
using Microsoft.AspNetCore.Components;
using System.IO;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Pages.Games;

public partial class VoucherGiftModal : ComponentBase
{
    private Modal modalRef;
    private string ImageLink { get; set; }

    public void InitVoucherGift(eVoucherType typeOfGift)
    {
        string path = $"images/voucher";
        switch (typeOfGift)
        {
            case eVoucherType.NetPrice:
                ImageLink = path + @"/voucherTwentyFivePercentage.png";
                break;
            case eVoucherType.TwentyFive:
                ImageLink = path + @"/voucherTwentyFivePercentageCyber.png";
                break;
            case eVoucherType.ThirtyFive:
                ImageLink = path + @"/voucherThirtyFivePercentageGold.png";
                break;
            case eVoucherType.Fifty:
                ImageLink = path + @"/voucherFiftyPercentageGold.png";
                break;
            case eVoucherType.OneHundered:
                break;
        }

        ShowModal();
    }

    private Task ShowModal()
    {
        return modalRef.Show();
    }

    private Task HideModal()
    {
        return modalRef.Hide();
    }
}