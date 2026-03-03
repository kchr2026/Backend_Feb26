using System.Text.Json;

public static class SaveGameService
{
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    /// <summary>
    /// Returns a standardly named savefile in the current working directory
    /// </summary>
    /// <returns>string</returns>
    public static string DefaultSaveFileName()
    {
        return "savegame.json";
    }

    /// <summary>
    /// Create a new savefile on a specified filepath
    /// </summary>
    /// <param name="path">filepath</param>
    /// <param name="save">savefile</param>
    public static void Save(string path, SaveGame save)
    {
        var jsonData = JsonSerializer.Serialize(save, _options);
        // This will rewrite the file & it's content, every time this method is called!, So we can later create a "OverwriteSave" where to add data to an existing save file
        File.WriteAllText(path, jsonData);
    }

    /// <summary>
    /// Attempt to load the save file in memory, if this method returns false, and error occured when trying to load a savefile
    /// </summary>
    /// <param name="path">filepath for the savefile</param>
    /// <param name="save">savefile</param>
    /// <param name="errorMessage">error message</param>
    /// <returns>bool</returns>
    public static bool TryLoadFile(string path, out SaveGame? save, out string errorMessage)
    {
        save = null;
        errorMessage = null!;

        try
        {
            if (!File.Exists(path))
            {
                errorMessage = "No save file found!";
                return false;
            }

            var jsonData = File.ReadAllText(path);
            save = JsonSerializer.Deserialize<SaveGame>(jsonData, _options);

            if (save == null)
            {
                errorMessage = "Could not properly load the save file! (empty/invalid data)";
                return false;
            }

            if (save.Version != 1)
            {
                errorMessage = $"Unknown save file version: {save.Version}";
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return false;
        }
    }
}