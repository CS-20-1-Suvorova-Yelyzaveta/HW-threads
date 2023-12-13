using Newtonsoft.Json;

namespace card_emulator;

public class TransactionsManager
{
    public void AddTransactionLog(string outputDirectory, string transactionFilePath, decimal transactionAmount, bool transactionSuccess)
    {
        try
        {
            var logFilePath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(transactionFilePath));
            var logEntry = $"{transactionAmount},{transactionSuccess}\n";

            File.AppendAllText(logFilePath, logEntry);
        }
        catch (Exception logException)
        {
            Console.WriteLine($"Error appending transaction log: {logException.Message}");
        }
    }
    public void addOutputDataToJSONFile(object dataObject, string targetFilePath)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(dataObject, Formatting.Indented);
            File.WriteAllText(targetFilePath, jsonData);
        }
        catch (Exception jsonException)
        {
            Console.WriteLine($"Error saving object to JSON file: {jsonException.Message}");
        }
    }

}