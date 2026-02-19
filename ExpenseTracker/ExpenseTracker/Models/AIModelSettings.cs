namespace ExpenseTracker.Models
{
    public class AIModelSettings
    {
        public string ApiKey { get; set; }
        public string Model { get; set; }
        public int MaxTokens { get; set; }
        public float Temperature { get; set; }
    }
}
