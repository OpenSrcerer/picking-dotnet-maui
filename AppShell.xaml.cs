using Project_CS412.Views;

namespace Project_CS412;

public partial class AppShell
{
    private Dictionary<string, Type> Routes { get; } = new();

    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
        BindingContext = this;
    }

    void RegisterRoutes()
    {
        Routes.Add("master", typeof(MasterTabulatedView));
        Routes.Add("detail", typeof(DetailTabulatedView));
        Routes.Add("easteregg", typeof(EasterEggView));
        Routes.Add("lookupitempkg", typeof(ItemPkgView));
        Routes.Add("lookupitemorder", typeof(ItemOrderView));
        Routes.Add("lookupitemorderline", typeof(ItemOrderLineView));
        Routes.Add("lookupstore", typeof(StoreView));
        Routes.Add("lookupstorepos", typeof(StorePosView));
        Routes.Add("lookupvatclasses", typeof(VatClassView));

        foreach (var item in Routes)
        {
            Routing.RegisterRoute(item.Key, item.Value);
        }
    }
}