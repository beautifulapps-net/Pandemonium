using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pandemonium.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(object dataContext)
        {
            try
            {
                InitializeComponent();

                DataContext = dataContext;
            }
            catch { }
        }

        public Action<IEnumerable<string>> OnFilesAdded { get; set; }
        public Action<IEnumerable<Models.PlaylistItem>> OnItemsRemoved { get; set; }
        public Action<IEnumerable<Models.PlaylistItem>, int> OnItemsVolumeChanged { get; set; }
        public Action<IEnumerable<Models.PlaylistItem>> OnTogglePaused { get; set; }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                case Key.Play:
                case Key.Pause:
                case Key.MediaPlayPause:
                    {
                        if (lvwFiles.SelectedItems.Count == 0) return;

                        OnTogglePaused?.Invoke(lvwFiles.SelectedItems.Cast<Models.PlaylistItem>());
                        return;
                    }

                case Key.OemPlus:
                case Key.Add:
                    {
                        if (lvwFiles.SelectedItems.Count == 0) return;

                        OnItemsVolumeChanged?.Invoke(lvwFiles.SelectedItems.Cast<Models.PlaylistItem>(), +1);
                        return;
                    }

                case Key.OemMinus:
                case Key.Subtract:
                    {
                        if (lvwFiles.SelectedItems.Count == 0) return;

                        OnItemsVolumeChanged?.Invoke(lvwFiles.SelectedItems.Cast<Models.PlaylistItem>(), -1);
                        return;
                    }

                case Key.Delete:
                    {
                        if (lvwFiles.SelectedItems.Count == 0) return;

                        OnItemsRemoved?.Invoke(lvwFiles.SelectedItems.Cast<Models.PlaylistItem>());
                        return;
                    }
            }

        }

        private void lvwFiles_DragEnter(object sender, DragEventArgs e)
        {
            bool hasFiles = e.Data.GetDataPresent(DataFormats.FileDrop);
            if (hasFiles)
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void lvwFiles_Drop(object sender, DragEventArgs e)
        {
            if (OnFilesAdded == null) return;

            bool hasFiles = e.Data.GetDataPresent(DataFormats.FileDrop);
            if (!hasFiles) return;

            var droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            if ((droppedFiles?.Count() ?? 0) == 0) return;

            var invalidFiles = droppedFiles.Where(file => !System.IO.File.Exists(file));
            var validFiles = droppedFiles.Where(file => System.IO.File.Exists(file));

            if (invalidFiles.Any())
            {
                MessageBox.Show("Some paths were invalid, or inaccessible, and will not be added.\nPress OK add the remaining files");
            }

            OnFilesAdded?.Invoke(validFiles);
        }

    }
}
