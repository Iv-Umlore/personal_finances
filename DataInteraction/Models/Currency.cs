using DataInteraction.Helpers;

namespace DataInteraction.Models
{
    /// <summary>
    /// Валюта
    /// </summary>
    public class Currency
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string Sumbol { get; set; }

        /// <summary>
        /// Список наименований валюты
        /// </summary>
        public string OtherNames { get; set; }

        /// <summary>
        /// Является ли это валютой по умолчанию
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Основаная ли это стабильная валюта (обмен происходит либо с доллара либо с евро)
        /// </summary>
        public bool IsMainStable { get; set; }

        /// <summary>
        /// Последний курс обмена
        /// </summary>
        public double LastExchangeRate { get; set; }

        public string ToSqlInsertCommand()
        {
            return $"INSERT INTO main.'Currencies' ({SQLModelFields.GetCurrencyFields()}) " +
                $"VALUES ('{Name}', '{Sumbol}', '{OtherNames}', {(IsDefault ? 1 : 0)}, {(IsMainStable ? 1 : 0)}, {Converter.DoubleToString(LastExchangeRate)})";
        }

    }
}
