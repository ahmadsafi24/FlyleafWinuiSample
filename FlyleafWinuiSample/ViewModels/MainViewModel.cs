using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FlyleafLib;
using FlyleafLib.MediaPlayer;

namespace FlyleafWinuiSample.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region MyRegion
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!(object.Equals(field, newValue)))
            {
                field = (newValue);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public Player Player { get; set; }
        public MainViewModel()
        {
            PlayPauseCommand = new AsyncRelayCommand(PlayPauseAsync);

            Engine.Start(new EngineConfig()
            {
                UIRefresh = false, // For Activity Mode usage
                PluginsPath = ":Plugins",
                FFmpegPath = @"D:\temp\FlyleafFFmpeg\FFmpeg",

            });

            Player = new Player();
            Player.PropertyChanged += Player_PropertyChanged;
            Player.Config.Video.BackgroundColor = System.Windows.Media.Colors.Black;


        }

        private void Player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                IsPlaying = Player.IsPlaying;
                NotifyPropertyChanged(nameof(IsPlaying));
            }
        }

        public ICommand PlayPauseCommand { get; }
        public Task PlayPauseAsync()
        {
            if (Player.Status == Status.Playing)
            {
                Player.Pause();
            }
            else if (Player.Status == Status.Paused)
            {
                Player.Play();
            }
            else
            {
                OpenCompletedArgs openArg = Player.Open(@"D:\temp\sample.mp4");

            }
            return Task.CompletedTask;
        }


        public bool IsPlaying { get; set; }
    }

}