namespace IMS.Domain.Entities
{
    public class CustomerPlan
    {
        public Guid Id { get; set; }
        public Guid InternetPlanId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
