using DataInteraction;
using DataInteraction.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.CommonStrings;
using TelegramBot.Worker;

namespace TelegramBot
{
    public class BotSheduler
    {
        private long _mainConfId;
        private CancellationTokenSource _botWorkCT;

        private ITelegramBotClient _tClient;
        private ReceiverOptions _receiverOptions;

        private DbProxy _dbProxy;
        private UserSpeaker _speaker;

        // Обработать чтобы не было вариантов Null

        /// <summary>
        /// Конструктор
        /// </summary>
        public BotSheduler(long confId, DbProxy db) {

            _dbProxy = db;
            _mainConfId = confId;
            _speaker = new UserSpeaker(db);
        }

        public async Task<bool> StartBot(string key)
        {
            _botWorkCT = await StartListen(key);
            return true;
        }

        public async Task<CancellationTokenSource> StartListen(string key)
        {
            _tClient = new TelegramBotClient(key);

            _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
            {
                AllowedUpdates = new[] {
                    UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                },
                // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
                // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
                ThrowPendingUpdates = false,                
            };

            using var cts = new CancellationTokenSource();
            
            _tClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

            var me = await _tClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
            Console.WriteLine($"{me.FirstName} запущен!");

            await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
            
            return cts;

        }

        private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            string botName = (await _tClient.GetMyNameAsync()).Name;

            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            if (update.Message.Chat.Id < 0)
                            {
                                if (update.Message.Chat.Id == _mainConfId)
                                {
                                    if (update.Message != null && update.Message.Text != null && update.Message.Text.Contains(botName))
                                    {
                                        // Хочу получать сообщения вида @bot сумма валюта категория . комментарий
                                        string[] spliting = update.Message.Text.Split('-');
                                        if (spliting.Length != 2)
                                        {
                                            await _tClient.SendTextMessageAsync(update.Message.Chat,
                                                CommonPhraces.GetFormatMessage(botName));
                                            return;
                                        }
                                        string comment = spliting[1];
                                        string[] mainInfos = spliting[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                                        try
                                        {

                                            var node = GetFinanceChangeForInsert(mainInfos, comment);
                                            node.DateOfFixation = update.Message.Date;
                                            node.FixedBy = _dbProxy.GetDbUserIdByUsername(update.Message?.From?.Username);

                                            _dbProxy.InsertFinanceChange(node);
                                            await _tClient.SendTextMessageAsync(update.Message.Chat, "Зафиксировано");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"{DateTime.Now} : {ex}");
                                            await _tClient.SendTextMessageAsync(update.Message.Chat, "Произошла ошибка при фиксации, проверяйте Логи");
                                        }

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Кто-то несанкционно пытается использовать бота");
                                }
                            }
                            else // Личный чат с пользователем
                            {
                                if (update.Message?.Text == null)
                                    await _tClient.SendTextMessageAsync(update.Message.Chat, "Отправлено пустое сообщение");
                                try
                                {
                                    string answer = _speaker.DoSmtg(update.Message?.From.Username ,update.Message?.Text);
                                    await _tClient.SendTextMessageAsync(update.Message.Chat, answer);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"{DateTime.Now} : {ex}");
                                    await _tClient.SendTextMessageAsync(update.Message.Chat, "Произошла ошибка при фиксации, проверяйте Логи");
                                }
                            }
                            
                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        /// <summary>
        /// @{botName} сумма валюта(опционально) категория - Комментарий
        /// </summary>
        private FinanceChange GetFinanceChangeForInsert(string[] messagePart, string comment)
        {
            // Нужно понять что за валюта
            if (messagePart.Length == 4)
            {
                Currency cur = _dbProxy.GetLikelyCurrency(messagePart[2]);
                var sum = double.Parse(messagePart[1]);
                FinanceChange financeChange = new FinanceChange()
                {
                    Summ = sum,
                    CurrencyId = cur.ID,
                    CategoryId = _dbProxy.GetLikelyCategoryId(messagePart[3]),
                    Comment = comment,
                    SumInIternationalCurrency = sum / cur.LastExchangeRate
                };
                return financeChange;
            }

            // Валюта по умолчанию
            if (messagePart.Length == 3)
            {
                Currency cur = _dbProxy.GetDefaultCurrency();
                var sum = double.Parse(messagePart[1]);
                FinanceChange financeChange = new FinanceChange()
                {
                    Summ = sum,
                    CurrencyId = cur.ID,
                    CategoryId = _dbProxy.GetLikelyCategoryId(messagePart[3]),
                    Comment = comment,
                    SumInIternationalCurrency = sum / cur.LastExchangeRate
                };
                return financeChange;
            }

            throw new Exception("Проблема с получением информации о категории");
        }

    }
}
