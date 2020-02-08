using Microsoft.ML;
using PlaygroundML.Model.DataModels;
using System;

namespace MachineLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsumeModel();
        }

        public static void ConsumeModel()
        {
            // Load the model
            MLContext mlContext = new MLContext();

            ITransformer mlModel = mlContext.Model.Load("MLModel.zip", out var modelInputSchema);

            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            // Use the code below to add input data
            var input = new ModelInput();
            input.SentimentText = "That is rude.";

            // Try model on sample data
            // True is toxic, false is non-toxic
            ModelOutput result = predEngine.Predict(input);

            $"[{this.GetType().Name}] " + $"Text: {input.SentimentText} | Prediction: {(Convert.ToBoolean(result.Prediction) ? "Toxic" : "Non Toxic")} sentiment");
        }
    }
}
