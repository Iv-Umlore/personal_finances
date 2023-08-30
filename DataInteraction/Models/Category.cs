using DataInteraction.Helpers;

namespace DataInteraction.Models
{
    public class Category : IBaseModel
    {
        public long ID { get; set; }

        public long ParentID { get; set; }

        public string Name { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string ToSqlInsertCommand()
        {
            return $"INSERT INTO main.Categories ({SQLModelFields.GetCategoryFields()})" +
                $"VALUES ({ParentID}, '{Name}', {CreatedBy}, '{Converter.DateToString(CreateDate)}')";
        }
    }
}
