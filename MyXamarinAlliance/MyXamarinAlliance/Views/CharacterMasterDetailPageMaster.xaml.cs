using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinAllianceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public CharacterMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new CharacterMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class CharacterMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<CharacterMasterDetailPageMenuItem> MenuItems { get; set; }

            public CharacterMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<CharacterMasterDetailPageMenuItem>(new[]
                {
                    new CharacterMasterDetailPageMenuItem { Id = 0, Title = "Page 1" },
                    new CharacterMasterDetailPageMenuItem { Id = 1, Title = "Page 2" },
                    new CharacterMasterDetailPageMenuItem { Id = 2, Title = "Page 3" },
                    new CharacterMasterDetailPageMenuItem { Id = 3, Title = "Page 4" },
                    new CharacterMasterDetailPageMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}