using System.Text;

namespace Quest_1;

class Program
{
    public static void Main()
    {
        string[] inputs = new string[]
        {
            "",                         // пустая строка
            "a",                        // "a" → один символ без числа
            "aaaaaaaaaaaa",             // "a12" → 12 раз "a"
            "aaab",                     // "a3b" → 3 раза "a" и 1 раз "b"
            "aaaaaaaaaabbbbb",          // "a10b5" → 10 раз "a" и 5 раз "b"
            "a",                        // "a0" → 0 интерпретируется как 1, поэтому "a"
            "aabbbc",                  // "a2b3c" → 2 раза "a", 3 раза "b" и 1 раз "c"
            "aaaaaaaaaaaabccc",         // "a12b0c3" → 12 раз "a", 0 → 1 раз "b", 3 раза "c"
            "zzzzzyyyyyyyyyy"           // "z5y10" → 5 раз "z" и 10 раз "y"
        };;
        foreach (var input in inputs)
        {
            Console.WriteLine(input);
            string resultCompression = Compression(input);
            Console.WriteLine(resultCompression);
            var resultDecompression = Decompression(resultCompression);
            Console.WriteLine(resultDecompression);
            Console.WriteLine(input == resultDecompression ? "Ok" : "Error");
        }
    }

    private static string Compression(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        var result = new StringBuilder();
        var count = 1;

        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] == input[i - 1])
            {
                count++;
            }
            else
            {
                result.Append(input[i - 1]);
                if (count > 1)
                {
                    result.Append(count);
                    count = 1;
                }
            }
        }

        result.Append(input[input.Length - 1]);
        if (count > 1)
            result.Append(count);

        return result.ToString();
    }

    private static string Decompression(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        var result = new StringBuilder();
        int i = 0;

        while (i < input.Length)
        {
            char current = input[i++];
            int count = 0;

            while (i < input.Length && char.IsDigit(input[i]))
            {
                count = count * 10 + (input[i] - '0');
                i++;
            }

            if (count == 0)
                count = 1;

            result.Append(new string(current, count));
        }

        return result.ToString();
    }
}