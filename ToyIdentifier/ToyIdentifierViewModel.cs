using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Cognitive.CustomVision.Prediction;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.TextToSpeech;
using Xamarin.Forms;

namespace ToyIdentifier
{
    public class ToyIdentifierViewModel : ViewModelBase
    {
        public ToyIdentifierViewModel()
        {
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
            TakePhotoCommand = new Command(async () => await TakePhoto());
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        }

        private async Task TakePhoto()
        {
            CanTakePhoto = false;

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { AllowCropping = true });

                if (file == null)
                {
                    ToyNameMessage = "";
                    return;
                }

                using (var stream = file.GetStream())
                {
                    var endpoint = new PredictionEndpoint {ApiKey = ApiKeys.ApiKey};
                    var result = endpoint.PredictImage(ApiKeys.ProjectId, stream);

                    var toy = result.Predictions.OrderByDescending(p =>p.Probability).First().Tag;
                    ToyNameMessage = $"Hello {toy}";
                    await CrossTextToSpeech.Current.Speak(ToyNameMessage);
                }
            }
            catch
            {
                ToyNameMessage = "";
            }
            finally
            {
                CanTakePhoto = true;
            }
        }

        private string _toyNameMessage = "";
        public string ToyNameMessage
        {
            get => _toyNameMessage;
            set => Set(ref _toyNameMessage, value);
        }

        private bool _canTakePhoto = true;
        public bool CanTakePhoto
        {
            get => _canTakePhoto;
            set => Set(ref _canTakePhoto, value);
        }

        public ICommand TakePhotoCommand { get; }
    }
}
