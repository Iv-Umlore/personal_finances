namespace DataInteraction.Models
{
    public class FinanceChange
	{

		public long ID { get; set; }

		public DateTime DateOfFixation { get; set; }

		/// <summary>
		/// Сумма
		/// </summary>
		public double Summ { get; set; }

		/// <summary>
		/// Валюта
		/// </summary>
		public long CurrencyId { get; set; }

		/// <summary>
		/// Комментарий
		/// </summary>
		public string Comment { get; set; }

		/// <summary>
		/// Кем зафиксирована трата
		/// </summary>
		public long FixedBy { get; set; }

		/// <summary>
		/// Категория покупки
		/// </summary>
		public long CategoryId { get; set; }

		/// <summary>
		/// Сумма в общей валюте
		/// </summary>
		public double SumInIternationalCurrency { get; set; }
	}
}
