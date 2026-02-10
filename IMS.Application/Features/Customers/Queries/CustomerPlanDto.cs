namespace IMS.Application.Features.Customers.Queries
{
    public class CustomerPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SpeedMbps { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? NextDueDate { get; set; }
    }
}
