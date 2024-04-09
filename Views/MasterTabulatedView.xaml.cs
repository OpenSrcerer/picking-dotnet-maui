using CommunityToolkit.Maui.Extensions;
using Microsoft.Extensions.Configuration;
using Project_CS412.Database.Models;
using Project_CS412.ViewModels;
using Syncfusion.Maui.DataGrid;

namespace Project_CS412.Views;

public partial class MasterTabulatedView : ITabulatedView
{
    public readonly MasterViewModel MasterViewModel;
    private readonly DetailViewModel _detailViewModel;

    public MasterTabulatedView(
        IConfiguration configuration,
        MasterViewModel masterViewModel,
        DetailViewModel detailViewModel
    )
    {
        MasterViewModel = masterViewModel;
        _detailViewModel = detailViewModel;

        var sfConfig = configuration.GetRequiredSection("Syncfusion").Get<SyncfusionConfig>();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(sfConfig.License);

        InitializeComponent();
        BindingContext = MasterViewModel;
    }

    public void OnSelect(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("detail");
        var selectedPicking = MasterViewModel.ItemPickings.ElementAt(dataGrid.SelectedIndex - 1);

        _detailViewModel.Refresh(selectedPicking);
        MasterViewModel.SelectedItemPicking = selectedPicking;
    }

    public void OnSearch(object sender, EventArgs e)
    {
        string filter = searchBar.Text;
        if (string.IsNullOrEmpty(filter))
        {
            dataGrid.View.Filter = null;
        }
        else
        {
            dataGrid.View.Filter = ip =>
            {
                var picking = ip as ItemPicking;
                return picking.Id.ToString().Contains(filter) ||
                       picking.ItemOrderId.ToString().Contains(filter) ||
                       picking.PickingStartDatetime.ToString().Contains(filter) ||
                       picking.PickingEndDatetime.ToString().Contains(filter);
            };
        }

        dataGrid.View.RefreshFilter();
    }

    public void OnCreate(object sender, EventArgs e)
    {
        MasterViewModel.Create();
    }

    public void OnDelete(object sender, EventArgs e)
    {
        MasterViewModel.Delete(dataGrid.SelectedIndex - 1);
        OnSelectionChanged(null, null);
    }

    public async void OnRefresh(object sender, EventArgs e)
    {
        bool accepted = await DisplayAlert(
            "Warning", "This will make you lose your changes. Are you sure you wish to proceed?", "Yes", "No");
        if (!accepted) return;

        await MasterViewModel.Refresh();
        DisplayAlert("Table", "Successfully reset!", "OK");
        OnSelectionChanged(null, null);
    }

    public async void OnSave(object sender, EventArgs e)
    {
        bool accepted = await DisplayAlert("Warning", "Are you sure you want to save your changes?", "Yes", "No");
        if (!accepted) return;

        try
        {
            await MasterViewModel.Save();
            DisplayAlert("Table", "Successfully saved!", "OK");
            OnSelectionChanged(null, null);
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message + ex.InnerException, "OK");
        }
    }

    public async void OnSelectionChanged(object sender, EventArgs e)
    {
        if (e == null)
        {
            await Task.WhenAll(
                DeleteButton.BackgroundColorTo(Color.FromArgb("#FAB096")),
                SelectButton.BackgroundColorTo(Color.FromArgb("#898CC7")));

            DeleteButton.IsEnabled = false;
            SelectButton.IsEnabled = false;
            return;
        }

        var args = e as DataGridSelectionChangedEventArgs;
        if (args.AddedRows.Count > 0)
        {
            await Task.WhenAll(
                DeleteButton.BackgroundColorTo(Color.FromArgb("#FA5D3B")),
                SelectButton.BackgroundColorTo(Color.FromArgb("#4668C7")));

            DeleteButton.IsEnabled = true;
            SelectButton.IsEnabled = true;
        }
    }
    
    // Lookups
    public void OnLookupItemOrderLine(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("lookupitemorderline");
    }
    
    public void OnLookupItemOrder(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("lookupitemorder");
    }
    
    public void OnLookupItemPkg(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("lookupitempkg");
    }
    
    public void OnLookupStorePos(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("lookupstorepos");
    }
    
    public void OnLookupStore(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("lookupstore");
    }
    
    public void OnLookupVatClasses(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("lookupvatclasses");
    }
}