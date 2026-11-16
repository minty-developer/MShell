namespace Minty.Shell.FileManager;

public static class FM
{
    public static string CurrentDirectory { get; private set; } = AppContext.BaseDirectory;
    public static string GetDisplayPath(string path, int keepFolders = 3)
    {
        string fullPath = Path.GetFullPath(path);
        return fullPath;

        // 전체 경로를 보여주기 위해 주석 처리
        // string rootPath = Path.GetPathRoot(fullPath)!;

        // string relativePath = Path.GetRelativePath(rootPath, fullPath);

        // string[] folders = relativePath.Split(
        //     [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
        //     StringSplitOptions.RemoveEmptyEntries
        // );

        // if (folders.Length <= keepFolders)
        //     return fullPath;

        // string shortenedPath = Path.Combine(
        //     rootPath,
        //     "...",
        //     Path.Combine([.. folders.Skip(folders.Length - keepFolders)])
        // );

        // return shortenedPath;
    }

    public static bool MoveToDirectory(string path)
    {
        string targetPath = Path.GetFullPath(Path.Join(CurrentDirectory, path));

        if (!Directory.Exists(targetPath))
            return false;

        CurrentDirectory = targetPath;
        System.Environment.CurrentDirectory = targetPath;
        return true;
    }
}