namespace Common.Models
{
    public class CategorySimpleModel
    {
        /// <summary>
        /// Id в БД
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя категории
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Родительская директория
        /// </summary>
        public long ParentId { get; set; } 

        /// <summary>
        /// Вспомогательный ID для вспомогательного списка
        /// </summary>
        public long IdInList { get; set; }
    }
}
