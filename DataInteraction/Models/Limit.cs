using DataInteraction.Helpers;

namespace DataInteraction.Models
{
    public class Limit : IBaseModel
    {
		public long ID { get; set; }

		public DateTime StartPeriod { get; set; }

		public DateTime EndPeriod { get; set; }

		public double LimitSumm { get; set; }

		public long LimitTypeId { get; set; }

		public double CreditByLastPeriod { get; set; }

		public long CurrencyId { get; set; }

        public string ToSqlInsertCommand()
        {
            return $"INSERT INTO main.Limits ({SQLModelFields.GetLimitFields()}) " +
                $"VALUES ('{Converter.DateToString(StartPeriod)}', '{Converter.DateToString(EndPeriod)}', {Converter.DoubleToString(LimitSumm)}, {LimitTypeId}, {Converter.DoubleToString(CreditByLastPeriod)}, {CurrencyId})";
        }
    }
}
