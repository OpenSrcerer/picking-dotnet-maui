using Project_CS412.ViewModels;

namespace Project_CS412.Views;

public partial class ItemOrderView : ITabulatedView
{
    public readonly ItemOrderViewModel ItemOrderViewModel;
    
    public ItemOrderView(ItemOrderViewModel viewModel)
    {
        InitializeComponent();
        
        ItemOrderViewModel = viewModel;
        BindingContext = ItemOrderViewModel;
    }

    public void OnSelect(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnSearch(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnCreate(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnDelete(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnRefresh(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnSave(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnSelectionChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}