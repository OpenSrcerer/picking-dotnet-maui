namespace Project_CS412.Views;

public interface ITabulatedView
{
    void OnSelect(object sender, EventArgs e);

    void OnSearch(object sender, EventArgs e);

    void OnCreate(object sender, EventArgs e);

    void OnDelete(object sender, EventArgs e);

    void OnRefresh(object sender, EventArgs e);

    void OnSave(object sender, EventArgs e);

    void OnSelectionChanged(object sender, EventArgs e);
}