namespace Notes.Views;


[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class NotePage : ContentPage
{
    //User notes saved as text file.
    //string _fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");



    //constructor of the class
    public NotePage() 
    {
		InitializeComponent();  //reads the XAML markup and initializes
                                //all of the objects defined by the markup.

        //if (File.Exists(_fileName))
        //TextEditor.Text = File.ReadAllText(_fileName);  //read the file from the device
                                                            //and store its contents in the
                                                            //TextEditor control's Text property:
        string appDataPath = FileSystem.AppDataDirectory;
        string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";

        LoadNote(Path.Combine(appDataPath, randomFileName));
    }


    public string ItemId
    {
        set { LoadNote(value); }
    }




    //Buttons Methods
    private void LoadNote(string fileName)
    {
        Models.Note noteModel = new Models.Note();
        noteModel.Filename = fileName;

        if (File.Exists(fileName))
        {
            noteModel.Date = File.GetCreationTime(fileName);
            noteModel.Text = File.ReadAllText(fileName);
        }

        BindingContext = noteModel;
    }
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Note note)
            File.WriteAllText(note.Filename, TextEditor.Text);

        await Shell.Current.GoToAsync("..");
    }
    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Note note)
        {
            // Delete the file.
            if (File.Exists(note.Filename))
                File.Delete(note.Filename);
        }

        await Shell.Current.GoToAsync("..");
    }

}