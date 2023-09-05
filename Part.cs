// Part.cs
public class Part
{
    public string PartShortName { get; set; } = "";
    public string PartLongName { get; set; } = "";

    public Part(string partName)
    {
        PartShortName = partName;
    }
}