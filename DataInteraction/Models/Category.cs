namespace DataInteraction.Models
{
    public class Category
    {
        public long ID { get; set; }

        public long ParentID { get; set; }

        public string Name { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
