using DataInteraction.Helpers;

namespace DataInteraction.Models
{
    public class LimitType : IBaseModel
    {
		public long ID { get; set; }

		public string Name { get; set; }

		public string Period { get; set; }

		public DateTime StartPeriod { get; set; }

		public bool IsAutoProlongation { get; set; }

		public bool IsActive { get; set; }

        public string ToSqlInsertCommand()
        {
            return $"INSERT INTO main.LimitTypes ({SQLModelFields.GetLimitTypeFields()}) " +
                $"VALUES ('{Name}', '{Period}', '{Converter.DateToString(StartPeriod)}', {(IsAutoProlongation? 1: 0)}, {(IsActive? 1: 0)})";
        }
    }
}
