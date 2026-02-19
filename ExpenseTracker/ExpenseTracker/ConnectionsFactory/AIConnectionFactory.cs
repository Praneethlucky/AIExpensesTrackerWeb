using ExpenseTracker.Models;
using Microsoft.Data.SqlClient;

namespace ExpenseTracker.ConnectionsFactory
{
    public class AIConnectionFactory
    {
        private readonly IConfigurationSection _aiModelDetails;

        public AIConnectionFactory(IConfiguration configuration)
        {
            _aiModelDetails = configuration.GetSection("AI");
        }

        public AIModelSettings GetModelDetails()
        {
            return new AIModelSettings
            {
                ApiKey = _aiModelDetails["ApiKey"],
                Model = _aiModelDetails["Model"],
                MaxTokens = Int32.Parse(_aiModelDetails["MaxTokens"]),
                Temperature = float.Parse(_aiModelDetails["Temperature"])
            };
        }
    }

}
