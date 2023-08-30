namespace DataInteraction.Helpers
{
    public static class SQLModelFields
    {
        #region Currency

        /// <summary>
        /// Name, Sumbol, OtherNames, IsDefault, IsMainStable, LastExchangeRate
        /// </summary>
        public static string GetCurrencyFields()
        {
            return "Name, Sumbol, OtherNames, IsDefault, IsMainStable, LastExchangeRate";
        }

        /// <summary>
        /// ID, Name, Sumbol, OtherNames, IsDefault, IsMainStable, LastExchangeRate
        /// </summary>
        public static string GetCurrency_Full()
        {
            return "ID, Name, Sumbol, OtherNames, IsDefault, IsMainStable, LastExchangeRate";
        }

        #endregion

        #region Category

        /// <summary>
        /// ParentID, Name, CreatedBy, CreateDate
        /// </summary>
        public static string GetCategoryFields()
        {
            return "ParentID, Name, CreatedBy, CreateDate";
        }

        /// <summary>
        /// ID, ParentID, Name, CreatedBy, CreateDate
        /// </summary>
        /// <returns></returns>
        public static string GetCategory_Full()
        {
            return "ID, ParentID, Name, CreatedBy, CreateDate";
        }

        #endregion

        #region User

        /// <summary>
        /// TName, RealName, DateCreate
        /// </summary>
        public static string GetUserFields()
        {
            return "TName, RealName, DateCreate";
        }

        /// <summary>
        /// ID, TName, RealName, DateCreate
        /// </summary>
        /// <returns></returns>
        public static string GetUser_Full()
        {
            return "ID, TName, RealName, DateCreate";
        }

        #endregion

        #region FinanceChange

        /// <summary>
        /// DateOfFixation, Summ, Currency, Comment, FixedBy, Category, SumInIternationalCurrency
        /// </summary>
        public static string GetFinanceChangeFields()
        {
            return "DateOfFixation, Summ, Currency, Comment, FixedBy, Category, SumInIternationalCurrency";
        }

        /// <summary>
        /// ID, DateOfFixation, Summ, Currency, Comment, FixedBy, Category, SumInIternationalCurrency
        /// </summary>
        public static string GetFinanceChange_Full()
        {
            return "ID, DateOfFixation, Summ, Currency, Comment, FixedBy, Category, SumInIternationalCurrency";
        }

        #endregion 

        #region LimitType

        /// <summary>
        /// Name, Period, StartPeriod, IsAutoProlongation, IsActive
        /// </summary>
        public static string GetLimitTypeFields()
        {
            return "Name, Period, StartPeriod, IsAutoProlongation, IsActive";
        }

        /// <summary>
        /// "ID, Name, Period, StartPeriod, IsAutoProlongation, IsActive"
        /// </summary>
        public static string GetLimitType_Full()
        {
            return "ID, Name, Period, StartPeriod, IsAutoProlongation, IsActive";
        }

        #endregion

        #region Limit

        /// <summary>
        /// StartPeriod, EndPeriod, LimitSumm, LimitType, CreditByLastPeriod, Currency
        /// </summary>
        public static string GetLimitFields()
        {
            return "StartPeriod, EndPeriod, LimitSumm, LimitType, CreditByLastPeriod, Currency";
        }

        /// <summary>
        /// ID, StartPeriod, EndPeriod, LimitSumm, LimitType, CreditByLastPeriod, Currency
        /// </summary>
        public static string GetLimit_Full()
        {
            return "ID, StartPeriod, EndPeriod, LimitSumm, LimitType, CreditByLastPeriod, Currency";
        }

        #endregion

    }
}
