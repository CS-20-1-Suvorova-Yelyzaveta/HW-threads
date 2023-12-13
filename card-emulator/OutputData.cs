namespace card_emulator;

public class OutputData
{
    public int FinalBalance { get; set; }
    public int SuccessfulTransactions { get; set; }
    public int UnsuccessfulTransactions { get; set; }
    public int ProcessedFilesCount { get; set; }

    public OutputData(int finalBalance, int successfulTransactions, int unsuccessfulTransactions, int processedFilesCount)
    {
        FinalBalance = finalBalance;
        SuccessfulTransactions = successfulTransactions;
        UnsuccessfulTransactions = unsuccessfulTransactions;
        ProcessedFilesCount = processedFilesCount;
    }

}