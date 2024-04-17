using Microsoft.Maui.Layouts;
using System.Diagnostics;
using SystemZarzadzaniaKolekcjonerstwem.Models;
using SystemZarzadzaniaKolekcjonerstwem.Services;
using SystemZarzadzaniaKolekcjonerstwem.Views;

namespace SystemZarzadzaniaKolekcjonerstwem
{
    public partial class MainPage : ContentPage
    {
        public List<Collection> collections = new List<Collection>();
        public Collection pickedCollection;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            Debug.WriteLine(FileService.GetFilepath());
            ReadCollections();
        }


        private void ReadCollections()
        {
            collections.Clear();
            foreach (var collection in FileService.ReadCollectionState())
            {
                collections.Add(new Collection(collection));
            }
            RefreshCollectionList();
        }

        private void addCollection_clicked(object sender, EventArgs e)
        {
            

            string collectionName = collectionNameEntry.Text;

            if(collectionName == null || collectionName == "" )
            {
                DisplayAlert("Uwaga", "Najpierw uzupełnij nazwę", "OK");
            }
            else if (!FileService.CheckAvailable(collectionName))
            {
                DisplayAlert("Uwaga", "Taka kolekcja już istnieje", "OK");
                collectionNameEntry.Text = "";
            }
            else
            {
                FileService.WriteCollection(collectionName);
                collections.Add(new Collection(collectionName));
                collectionNameEntry.Text = "";
                RefreshCollectionList();
            }
        }

        private void RefreshCollectionList()
        {
            collectionListLayout.Children.Clear();

            foreach (var collection in collections)
            {
                Button button = new Button { Text = collection.Name };
                button.Clicked += OnCollectionPicked;
                button.Margin = new Thickness(0, 0, 20, 0);
                FlexLayout.SetBasis(button, new FlexBasis(300));
                collectionListLayout.Add(button);
            }
        }


        private async void OnCollectionPicked(object sender, EventArgs e)
        {
            Button clicked = sender as Button;
            string pickedName = clicked.Text;

            pickedCollection = collections.FirstOrDefault(col => col.Name == pickedName);

            if (pickedCollection == null)
            {
                DisplayAlert("Błąd", "Nie udało się wybrać kolekcji", "OK");
                throw new Exception("pickedCollection is null");
            }
            await Navigation.PushAsync(new CollectionPage(pickedCollection));



        }

        private async void RemoveCollection_clicked(object sender, EventArgs e)
        {
            
            var colNameList = collections.Select(col => col.Name).ToArray();
            string colToDeleteName = await DisplayActionSheet("Wybierz kolekcję do usunięcia", "Anuluj", null, colNameList);
            if(colToDeleteName != "Anuluj")
            {
                bool areYouSureAboutThat =  await DisplayAlert("Uwaga", $"Czy na pewno chcesz usunąć: {colToDeleteName} ?", "Tak", "Nie");
                if(areYouSureAboutThat)
                {
                FileService.DeleteCollection(colToDeleteName);
                collections.Remove(collections.FirstOrDefault(col=>col.Name == colToDeleteName));
                RefreshCollectionList();
                }
            }
        }
    }

}
