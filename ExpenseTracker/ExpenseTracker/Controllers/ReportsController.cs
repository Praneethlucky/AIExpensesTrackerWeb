using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

/// <summary>
/// Small server-side reporting endpoints. These provide real-world usable summaries
/// without changing existing Razor page behavior. Reports are computed from the Bills table.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly SqlConnectionFactory _connectionFactory;

    public ReportsController(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public class MonthlyReport
    {
        public string MonthKey { get; set; } = "";
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalExpenses { get; set; }
        public Dictionary<string, decimal> BreakdownByPeriod { get; set; } = new Dictionary<string, decimal>();
        public int BillCount { get; set; }
    }

    /// <summary>
    /// Generate a lightweight monthly report (aggregates full bill amounts that apply during the month).
    /// Accepts query: ?year=2026&month=3&userId=1
    /// </summary>
    [HttpGet("monthly")]
    public async Task<ActionResult<MonthlyReport>> GetMonthly([FromQuery] int year, [FromQuery] int month, [FromQuery] int userId)
    {
        if (year <= 0 || month < 1 || month > 12) return BadRequest("Invalid year or month.");
        if (userId <= 0) return BadRequest("userId is required.");

        var monthStart = new DateTime(year, month, 1);
        var monthEnd = monthStart.AddMonths(1).AddTicks(-1);

        // Select bills that are active at any time during the month:
        // StartDate <= monthEnd AND (IsInfinite = 1 OR EndDate IS NULL OR EndDate >= monthStart)
        const string sql = @"
SELECT Period, SUM(Amount) AS Total, COUNT(*) AS Cnt
FROM Bills
WHERE UserId = @UserId
  AND StartDate <= @MonthEnd
  AND (IsInfinite = 1 OR EndDate IS NULL OR EndDate >= @MonthStart)
GROUP BY Period;
";

        var report = new MonthlyReport
        {
            MonthKey = $"{year}-{month:D2}",
            Year = year,
            Month = month
        };

        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@MonthStart", monthStart);
        cmd.Parameters.AddWithValue("@MonthEnd", monthEnd);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        decimal total = 0;
        int billCount = 0;
        while (await reader.ReadAsync())
        {
            var period = reader.GetString(0);
            var partTotal = reader.IsDBNull(1) ? 0m : reader.GetDecimal(1);
            var cnt = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);

            report.BreakdownByPeriod[period] = partTotal;
            total += partTotal;
            billCount += cnt;
        }

        report.TotalExpenses = total;
        report.BillCount = billCount;
        return Ok(report);
    }
}