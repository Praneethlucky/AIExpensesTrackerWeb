namespace ExpenseTracker.BusinessLogic.DTO;
public class CategoryDto
{
    public int CategoryId { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string Icon { get; set; }

    public string Color { get; set; }

    public bool IsSystem { get; set; }
}