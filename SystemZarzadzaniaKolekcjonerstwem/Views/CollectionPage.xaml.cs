using SystemZarzadzaniaKolekcjonerstwem.Models;
using Microsoft.Maui.Controls;
using SystemZarzadzaniaKolekcjonerstwem.Services;
using System.Linq;
namespace SystemZarzadzaniaKolekcjonerstwem.Views;



public partial class CollectionPage : ContentPage
{
    public List<Item> Items = new List<Item>();
    public string SelectedColName;
    public CollectionPage(Collection selectedCollection)
    {
        InitializeComponent();
        SelectedColName = selectedCollection.Name;
        collectionNameLabel.Text = SelectedColName;
        ReadItems();
        RefreshItems();
    }


    private void Button_Clicked(object sender, EventArgs e)
    {
        if(itemNameEntry.Text == "" || itemNameEntry.Text == null)
        {
            DisplayAlert("Uwaga", "Najpierw nazwij przedmiot", "OK");
        }
        else
        {
            Items.Add(new Item(itemNameEntry.Text));
            itemNameEntry.Text = "";
            FileService.WriteColItems(SelectedColName, Items);
            RefreshItems();
        }

    }

    public void ReadItems()
    {
        Items.Clear();
        Items = FileService.ReadColItems(SelectedColName);
    }

    private void RefreshItems()
    {
        itemsListLayout.Children.Clear();
        int numerator = 1;
        foreach (var item in Items)
        {
            Label label = new Label { Text = $"{numerator}. "+item.Name, Margin = new Thickness(10, 10, 10, 10) };
            //label.BackgroundColor = Colors.Red;
            itemsListLayout.Add(label);
            numerator++;
        }
    }

    private void ReadCollection()
    {
        itemsListLayout.Children.Clear();
        //file read collection
    }

    private async void RemoveButton_Clicked(object sender, EventArgs e)
    {
        var itemNamesList = Items.Select(x => x.Name).ToArray();
        var itemToDelete = await DisplayActionSheet("Wybierz element do usuniêcia: ", "Anuluj", null, itemNamesList);

        if(itemToDelete != "Anuluj")
        {
            Items.Remove(Items.FirstOrDefault(it => it.Name == itemToDelete));
            FileService.WriteColItems(SelectedColName, Items);
            RefreshItems();
        }
    }

    private async void EditButton_Clicked(object sender, EventArgs e)
    {
        var itemNamesList = Items.Select(x => x.Name).ToArray();
        var itemToEdit = await DisplayActionSheet("Wybierz element do usuniêcia: ", "Anuluj", null, itemNamesList);

        if(itemToEdit != "Anuluj")
        {
            string newValue = await DisplayPromptAsync("Edycja",$"Podaj now¹ wartoœæ elementu {itemToEdit}: ");
            if(newValue != null)
            {
                Items.FirstOrDefault(it => it.Name == itemToEdit).Name = newValue;
                FileService.WriteColItems(SelectedColName, Items);
                RefreshItems();

            }
        }

    }
}