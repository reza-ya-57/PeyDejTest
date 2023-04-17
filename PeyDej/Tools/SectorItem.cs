using Microsoft.AspNetCore.Mvc.Rendering;

namespace PeyDej.Tools;

public class SectorItem
{
    public int Id { set; get; }
    public string Name { set; get; }

    public SectorItem(int Id, string Name)
    {
        this.Id = Id;
        this.Name = Name;
    }
}