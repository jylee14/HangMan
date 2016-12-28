using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Hangman
{
    /**
     * Load the dictionary from the assets into the dictionary DS 
     * This class should be constructed and called to work upon start of the app
     */
    class words
    {
        List<string> dictionary = new List<string>();
        string[] backup = { "books", "book", "golf", "reddit", "youtube", "author", "canada", "duck" };  //just in case load fails 
        public words(string fileName)
        {          
            loadFile(fileName);            
        }

        //return next word from the dictaionry 
        public string next()
        {
            Random rand = new Random();
            int next = rand.Next(0, dictionary.Count);

            try
            {
                return dictionary[next];
            }
            catch (Exception)
            {
                next = rand.Next(0, backup.Length);
                return backup[next];
            }
        }

        /**
         * name: private async void loadFile(string fileName);
         * description: load the fileName into the dicationry (list) ds
         */
        private async void loadFile(string fileName)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(fileName));
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                while (streamReader.Peek() >= 0)
                    dictionary.Add(streamReader.ReadLine());
            }
        }
    }
}
