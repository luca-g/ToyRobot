namespace ToyRobot.Core.Configuration;

public class MapSettings
{
    public const string SectionName = "MapSettings";
    public int MinWidth { get; set; } = 5;
    public int MinHeight { get; set; } = 5;
    public int MaxWidth { get; set; } = 100;
    public int MaxHeight { get; set; } = 100;

    public (int width, int height) GetValidSize(int width, int height)
    {
        width = width < this.MinWidth ? this.MinWidth :
            (width > this.MaxWidth ? this.MaxWidth : width);
        height = height < this.MinHeight ? this.MinHeight :
            (height > this.MaxHeight ? this.MaxHeight : height);
        return (width, height);
    }
}
