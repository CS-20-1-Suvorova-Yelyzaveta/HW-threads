using System;
using System.Threading;
using System.Threading.Tasks;

namespace card_emulator
{
    public class RunThreadPools
    {
        public static string InputPath = "../../../input/";
        public static string OutputPath = "../../../output/";
        public const string OutputFile = "/output.json";

        private readonly FilesManager _filesManager = new FilesManager();
        private readonly CardManager _cardManager = new CardManager();
        private readonly TransactionsManager _transactionsManager = new TransactionsManager();
        private readonly object _lockObject = new object();
        private readonly CountdownEvent _countdownEvent;

        public RunThreadPools()
        {
            _countdownEvent = new CountdownEvent(1);
        }

        public void Run()
        {
            _filesManager.DeleteAllFilesInDirectory(OutputPath);
            string[] files = _filesManager.GetSortedFiles(InputPath);

            foreach (var filePath in files)
            {
                _countdownEvent.AddCount();
                ThreadPool.QueueUserWorkItem(ProcessTransaction, filePath);
            }

            _countdownEvent.Signal();

            _countdownEvent.Wait();

            var outputData = new OutputData(
                _cardManager.AccountBalance,
                _cardManager.SuccessfulTransactionsCount,
                _cardManager.NonSuccessfulTransactionsCount,
                _filesManager.ProcessedFileCount
            );

            _transactionsManager.addOutputDataToJSONFile(outputData, OutputPath + OutputFile);
        }

        private void ProcessTransaction(object filePathObj)
        {
            try
            {
                string filePath = (string)filePathObj;
                int transactionAmount = _filesManager.ReadLastLineValue(filePath);
                bool isDeposit = transactionAmount > 0;
                bool success = isDeposit ? _cardManager.MakeDeposit(Math.Abs(transactionAmount)) : _cardManager.MakeWithdrawal(Math.Abs(transactionAmount));

                lock (_lockObject)
                {
                    _transactionsManager.AddTransactionLog(OutputPath, filePath, transactionAmount, success);
                }
            }
            finally
            {
                _countdownEvent.Signal(); 
            }
        }
    }
}
