namespace IMS.Application.Features.Customers.Queries
{
    public class CustomerPlanDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SpeedMbps { get; set; }
        public decimal Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? NextDueDate { get; set; }
    }
}
