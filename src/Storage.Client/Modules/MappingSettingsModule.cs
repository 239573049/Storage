namespace Storage.Client.Modules;

public class MappingSettingsModule
{
    public string Label { get; set; }

    public int Value { get; set; }

    public MappingSettingsModule(string label, int value)
    {
        Label = label;
        Value = value;
    }
}