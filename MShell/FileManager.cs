namespace Minty.Shell.FileManager;

public static class FM
{
    public static string CurrentDirectory { get; private set; } = AppContext.BaseDirectory;
    public static string GetDisplayPath(string path, int keepFolders = 3)
    {
        string fullPath = Path.GetFullPath(path);
        string rootPath = Path.GetPathRoot(fullPath)!;

        string relativePath = Path.GetRelativePath(rootPath, fullPath);

        string[] folders = relativePath.Split(
            [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
            StringSplitOptions.RemoveEmptyEntries
        );

        if (folders.Length <= keepFolders)
            return fullPath;

        string shortenedPath = Path.Combine(
            rootPath,
            "...",
            Path.Combine([.. folders.Skip(folders.Length - keepFolders)])
        );

        return shortenedPath;
    }

    public static bool MoveToDirectory(string path)
    {
        string targetPath = Path.GetFullPath(Path.Join(CurrentDirectory, path));

        if (!Directory.Exists(targetPath))
            return false;

        CurrentDirectory = targetPath;
        return true;
    }

    public static string GetFolderTree(string path)
    {
        DirectoryInfo directoryInfo = new(path);
        DirectoryInfo[] directories = directoryInfo.GetDirectories();
        string treeString = string.Empty;
        if(directories.Length == 0) { return ""; }
        treeString += $"{Path.GetDirectoryName(path)}\n";
        for(int i = 0; i < directories.Length - 1; i++)
        {
            treeString += $"  ├─ {directories[i].Name}\n";
        }
        treeString += $"  └─ {directories[^1].Name}";
        return treeString;
    }
}