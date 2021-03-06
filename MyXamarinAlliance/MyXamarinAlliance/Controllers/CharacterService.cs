﻿using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using XamarinAllianceApp.Helpers;
using XamarinAllianceApp.Models;

namespace XamarinAllianceApp.Controllers
{
    public class CharacterService
    {
        public MobileServiceClient Client;
        private IMobileServiceTable<Character> CharacterTable;

        public CharacterService()
        {
            Client = new MobileServiceClient(Constants.MobileServiceClientUrl);
        }

        /*
        /// <summary>
        /// Get the list of characters from an embedded JSON file, including their child entities.
        /// </summary>
        /// <returns>Array of Character objects</returns>
        private async Task<Character[]> ReadCharactersFromFile()
        {
            var assembly = typeof(CharacterService).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(Constants.CharactersFilename);
            string text;

            using (var reader = new System.IO.StreamReader(stream))
            {
                text = await reader.ReadToEndAsync();
            }

            var characters = JsonConvert.DeserializeObject<Character[]>(text);
            return characters;
        }
        */

        public async Task<ObservableCollection<Character>> GetCharactersAsync()
        {
            //var characters = await ReadCharactersFromFile();
            //return new ObservableCollection<Character>(characters);
            try
            {
                CharacterTable = Client.GetTable<Character>();
                var query = CharacterTable.OrderBy(c => c.Name);
                var characters = await query.ToListAsync();

                return new ObservableCollection<Character>(characters);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
