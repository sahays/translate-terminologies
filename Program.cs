using System;
using System.Text;
using System.Threading.Tasks;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Translate;
using Amazon.Translate.Model;
using Microsoft.Extensions.Configuration;

namespace amazon_translate
{
    class Program
    {
        const string EnglishText = @"Amazon Translate is a text translation service that uses advanced machine learning technologies to provide high-quality translation on demand.";
        static void Main(string[] args)
        {
            var awsOptions = BuildAwsOptions();
            var service = new TranslateService(awsOptions.CreateServiceClient<IAmazonTranslate>());
            // list of supported languages
            // https://docs.aws.amazon.com/translate/latest/dg/how-it-works.html#how-it-works-language-codes
            // translate from English to Hindi
            var translateTask = service.TranslateText(EnglishText, "en", "hi");
            translateTask.Wait();
            var translatedText = translateTask.Result.TranslatedText;
            Console.WriteLine("Source: {0}", EnglishText);
            Console.WriteLine("Translation: {0}", translatedText);
        }

        private static AWSOptions BuildAwsOptions()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            return builder.GetAWSOptions();
        }
    }

    public class TranslateService
    {
        private IAmazonTranslate translate;
        public TranslateService(IAmazonTranslate translate)
        {
            this.translate = translate;
        }

        public async Task<TranslateTextResponse> TranslateText(string text, string sourceLanguage, string targetLanguage)
        {
            var request = new TranslateTextRequest
            {
                SourceLanguageCode = sourceLanguage,
                TargetLanguageCode = targetLanguage,
                Text = text
            };

            return await this.translate.TranslateTextAsync(request);
        }
    }

}
