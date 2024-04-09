namespace Project_CS412;

// Configures application through AppSettings.json in Resources/Raw
public class AppConfig
{
    public DatabaseConfig Database { get; set; }
    public SyncfusionConfig Syncfusion { get; set; }
}

public class DatabaseConfig
{
    public string Name { get; set; }
}

public class SyncfusionConfig
{
    public string License { get; set; }
}