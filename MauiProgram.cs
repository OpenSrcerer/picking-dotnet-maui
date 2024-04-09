using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Project_CS412.Database;
using Project_CS412.Database.Models;
using Project_CS412.ViewModels;
using Project_CS412.Views;
using Syncfusion.Maui.Core.Hosting;

namespace Project_CS412;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // Wait for promise to complete
        using var stream = FileSystem.OpenAppPackageFileAsync("AppSettings.json").Result;
        var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Aileron-Regular.otf", "AileronSansRegular");
                fonts.AddFont("Aileron-SemiBold.otf", "AileronSansSemibold");
            })
            .Configuration.AddConfiguration(config);

        // Register DI for DB
        builder.Services.AddSingleton<DatabaseFacade>();

        // DI for Models
        builder.Services.AddSingleton<MasterViewModel>();
        builder.Services.AddSingleton<DetailViewModel>();
        builder.Services.AddSingleton<ItemPkgViewModel>();
        builder.Services.AddSingleton<ItemOrderLineViewModel>();
        builder.Services.AddSingleton<ItemOrderViewModel>();
        builder.Services.AddSingleton<StorePosViewModel>();
        builder.Services.AddSingleton<StoreViewModel>();
        builder.Services.AddSingleton<VatClassViewModel>();

        // DI for Views
        builder.Services.AddTransient<MasterTabulatedView>();
        builder.Services.AddTransient<DetailTabulatedView>();
        builder.Services.AddTransient<EasterEggView>();
        builder.Services.AddTransient<ItemPkgView>();
        builder.Services.AddTransient<ItemOrderLineView>();
        builder.Services.AddTransient<ItemOrderView>();
        builder.Services.AddTransient<StorePosView>();
        builder.Services.AddTransient<StoreView>();
        builder.Services.AddTransient<VatClassView>();

        builder.ConfigureSyncfusionCore();
        return builder.Build();
    }
}