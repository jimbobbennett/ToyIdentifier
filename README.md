# Toy Identifier

This is an example app, showing a number of different ways to use the [Azure Custom Vision service](https://customvision.ai/?WT.mc_id=toyidentifier-blog-jabenn) from a Xamarin app to identify images with different tags.

This repo has a number of branches, each with a different version of the app using the custom vision service in a different way.

The basics of the app are simple - a single Xamarin Forms page with a button. Tap the button, it will open the camera so you can take a photo. This photo is then analysed using an image classifier built using the custom vision service and the tag with the highest probability is found. If this probability is higher than a threshold (defaults to 50%) then the app says hello to the tag - both using text and speech. If no tags are found that are higher than the threshold the apps says it doesn't know what the image is.

Before you can use this app, you will need to create a custom vision project:

* Head to [https://customvision.ai](https://customvision.ai/?WT.mc_id=toyidentifier-blog-jabenn)
* Sign in with a Microsoft account
* Create a new project, upload some images and tag them as described [here](https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/getting-started-build-a-classifier?WT.mc_id=toyidentifier-blog-jabenn)

You can read more on creating this project at https://www.jimbobbennett.io/identifying-my-daughters-toys-using-ai/.

### master

The `master` branch is a version of the app that uses a [NuGet package](https://github.com/Microsoft/Cognitive-CustomVision-Windows) to make predictions using the classifier.

To use this NuGet package you need your project Id and prediction key from the project settings tab of the custom vision service. Set these values in the `ApiKeys.cs` file.

You can read more on this code at https://www.jimbobbennett.io/identifying-my-daughters-toys-using-ai-part-2-using-the-model/.

### OnDeviceModel

The `OnDeviceModel` branch contains a version of the app that uses a model downloaded from the custom vision service, both as a CoreML model on iOS and a TensorFlow model on Android. To use your models with the app, export them from the Performance tab as described [here](https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/export-your-model?WT.mc_id=toyidentifier-blog-jabenn).

You can read more on using this model from CoreML at https://www.jimbobbennett.io/identifying-my-daughters-toys-using-ai-part-3-offline-ios/.
