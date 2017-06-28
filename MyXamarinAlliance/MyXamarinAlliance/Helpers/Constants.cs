using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinAllianceApp.Helpers
{
    public class Constants
    {
        /// <summary>
        /// File name for the embedded characters JSON file
        /// </summary>
        public static readonly string CharactersFilename = "MyXamarinAlliance.characters.json";
        //public static readonly string MobileServiceClientUrl = "http://xamarinalliancebackend.azurewebsites.net/";
        public static readonly string MobileServiceClientUrl = "https://xamarinalliancesecurebackend.azurewebsites.net/";
        public static readonly string BlobClientUrl = "https://xamarinalliance.blob.core.windows.net/";
    }
}
