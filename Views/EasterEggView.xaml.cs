namespace Project_CS412.Views;

public partial class EasterEggView
{
    public EasterEggView()
    {
        InitializeComponent();
    }

    private async void GoBack_OnClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }
}