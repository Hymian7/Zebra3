using System.IO;

namespace Zebra3;

class Program
{
    public static string RootPath { get; set; } = @"C:\Users\lukas\Desktop\TaylorNoten";
    public static string MappingStructure { get; set; } = "%PIECE%/%PIECE%_%PART%.pdf";
    public static string ZebraPath
    {
        get { return Path.Join(RootPath, ".zebra"); }
    }

    public static List<FileInfo> PdfFiles {get; set; } = new List<FileInfo>();
    
    public static List<Piece> Pieces { get; set; } = new List<Piece>();
    public static List<Part> Parts {get; set; } = new List<Part>();
    public static List<Sheet> Sheets {get; set;} = new();

    static void Main(string[] args)
    {
        // Init repository = create .zebra hidden folder in the RootPath
        if (!Directory.Exists(ZebraPath))
        {
            // If it doesn't exist, create it as a hidden folder
            DirectoryInfo subfolderInfo = Directory.CreateDirectory(ZebraPath);
            
            // Set the hidden attribute for the folder
            subfolderInfo.Attributes |= FileAttributes.Hidden;
        }

        // Save MappingStructure
        var mappingStructurePath = Path.Combine(ZebraPath, "mappingstructure");
        if (!File.Exists(mappingStructurePath))
        {
            // Create the file with the hidden attribute
            using (FileStream fs = File.Create(mappingStructurePath))
            {
                // Add content to the file if needed
                byte[] content = System.Text.Encoding.UTF8.GetBytes(MappingStructure);
                fs.Write(content, 0, content.Length);
            }

            // Set the hidden attribute for the file
            File.SetAttributes(mappingStructurePath, File.GetAttributes(mappingStructurePath) | FileAttributes.Hidden);
        }

        // Loop through all PDF files
        // Recursively search for PDF files in the directory and its subdirectories
        PdfFiles.AddRange(GetPdfFiles(new DirectoryInfo(RootPath)));

        foreach (var file in PdfFiles)
        {
            var currentPiece = Path.GetFileNameWithoutExtension(file.FullName)?.Split('_').FirstOrDefault();
            var currentPart = Path.GetFileNameWithoutExtension(file.FullName)?.Split('_').LastOrDefault();

            var newSheet = new Sheet()
            {
                Path = file.FullName.Substring(RootPath.Length)
            };
            Sheets.Add(newSheet);
            

            if (!Pieces.Any(p => p.PieceName == currentPiece))
            {   
            // Add the new piece to the list
            Pieces.Add(new Piece(currentPiece));        
            }

             if(!Parts.Any(p => p.PartShortName == currentPart))
            {
            Parts.Add(new Part(currentPart));
            }
        }

        System.Console.WriteLine("# Pieces ".PadRight(25, '#'));
        foreach (var piece in Pieces)
        {
            System.Console.WriteLine(piece.PieceName);
        }
        System.Console.WriteLine("# Parts ".PadRight(25, '#'));
        foreach (var part in Parts)
        {
            System.Console.WriteLine(part.PartShortName);
        }
        System.Console.WriteLine("# Sheets ".PadRight(25, '#'));
        foreach (var sheet in Sheets)
        {
            System.Console.WriteLine(sheet.Path);
        }

    }   

    static List<FileInfo> GetPdfFiles(DirectoryInfo directory)
    {
        List<FileInfo> pdfFiles = new List<FileInfo>();

        try
        {
            // Get all PDF files in the current directory
            pdfFiles.AddRange(directory.GetFiles("*.pdf"));

            // Recursively search for PDF files in subdirectories
            foreach (var subdirectory in directory.GetDirectories())
            {
                pdfFiles.AddRange(GetPdfFiles(subdirectory));
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Handle any permission issues here, if necessary.
            Console.WriteLine($"Access denied to directory: {directory.FullName}");
        }

        return pdfFiles;
    }
}