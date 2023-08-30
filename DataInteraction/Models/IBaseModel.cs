namespace DataInteraction.Models
{
    public interface IBaseModel
    {

        public string ToSqlInsertCommand();
    }
}
