using System.Globalization;

namespace Aplikacja_do_wyliczania_proporcji.Views;

public partial class InfoPage : ContentPage
{
	public InfoPage()
	{
		InitializeComponent();
        ReadTextFile();

    }

    private async Task  ReadTextFile()
    {
        string filePath,text;
        var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        if (culture=="pl")
        {
            filePath = "Info/InfText_pl.txt";
            text = "Brak pliku";
        }
        else
        {
            filePath = "Info/InfText.txt";
            text = "No file";
        }
        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
        if (fileStream!=null)
        {
            using StreamReader reader = new StreamReader(fileStream);
            if (reader != null)
            {
                 text = await reader.ReadToEndAsync();
                if (text!=null)
                { 
                TextName.Text = text;
                }
            }
        }
    }
}