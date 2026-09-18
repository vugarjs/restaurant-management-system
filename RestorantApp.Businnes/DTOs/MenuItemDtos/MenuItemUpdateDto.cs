namespace RestorantApp.Businnes.DTOs.MenuItemDtos;

public class MenuItemUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

}
