namespace card_emulator;

public class RunTasks
{
    public static string InputPath = "../../../input/";
    public static string OutputPath = "../../../output/";
    public const string OutputFile = "/output.json";

    private readonly FilesManager _filesManager = new FilesManager();
    private readonly CardManager _cardManager = new CardManager();
    private readonly TransactionsManager _transactionsManager = new TransactionsManager();
    private readonly object _lockObjectCardManager = new object();
    private readonly object _lockObjectTransactionsManager = new object();

    public async Task Run()
    {
        _filesManager.DeleteAllFilesInDirectory(OutputPath);
        string[] files = _filesManager.GetSortedFiles(InputPath);

        var tasks = files.Select(filePath => ProcessTransaction(filePath)).ToArray();
        await Task.WhenAll(tasks);

        var outputData = new OutputData(
            _cardManager.AccountBalance,
            _cardManager.SuccessfulTransactionsCount,
            _cardManager.NonSuccessfulTransactionsCount,
            _filesManager.ProcessedFileCount
        );

        lock (_lockObjectTransactionsManager)
        {
            _transactionsManager.addOutputDataToJSONFile(outputData, OutputPath + OutputFile);
        }
    }

    private async Task ProcessTransaction(string filePath)
    {
        int transactionAmount = _filesManager.ReadLastLineValue(filePath);
        bool isDeposit = transactionAmount > 0;
        bool success;

        lock (_lockObjectCardManager)
        {
            success = isDeposit ? _cardManager.MakeDeposit(Math.Abs(transactionAmount)) : _cardManager.MakeWithdrawal(Math.Abs(transactionAmount));
        }

        lock (_lockObjectTransactionsManager)
        {
            _transactionsManager.AddTransactionLog(OutputPath, filePath, transactionAmount, success);
        }
    }
}
