namespace Services.Interfaces
{
    public interface ILimitService
    {
        /// <summary>
        /// Вернуть детальную информацию об активных Лимитах
        /// </summary>
        public string GetActiveLimitResult();
    }
}
