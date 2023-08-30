using DataInteraction.Helpers;

namespace DataInteraction.Models
{
    public class User : IBaseModel
    {
		public long ID { get; set; }

		public string TName { get; set; }

		public string RealName { get; set; }

		public DateTime DateCreate { get; set; }

		public long Roots { get; set; }

        public string ToSqlInsertCommand()
        {
            return $"INSERT INTO main.Users ({SQLModelFields.GetUserFields()})" +
                $" VALUES ('{TName}', '{RealName}', '{Converter.DateToString(DateCreate)}')";
        }
    }
}
