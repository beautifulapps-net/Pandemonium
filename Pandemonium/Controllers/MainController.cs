using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Pandemonium.Models;

namespace Pandemonium.Controllers
{
    public class MainController
    {
        private Playlist Playlist { get; set; }

        public MainController()
        {
            Playlist = new Playlist();
        }

        public void Start()
        {
            LoadState();

            var pandemoniumPlayer = new Pandemonium.Views.MainView(Playlist);

            pandemoniumPlayer.OnFilesAdded += AddFiles;
            pandemoniumPlayer.OnItemsRemoved += RemoveFiles;
            pandemoniumPlayer.OnTogglePaused += TogglePauseState;
            pandemoniumPlayer.OnItemsVolumeChanged += ChangeVolume;

            pandemoniumPlayer.ShowDialog();

            SaveState();
        }

        private void AddFiles(IEnumerable<string> files)
        {
            Playlist.Add(files);
        }

        private void RemoveFiles(IEnumerable<PlaylistItem> playlistItems)
        {
            Playlist.Delete(playlistItems);
        }

        private void ChangeVolume(IEnumerable<PlaylistItem> playlistItems, int delta)
        {
            foreach (var item in playlistItems)
            {
                item.Volume += delta;
            }
        }

        private void TogglePauseState(IEnumerable<PlaylistItem> playlistItems)
        {
            foreach (var item in playlistItems)
            {
                item.Paused = !item.Paused;
            }
        }


        private void SaveState()
        {
            string settingsFolder = GetSettingsFolderPath();
            if (!System.IO.Directory.Exists(settingsFolder))
            {
                System.IO.Directory.CreateDirectory(settingsFolder);
            }

            string json = JsonConvert.SerializeObject(Playlist);
            System.IO.File.WriteAllText(GetSettingsFilePath(), json);
        }

        private void LoadState()
        {
            string settingsFile = GetSettingsFilePath();
            if (!System.IO.File.Exists(settingsFile)) return;

            string json = System.IO.File.ReadAllText(settingsFile);
            var _playlist = JsonConvert.DeserializeObject<Playlist>(json);
            Playlist = _playlist;
        }

        private string GetSettingsFolderPath()
        {
            return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"pandemonium");
        }

        private string GetSettingsFilePath()
        {
            return System.IO.Path.Combine(GetSettingsFolderPath(), @"pandemonium.config");
        }
    }

    class MainControllerConverter : JsonConverter<MainController>
    {
        public override MainController ReadJson(JsonReader reader, Type objectType, MainController existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, MainController value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
