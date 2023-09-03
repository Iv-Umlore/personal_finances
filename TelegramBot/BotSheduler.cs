using DataInteraction;
using DataInteraction.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
//using Telegram.Bots.Types;

namespace TelegramBot
{
    public class BotSheduler
    {
        private long _mainConfId;
        private CancellationTokenSource _botWorkCT;

        private ITelegramBotClient _tClient;
        private ReceiverOptions _receiverOptions;

        private DbProxy _dbProxy;

        public async Task<bool> StartBot(string key, long confId, DbProxy db)
        {
            _dbProxy = db;
            _mainConfId = confId;
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
                            if (update.Message.Chat.Id == _mainConfId)
                            {
                                if (update.Message != null && update.Message.Text != null && update.Message.Text.Contains(botName))
                                {
                                    // Хочу получать сообщения вида @bot сумма валюта категория . комментарий
                                    string[] spliting = update.Message.Text.Split('-');
                                    if (spliting.Length != 2)
                                    {
                                        await _tClient.SendTextMessageAsync(update.Message.Chat, GetFormatMessage(botName));
                                        return;
                                    }
                                    string comment = spliting[1];
                                    string[] mainInfos = spliting[0].Split(" ",StringSplitOptions.RemoveEmptyEntries);

                                    try {

                                        var node = GetFinanceChangeForInsert(mainInfos, comment);
                                        // Добавить пользователя и время фиксации к объекту

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

                            // Личный чат с пользователем
                            if (update.Message.Chat.Id > 0)
                            {

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

        private string GetFormatMessage(string botName)
        {
            return $"Ошибка формата!\r\n\r\nОжидаю получение информации о тратах в виде: \r\n@{botName} сумма валюта(опционально) категория - Комментарий : \r\n@{botName} 1000 Еда - Купили мороженое\r\n@{botName} 20.95 лари Кафе - Сходили в Макдоналдс\r\n\r\n Соблюдение пробелов и тире обязательно!";
        }

    }
}
