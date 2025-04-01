
namespace Quest_2;


class Program
{
    public static void Main()
    {
            Task[] tasks = new Task[10];

        for (int i = 0; i < 5; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                for (int j = 0; j < 10; j++)
                {
                    int currentCount = Server.GetCount();
                    Console.WriteLine($"Reader {Task.CurrentId} read count = {currentCount}");
                    Thread.Sleep(100); 
                }
            });
        }

        for (int i = 5; i < 10; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                for (int j = 0; j < 5; j++)
                {
                    Server.AddToCount(1);
                    Thread.Sleep(150); 
                }
            });
        }

        Task.WaitAll(tasks);
        Console.WriteLine("Final count: " + Server.GetCount());
    }
}


public static class Server
{
    
    private static int count = 0;
    
    private static readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

    public static int GetCount()
    {
        rwLock.EnterReadLock();
        try
        {
            return count;
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }

    public static void AddToCount(int value)
    {
        rwLock.EnterWriteLock();
        try
        {
            count += value;
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }
}