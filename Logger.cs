static class Logger
{
    static Logger()
    {
        logWriter = new StreamWriter(File.Open("log.txt", FileMode.Create));
    }
    static StreamWriter logWriter;
    public static void Log(string message)
    {
        logWriter.Write(message);
    }
}