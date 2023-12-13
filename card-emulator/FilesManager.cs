namespace card_emulator;

public class FilesManager
{
    public int ProcessedFileCountValue;

    public int ProcessedFileCount => ProcessedFileCountValue;

    public string[] GetSortedFiles(string folderPath)
    {
        try
        {
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);
                string[] sortedFiles = files.OrderBy(file => Path.GetFileName(file)).ToArray();
                ProcessedFileCountValue = sortedFiles.Length;
                return sortedFiles;
            }
            else
            {
                Console.WriteLine($"Error: Folder does not exist - {folderPath}");
                return Array.Empty<string>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving sorted files: {ex.Message}");
            return Array.Empty<string>();
        }
    }


    public int ReadLastLineValue(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length > 0)
                {
                    return int.TryParse(lines[^1], out int lastLineValue) ? lastLineValue : 0;
                }
                else
                {
                    Console.WriteLine($"Error: File {filePath} is empty.");
                    return 0;
                }
            }
            else
            {
                Console.WriteLine($"Error: File {filePath} does not exist.");
                return 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the last line of the file {filePath}: {ex.Message}");
            return 0;
        }
    }

    public void DeleteAllFilesInDirectory(string directoryPath)
    {
        try
        {
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);

                if (files.Length > 0)
                {
                    Array.ForEach(files, File.Delete);
                    Console.WriteLine($"Successfully deleted {files.Length} files in {directoryPath}.");
                }
                else
                {
                    Console.WriteLine($"No files found in {directoryPath} to delete.");
                }
            }
            else
            {
                Console.WriteLine($"Error: Directory {directoryPath} does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting files in {directoryPath}: {ex.Message}");
        }
    }

}