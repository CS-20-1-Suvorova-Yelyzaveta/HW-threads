namespace card_emulator;

public class CardManager
{
    public int AccountBalance = 0;
    public int SuccessfulTransactionsCount = 0;

    public int NonSuccessfulTransactionsCount
    {
        get => nonSuccessfulTransactionsCount;
        set => nonSuccessfulTransactionsCount = value;
    }

    public int nonSuccessfulTransactionsCount = 0;
    private readonly object balanceLock = new object();

    public bool MakeDeposit(int amount)
    {
        lock (balanceLock)
        {
            AccountBalance += amount;
            SuccessfulTransactionsCount++;
        }

        return true;
    }
    
    public bool MakeWithdrawal(int amount)
    {
        lock (balanceLock)
        {
            if (AccountBalance >= amount)
            {
                AccountBalance -= amount;
                SuccessfulTransactionsCount++;
                return true;
            }

            NonSuccessfulTransactionsCount++;
            return false;
        }
    }
}