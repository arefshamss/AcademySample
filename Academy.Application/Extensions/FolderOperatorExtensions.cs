using System.Diagnostics;

namespace Academy.Application.Extensions;

public static class FolderOperatorExtensions
{
    private static string _deletedFilePath;
    private static DeleteActionType _deleteActionType;
        
    public static void Delete(string path,bool recursive=false)
    {
        System.IO.Directory.Delete(path,recursive);
    }
        
    public static void Create(string path)
    {
        System.IO.Directory.CreateDirectory(path);
    }

    public static bool Exists(string path)
    {
        return System.IO.Directory.Exists(path);
    }
        
    public static void Rar(string inputPath, string output,DeleteActionType deleteActionType=DeleteActionType.NoAction)
    {
        Process p = new Process();
        p.StartInfo.FileName = @"C:\Program Files\WinRAR\Rar.exe";
        p.StartInfo.Arguments = $"a -ep1 -idq -r -y {output}  {inputPath.Replace("/", "\\")}";
        p.StartInfo.CreateNoWindow = false;
        p.EnableRaisingEvents = true;
        p.Start();

        _deleteActionType = deleteActionType;
        _deletedFilePath = inputPath;

        p.Exited +=new EventHandler(RarProcess_Existed);
    }

    private static void RarProcess_Existed(object sender, System.EventArgs e)
    {
        switch (_deleteActionType)
        {
            case DeleteActionType.DeleteFile:
                    
                if(FileOperatorExtensions.Exists(_deletedFilePath))
                    FileOperatorExtensions.Delete(_deletedFilePath);
                    
                break;
                
            case DeleteActionType.DeleteDirectory:
                    
                if(Exists(_deletedFilePath))
                    Delete(_deletedFilePath,true);
                    
                break;
        }
    }
}

public enum DeleteActionType
{
    NoAction,   
    DeleteFile,
    DeleteDirectory
}