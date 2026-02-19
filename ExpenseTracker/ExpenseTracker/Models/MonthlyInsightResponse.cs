namespace ExpenseTracker.Models
{
    public class MonthlyInsightResponse
    {
        public decimal PredictedTotal { get; set; }
        public string Trend { get; set; }
        public string RiskLevel { get; set; }
        public List<string> Drivers { get; set; }
        public List<string> Recommendations { get; set; }
    }

}
