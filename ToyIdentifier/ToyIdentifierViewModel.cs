using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Cognitive.CustomVision.Prediction;
using Microsoft.Cognitive.CustomVision.Prediction.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.TextToSpeech;
using Xamarin.Forms;

namespace ToyIdentifier
{
    public class ToyIdentifierViewModel : ViewModelBase
    {
        private PredictionEndpoint _endpoint = new PredictionEndpoint { ApiKey = ApiKeys.ApiKey };
        private const double ProbabilityThreashold = 0.5;

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
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { PhotoSize = PhotoSize.Medium });
            ToyNameMessage = BuildToyMessage(file);
            DeletePhoto(file);
        }

        private string BuildToyMessage(MediaFile file)
        {
            var message = "You need to photo a toy";

            try
            {
                if (file != null)
                {
                    var mostLikely = GetBestTag(file);
                    if (mostLikely == null)
                        message = "I don't know who that is";
                    else
                        message = $"Hello {mostLikely.Tag}";
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

        private ImageTagPredictionModel GetBestTag(MediaFile file)
        {
            using (var stream = file.GetStream())
            {
                return _endpoint.PredictImage(ApiKeys.ProjectId, stream)
                                .Predictions
                                .OrderByDescending(p => p.Probability)
                                .FirstOrDefault(p => p.Probability > ProbabilityThreashold);
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
