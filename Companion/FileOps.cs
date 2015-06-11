using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Companion
{
    
    class FileOps
    {
        string session;
        GeoCoordinate coord;
        public List<GeoCoordinate> coordList = new List<GeoCoordinate>();
        public FileOps(string sessionName)
        {
            this.session = sessionName;
            //this.coordList = new List<GeoCoordinate>();
        }

        public FileOps()
        {
           // this.coordList = new List<GeoCoordinate>();
        }

        public async Task readCoordinate()
        {
         //   List<GeoCoordinate> coordList = new List<GeoCoordinate>();
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            
            // Get the DataFolder folder.
            var dataFolder = await local.GetFolderAsync("Companion");
            var files = from file in Directory.GetFiles(dataFolder.Path)
                        orderby file descending
                        select file;
            // Get the file.
            try
            {
                var file1 = await dataFolder.OpenStreamForReadAsync("pqr.txt");
                // file1.ReadTimeout = 5000;
                // file1.WriteTimeout = 5000;

                using (StreamReader sr = new StreamReader(file1))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        String[] words = s.Split(',');
                        coord = new GeoCoordinate(Double.Parse(words[0]), Double.Parse(words[1]));
                        coordList.Add(coord);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public async void generateCoordList()
        {
            await readCoordinate();
            
        }

        public List<GeoCoordinate> getCoordList()
        {
           // generateCoordList();
            return coordList;
        }

        public async void writeToFile(string data){
           
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dataFolder = await local.CreateFolderAsync("Companion",
                CreationCollisionOption.OpenIfExists);
            // Create a new file named DataFile.txt.
            var file = await dataFolder.CreateFileAsync(session,
            CreationCollisionOption.GenerateUniqueName);

            // Write the data from the textbox.
            using (StreamWriter sw = File.AppendText(file.Path))
            {
                await sw.WriteLineAsync(data);
            }

            
        }

        
    }
}
