using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.InternetPlans.Queries
{
    public class InternetPlanDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SpeedMbps { get; set; }
        public decimal Price { get; set; }
    }
}
