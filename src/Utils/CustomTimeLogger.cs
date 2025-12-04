using System.Text;

class CustomTimeLogger : TextWriter
{
    private readonly TextWriter _original;

    private readonly DateTime _startTime = DateTime.Now;
    private readonly string _timerOperation;

    public CustomTimeLogger(TextWriter original, string timerOperation)
    {
        _original = original;
        _timerOperation = timerOperation;
        Console.SetOut(this);

        LogInfo($"STARTING: {_timerOperation}");
    }

    public override Encoding Encoding => _original.Encoding;

    public override void WriteLine(string? value) =>
        _original.WriteLine($"[{DateTime.Now - _startTime}] {value}");

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            LogInfo($"FINISHED: {_timerOperation}");
            Console.SetOut(_original);
        }
        base.Dispose(disposing);
    }

    private static void LogInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}