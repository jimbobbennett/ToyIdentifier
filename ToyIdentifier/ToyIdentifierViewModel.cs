using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.TextToSpeech;
using Xamarin.Forms;

namespace ToyIdentifier
{
    public class ToyIdentifierViewModel : ViewModelBase
    {
        private const double ProbabilityThreshold = 0.5;

        public ToyIdentifierViewModel()
        {
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
            TakePhotoCommand = new Command(async () => await TakePhoto());
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        }

        private async Task TakePhoto()
        {
            CanTakePhoto = false;
            await TakePhotoAndBuildToyMessage();
            CanTakePhoto = true;

            await CrossTextToSpeech.Current.Speak(ToyNameMessage);
        }

        private async Task TakePhotoAndBuildToyMessage()
        {
            var options = new StoreCameraMediaOptions { PhotoSize = PhotoSize.Medium };
            var file = await CrossMedia.Current.TakePhotoAsync(options);
            ToyNameMessage = await BuildToyMessage(file);
            DeletePhoto(file);
        }

        private async Task<string> BuildToyMessage(MediaFile file)
        {
            var message = "You need to photo a toy";

            try
            {
                if (file != null)
                {
                    var mostLikely = await GetBestTag(file);
                    if (mostLikely == null)
                        message = "I don't know who that is";
                    else
                        message = $"Hello {mostLikely}";
                }
            }
            catch
            {
                message = "I don't know who that is";
            }

            return message;
        }

        private static void DeletePhoto(MediaFile file)
        {
            var path = file?.Path;

            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                File.Delete(file?.Path);
        }

        private async Task<string> GetBestTag(MediaFile file)
        {
            using (var stream = file.GetStream())
            {
                var tags = await DependencyService.Get<IImageClassifier>().ClassifyImage(stream);
                if (!tags.Any()) return null;

                return tags.Where(t => t.Probability > ProbabilityThreshold)
                           .OrderByDescending(t => t.Probability)
                           .First().Tag;
            }
        }

        private string _toyNameMessage = string.Empty;
        public string ToyNameMessage
        {
            get => _toyNameMessage;
            set => Set(ref _toyNameMessage, value);
        }

        private bool _canTakePhoto = true;
        public bool CanTakePhoto
        {
            get => _canTakePhoto;
            set
            {
                if (Set(ref _canTakePhoto, value))
                    RaisePropertyChanged(nameof(ShowSpinner));
            }
        }

        public bool ShowSpinner => !CanTakePhoto;

        public ICommand TakePhotoCommand { get; }
    }
}
