using DataInteraction;
using TelegramBot.Cases.FinancialChange;
using TelegramBot.CommonStrings;

namespace TelegramBot.Worker
{
    public class GroupChatSpeaker
    {
        private Dictionary<string, List<string>> _commands;
        private FinancialChangeCase _financialChangeCase;

        public GroupChatSpeaker(DbProxy proxy)
        {
            _financialChangeCase = new FinancialChangeCase(proxy);
            _commands = new Dictionary<string, List<string>>();
        }

        public string DoSmtg(string userName, string command, string botName, DateTime commandDateTime)
        {
            try
            {
                if (!_commands.ContainsKey(userName))
                {
                    _commands[userName] = new List<string>();
                }

                if (command == KeyWords.Canсel)
                {
                    _commands[userName].Clear();
                    return CommonPhrases.DoneMessage;
                }

                if (command.Contains(botName))
                {
                    if (command.Contains(" - "))
                        _commands[userName].Clear();
                    _financialChangeCase.SetHelpfullInfo(commandDateTime, botName);
                    _commands[userName].Add(command);
                    return _financialChangeCase.ProcessTheCommand(userName, _commands[userName]);
                }

                _commands[userName].Clear();

                return string.Empty;
            }
            catch (Exception ex)
            {
                _commands[userName].Clear();
                Console.WriteLine(ex.ToString());
                return CommonPhrases.GetInternalErrorMessage();
            }
        }


    }
}
