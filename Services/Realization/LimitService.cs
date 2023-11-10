using DataInteraction;
using Services.Interfaces;
using System.Text;

namespace Services.Realization
{
    public class LimitService : ILimitService
    {
        private DbProxy _dbProxy;
        public LimitService(DbProxy proxy) {
            _dbProxy = proxy;
        }

        private string limitFormat = "Лимит с {0} по {1}. \r\n Израсходовано: {2} / {3}. Осталось {4}\r\nВалюта: {5}\r\n";

        public string GetActiveLimitResult()
        {
            var activeLimits = _dbProxy.GetActiveLimits();

            DateTime startPeriodDate = activeLimits.Min(it => it.StartPeriod);
            DateTime endPeriodDate = activeLimits.Max(it => it.EndPeriod);

            var financeChanges = _dbProxy.GetPastFinancialChange(startPeriodDate, endPeriodDate);

            var currencies = _dbProxy.GetCurrencyList();

            StringBuilder result = new StringBuilder();
            foreach(var limit in activeLimits)
            {
                result.AppendLine();

                double moneySpend = financeChanges
                    .Where(fChange =>
                        fChange.DateOfFixation < limit.EndPeriod &&
                        fChange.DateOfFixation > limit.StartPeriod &&
                        fChange.CurrencyId == limit.CurrencyId)
                    .Sum(fChange => fChange.Summ);

                var currName = currencies.FirstOrDefault(c => c.ID == limit.CurrencyId)?.Name;

                result.Append(string.Format(limitFormat,
                    // Начало периода лимита, конец периода лимита
                    limit.StartPeriod, limit.EndPeriod,
                    // Денег израсходовано, изначальный лимит
                    moneySpend, limit.LimitSumm,
                    // Остаток, имя валюты
                    limit.LimitSumm - moneySpend, currName));
            }

            return result.ToString();
        }
    }
}
