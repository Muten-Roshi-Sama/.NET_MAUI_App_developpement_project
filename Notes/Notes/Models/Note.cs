namespace Notes.Models;


/*In the first commit, the model types were acting both as the model (data) 
 * and as a view model (data preparation), which was mapped directly to a view. 
  * we'll delete About.cs and AllNotes.cs and replace their functionalities inside
    * Notes.cs (loading, saving, and deleting notes.)*/
internal class Note
{
    //Commit_1
    public string Filename { get; set; } //unique identifier, which is the file name of the note as stored on the device.
    public string Text { get; set; } //The text of the note.
    public DateTime Date { get; set; } //A date to indicate when the note was created or last updated.

    //Commit_2
    public Note() // constructor which sets the default values for the properties, including a random file name:
    {
        Filename = $"{Path.GetRandomFileName()}.notes.txt";
        Date = DateTime.Now;
        Text = "";
    }
    public void Save() =>
    File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Text);
    public void Delete() =>
        File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename));

    //Load notes way 1:  loading an individual note from a file
    public static Note Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return
            new()
            {
                Filename = Path.GetFileName(filename),
                Text = File.ReadAllText(filename),
                Date = File.GetLastWriteTime(filename)
            };
    }


    //Load notes way 2:  loading all notes on the device by enumerating all notes and load them into a collection.
    public static IEnumerable<Note> LoadAll()
    {
        /*This code returns an enumerable collection of Note model types by retrieving the 
         * files on the device that match the notes file pattern: *.notes.txt.Each file name 
         * is passed to the Load method, loading an individual note. Finally, the collection 
         * of notes is ordered by the date of each note and returned to the caller.*/

        // Get the folder where the notes are stored.
        string appDataPath = FileSystem.AppDataDirectory;

        // Use Linq extensions to load the *.notes.txt files.
        return Directory

        // Select the file names from the directory
                .EnumerateFiles(appDataPath, "*.notes.txt")

        // Each file name is used to load a note
                .Select(filename => Note.Load(Path.GetFileName(filename)))

        // With the final collection of notes, order them by date
                .OrderByDescending(note => note.Date);
    }

}