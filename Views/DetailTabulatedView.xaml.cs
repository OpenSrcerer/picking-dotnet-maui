using CommunityToolkit.Maui.Extensions;
using Project_CS412.Database.Models;
using Project_CS412.ViewModels;
using Syncfusion.Maui.DataGrid;

namespace Project_CS412.Views;

public partial class DetailTabulatedView : ITabulatedView
{
    public readonly DetailViewModel DetailViewModel;

    public DetailTabulatedView(DetailViewModel detailViewModel)
    {
        DetailViewModel = detailViewModel;

        InitializeComponent();
        BindingContext = DetailViewModel;
    }

    public void OnSelect(object sender, EventArgs e)
    {
        throw new NotSupportedException();
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
                var pickingLine = ip as ItemPickingLine;
                return pickingLine.Id.ToString().Contains(filter) ||
                       pickingLine.ItemPickingId.ToString().Contains(filter) ||
                       pickingLine.ItemOrderLineId.ToString().Contains(filter) ||
                       pickingLine.ItemPkgId.ToString().Contains(filter) ||
                       pickingLine.StorePosCid.ToString().Contains(filter) ||
                       pickingLine.QtyInUnit.ToString().Contains(filter);
            };
        }

        dataGrid.View.RefreshFilter();
    }

    public void OnCreate(object sender, EventArgs e)
    {
        DetailViewModel.Create();
    }

    public void OnDelete(object sender, EventArgs e)
    {
        DetailViewModel.Delete(dataGrid.SelectedIndex - 1);
        OnSelectionChanged(null, null);
    }

    public async void OnRefresh(object sender, EventArgs e)
    {
        bool accepted = await DisplayAlert(
            "Warning", "This will make you lose your changes. Are you sure you wish to proceed?", "Yes", "No");
        if (!accepted) return;

        DetailViewModel.Refresh();
        DisplayAlert("Table", "Successfully reset!", "OK");
        OnSelectionChanged(null, null);
    }

    public async void OnSave(object sender, EventArgs e)
    {
        bool accepted = await DisplayAlert("Warning", "Are you sure you want to save your changes?", "Yes", "No");
        if (!accepted) return;

        try
        {
            await DetailViewModel.Save();
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
            await DeleteButton.BackgroundColorTo(Color.FromArgb("#FAB096"));
            DeleteButton.IsEnabled = false;
            return;
        }

        var args = e as DataGridSelectionChangedEventArgs;
        if (args.AddedRows.Count > 0)
        {
            await DeleteButton.BackgroundColorTo(Color.FromArgb("#FA5D3B"));
            DeleteButton.IsEnabled = true;
        }
    }

    public void OnEditCell(object sender, EventArgs e)
    {
        var args = e as DataGridCurrentCellEndEditEventArgs;

        // Filter only events for picked quantity
        if (args.RowColumnIndex.ColumnIndex != 6)
        {
            return;
        }

        try
        {
            DetailViewModel.OnPickingChange(
                args.RowColumnIndex.RowIndex - 1,
                Double.Parse(args.OldValue.ToString()) - Double.Parse(args.NewValue.ToString())
            );
        }
        catch (ArithmeticException ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
            args.Cancel = true;
        }
    }
}