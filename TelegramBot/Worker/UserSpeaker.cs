using DataInteraction;
using TelegramBot.Cases.Currency;
using TelegramBot.Cases.Categories;
using TelegramBot.Cases.Limits;
using TelegramBot.CommonStrings;

namespace TelegramBot.Worker
{
    public class UserSpeaker
    {
        private Dictionary<string, List<string>> _userFullCommands = new Dictionary<string, List<string>>();
        private Dictionary<string, string> _userLastCommands = new Dictionary<string, string>();

        private CurrencyCases _currencyCases;
        private LimitCases _limitCases;
        private CategoriesCases _categoriesCases;

        public UserSpeaker(DbProxy proxy)
        {
            _currencyCases = new CurrencyCases(proxy);
            _limitCases = new LimitCases(proxy);
            _categoriesCases = new CategoriesCases(proxy);
        }

        public string DoSmtg(string userName, string command)
        {
            if (!_userLastCommands.ContainsKey(userName))
            {
                _userLastCommands[userName] = "";
                _userFullCommands[userName] = new List<string>();
            }

            if (command == KeyWords.Canсel)
            {
                _userLastCommands[userName] = "";
                _userFullCommands[userName].Clear();
                return CommonPhraces.BaseMessage;
            }
            
            if (command.Length < 0)
                return CommonPhraces.EmptyPhrase + CommonPhraces.CanсelCommand;

            if (KeyWords.Commands.Contains(command))
            {
                _userLastCommands[userName] = command;
                _userFullCommands[userName].Add(command);
            }
            else
                _userFullCommands[userName].Add(command);


            switch (_userLastCommands[userName])
            {
                case KeyWords.Limits:
                    return _limitCases.ProcessTheCommand(userName, _userFullCommands[userName]) + CommonPhraces.CanсelCommand;

                case KeyWords.Currency:
                    return _currencyCases.ProcessTheCommand(userName, _userFullCommands[userName]) + CommonPhraces.CanсelCommand;

                case KeyWords.Category:
                    return _categoriesCases.ProcessTheCommand(userName, _userFullCommands[userName]) + CommonPhraces.CanсelCommand;

                default:
                    _userLastCommands[userName] = "";
                    _userFullCommands[userName].Clear();
                    return CommonPhraces.BaseMessage;
            }
        }
    }
}
