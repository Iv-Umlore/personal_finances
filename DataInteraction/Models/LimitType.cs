namespace DataInteraction.Models
{
    public class LimitType
	{
		public long ID { get; set; }

		public string Name { get; set; }

		public string Period { get; set; }

		public DateTime StartPeriod { get; set; }

		public bool IsAutoProlongation { get; set; }

		public bool IsActive { get; set; }

	}
}
