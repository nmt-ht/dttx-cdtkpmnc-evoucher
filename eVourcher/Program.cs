using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Icons.Material;
using Blazorise.Material;
using Blazorise.RichTextEdit;
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

        builder.Services.AddSingleton<IAccountService, AccountService>();

        builder.Services.AddHttpClient("default", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        await builder.Build().RunAsync();
    }
}