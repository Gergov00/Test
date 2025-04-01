using System.Globalization;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // Можно задавать имена файлов как аргументы командной строки или задавать жестко.
        string inputFile = "D:\\_C# Projects\\Test\\Quest_3\\input.txt";
        string outputFile = "D:\\_C# Projects\\Test\\Quest_3\\output.txt";
        string problemFile = "D:\\_C# Projects\\Test\\Quest_3\\problems.txt";

        LogStandardizer standardizer = new LogStandardizer(inputFile, outputFile, problemFile);
        standardizer.ProcessLogs();
    }
}



public class LogStandardizer
{
    private readonly string inputFile;
    private readonly string outputFile;
    private readonly string problemFile;

    public LogStandardizer(string inputFile, string outputFile, string problemFile)
    {
        this.inputFile = inputFile;
        this.outputFile = outputFile;
        this.problemFile = problemFile;
    }

    public void ProcessLogs()
    {
        using (StreamReader reader = new StreamReader(inputFile))
        using (StreamWriter writer = new StreamWriter(outputFile))
        using (StreamWriter problemWriter = new StreamWriter(problemFile))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Попытка обработки Формата 1:
                // Пример: 10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'
                var format1 = new Regex(@"^(?<date>\d{2}\.\d{2}\.\d{4})\s+(?<time>[\d:.]+)\s+(?<level>INFORMATION|WARNING|ERROR|DEBUG)\s+(?<message>.*)$");
                var match1 = format1.Match(line);
                if (match1.Success)
                {
                    string dateStr = match1.Groups["date"].Value;
                    string timeStr = match1.Groups["time"].Value;
                    string level = match1.Groups["level"].Value;
                    string message = match1.Groups["message"].Value;

                    if (level == "INFORMATION")
                        level = "INFO";
                    else if (level == "WARNING")
                        level = "WARN";

                    string callerMethod = "DEFAULT";

                    // Преобразуем дату из формата dd.MM.yyyy в dd-MM-yyyy.
                    if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateParsed))
                    {
                        dateStr = dateParsed.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        problemWriter.WriteLine(line);
                        continue;
                    }

                    writer.WriteLine($"{dateStr}\t{timeStr}\t{level}\t{callerMethod}\t{message}");
                    continue;
                }

                // Попытка обработки Формата 2:
                // Пример: 2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'
                string[] tokens = line.Split('|');
                if (tokens.Length >= 5)
                {
                    string[] firstParts = tokens[0].Trim().Split(' ');
                    if (firstParts.Length >= 2)
                    {
                        string dateStr = firstParts[0].Trim();
                        string timeStr = firstParts[1].Trim();
                        string level = tokens[1].Trim();
                        // Токен tokens[2] игнорируем.
                        string callerMethod = tokens[3].Trim();
                        string message = tokens[4].Trim();

                        if (level == "INFORMATION")
                            level = "INFO";
                        else if (level == "WARNING")
                            level = "WARN";


                        // Преобразуем дату из формата yyyy-MM-dd в dd-MM-yyyy.
                        if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateParsed))
                        {
                            dateStr = dateParsed.ToString("dd-MM-yyyy");
                        }
                        else
                        {
                            problemWriter.WriteLine(line);
                            continue;
                        }

                        writer.WriteLine($"{dateStr}\t{timeStr}\t{level}\t{callerMethod}\t{message}");
                        continue;
                    }
                }

                // Если ни один формат не подошёл, записываем исходную строку в файл проблем.
                problemWriter.WriteLine(line);
            }
        }

        Console.WriteLine("Обработка завершена.");
    }
}


