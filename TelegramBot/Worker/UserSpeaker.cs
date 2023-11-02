using DataInteraction;
using TelegramBot.Cases;

namespace TelegramBot.Worker
{
    public class UserSpeaker
    {
        private string _lastCommands;
        private string _currentCommand;

        private CurrencyCases _currencyCases;
        private LimitCases _limitCases;
        private CategoriesCases _categoriesCases;

        public UserSpeaker(DbProxy proxy)
        {
            _currencyCases = new CurrencyCases(proxy);
            _limitCases = new LimitCases(proxy);
            _categoriesCases = new CategoriesCases(proxy);
        }

        public string DoSmtg(string command)
        {
            if (command == KeyWords.Cansel)
            {
                _lastCommands = "";
                _currentCommand = "";
                return KeyWords.BaseMessage;
            }
            
            if (KeyWords.Commands.Contains(command))
            {
                _lastCommands = command;
                _currentCommand = command;
            }
            else
                _lastCommands += $"&{command}";


            switch (_currentCommand)
            {
                case KeyWords.Limits:
                    return _limitCases.ProcessTheCommand(_lastCommands);

                case KeyWords.Currency:
                    return _currencyCases.ProcessTheCommand(_lastCommands);

                case KeyWords.Category:
                    return _categoriesCases.ProcessTheCommand(_lastCommands);

                default:
                    _lastCommands = "";
                    _currentCommand = "";
                    return KeyWords.BaseMessage;
            }
        }
    }
}
