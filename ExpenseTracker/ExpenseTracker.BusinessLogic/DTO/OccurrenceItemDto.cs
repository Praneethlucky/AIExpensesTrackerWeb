public class OccurrenceItemDto
{
    public int occurrenceId { get; set; }

    public string name { get; set; }

    public decimal amount { get; set; }

    public DateTime dueDate { get; set; }

    public bool isPaid { get; set; }
    public string categoryName { get; set; }
    public string categoryIcon { get; set; }
    public string categoryColor { get; set; }
    public string paymentTypeName { get; set; }
}