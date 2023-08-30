namespace DataInteraction.Models
{
    public class Limit
	{
		public long ID { get; set; }

		public DateTime StartPeriod { get; set; }

		public DateTime EndPeriod { get; set; }

		public double LimitSumm { get; set; }

		public long LimitTypeId { get; set; }

		public double CreditByLastPeriod { get; set; }

		public long CurrencyId { get; set; }

	}
}
