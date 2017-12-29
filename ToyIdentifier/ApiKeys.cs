using System;

namespace ToyIdentifier
{
    public static class ApiKeys
    {
#error You need to set up your API keys.
        // Start by registering for an account at https://customvision.ai
        // Then create a new project.
        // From the settings tab, find:
        // Prediction Key
        // Project Id
        // and update the values below
        public static string PredictionKey = "<Your Prediction Key>";
        public static Guid ProjectId = Guid.Parse("<Your Project GUID>");
    }
}
