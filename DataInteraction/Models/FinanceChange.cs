using DataInteraction.Helpers;

namespace DataInteraction.Models
{
    public class FinanceChange : IBaseModel
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

        public string ToSqlInsertCommand()
        {
			return $"INSERT INTO main.FinanceChanges ({SQLModelFields.GetFinanceChangeFields})" +
				$"VALUES ('{Converter.DateToString(DateOfFixation)}', {Summ.ToString("0.##")}, {CurrencyId}, '{Comment}', {FixedBy}, {CategoryId}, {SumInIternationalCurrency.ToString("0.##")})";
        }
    }
}
