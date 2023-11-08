using DataInteraction;

namespace TelegramBot.Cases
{
    public abstract class BaseCase : IBaseCase
    {

        protected DbProxy _dbProxy;

        protected BaseCase(DbProxy dbProxy) {
            _dbProxy = dbProxy;
        }

        /// <summary>
        /// Получить следующую инструкцию для общения с пользователем
        /// </summary>
        protected abstract Func<string, List<string>, string> GetNextStep(string userName, List<string> commands);

        /// <summary>
        /// Выполнить поуступившую команду
        /// </summary>
        /// <param name="userName"> Имя пользователя в месседжере </param>
        /// <param name="commands"> Набор команд в строгой последовательности</param>
        /// <returns> Ответ пользователю </returns>
        public virtual string ProcessTheCommand(string userName, List<string> commands)
        {
            InputValidation(userName, commands);

            Func<string, List<string>, string> callFunction = GetNextStep(userName, commands);

            return callFunction(userName, commands);
        }

        public virtual string CancelTheCommand(string userName, List<string> Commands)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверка входящих данных на корректность
        /// </summary>
        /// <exception cref="NullReferenceException"> При отсутсвии входящих значений </exception>
        protected static void InputValidation(string userName, List<string> commands)
        {
            if (string.IsNullOrEmpty(userName))
                throw new NullReferenceException("Имя пользователя null");

            if (commands == null)
                throw new NullReferenceException("Список команд пуст");
        }

        protected string DefaultAnswer(string userName, List<string> commands)
        {
            throw new Exception($"Вызов использование нереализованного функционала от пользователя {userName}" +
                $"\r\nИспользуемые команды: \"{string.Join(" ... ", commands)}\"");
        }
    }
}
