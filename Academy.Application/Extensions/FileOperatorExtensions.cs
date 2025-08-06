namespace Academy.Application.Extensions;

public static class FileOperatorExtensions
{
    public static void Copy(string input, string output)
    {
        if (Exists(@input))
        {
            System.IO.File.Copy(@input, @output, true);
            while (new FileInfo(@input).Length > new FileInfo(@output).Length)

                System.Threading.Thread.Sleep(1000);
        }
        else
        {
            throw new FileNotFoundException();
        }
    }

    public static void Cut(string input, string output)
    {
        if (Exists(@input))
            System.IO.File.Move(@input, @output, true);
    }

    public static void Delete(string input)
    {
        if (Exists(@input))
            System.IO.File.Delete(@input);
    }

    public static bool Exists(string input)
    {
        return System.IO.File.Exists(@input);
    }
}