using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pandemonium.Models;

namespace Pandemonium.Models
{
    public class Playlist:ObservableCollection<PlaylistItem>
    {
        public Playlist()
        { }

        public void Add(string soundFile)
        {
            Add(new PlaylistItem(soundFile));
        }

        public void Add(IEnumerable<string> soundFiles)
        {
            foreach (var soundFile in soundFiles)
            {
                Add(new PlaylistItem(soundFile));
            }
        }

        public void Delete(PlaylistItem playlistItem)
        {
            Remove(playlistItem);
        }

        public void Delete(IEnumerable<PlaylistItem> playlistItems)
        {
            foreach (var item in playlistItems.ToList())
            {
                Remove(item);
            }
        }

    }
}
