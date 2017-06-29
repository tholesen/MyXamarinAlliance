using MyXamarinAlliance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinAllianceApp.Controllers;
using XamarinAllianceApp.Helpers;
using XamarinAllianceApp.Models;

namespace XamarinAllianceApp.ViewModels
{
    public class CharacterListViewModel : BaseViewModel
    {
        private CharacterService service;

        public ObservableCollection<Grouping<string, Character>> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public CharacterListViewModel()
        {
            service = App.CharacterService;
            Items = new ObservableCollection<Grouping<string, Character>>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await service.GetCharactersAsync();

                Dictionary<string, List<Character>> characterDictionary = new Dictionary<string, List<Character>>();

                foreach (var item in items)
                {
                    foreach (var appearance in item.Appearances)
                    {
                        if (!characterDictionary.ContainsKey(appearance.Title))
                        {
                            characterDictionary.Add(appearance.Title, new List<Character>());
                        }

                        characterDictionary[appearance.Title].Add(item);
                    }
                }

                var groupedItems = characterDictionary.OrderBy(x => x.Key)
                    .Select(x => new Grouping<string, Character>(x.Key, x.Value));

                //Items.AddRange(groupedItems);
                foreach (var item in groupedItems)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
