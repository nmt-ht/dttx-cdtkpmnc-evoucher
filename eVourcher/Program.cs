using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Icons.Material;
using Blazorise.Material;
using Blazorise.RichTextEdit;
using eVoucher.Handlers;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eVoucher;
public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        builder.Services
                       .AddBlazorise(options =>
                       {
                           options.Immediate = true;
                       })
                       .AddMaterialProviders()
                       .AddMaterialIcons()
                       .AddFontAwesomeIcons()
                       .AddBlazoriseRichTextEdit(options => { options.UseShowTheme = true; });

        builder.Services.AddSingleton(provider =>
        {
            return provider.GetService<IConfiguration>();
        });
        builder.Configuration.Bind("eVoucherConfig", RestClient.VoucherConfig);

        builder.Services.AddScoped<User>();
        builder.Services.AddSingleton<ILocalStorage, LocalStorage>(); 
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<ICampaignService, CampaignService>();
        builder.Services.AddSingleton<IPartnerService, PartnerService>();
        builder.Services.AddSingleton<IGameService, GameService>();

        builder.Services.AddHttpClient("default", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        await builder.Build().RunAsync();
    }
}