using DataInteraction;
using Services.Interfaces;
using Services.Realization;
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

        private PersonalitySpeaker _personalSpeaker;
        private GroupChatSpeaker _groupSpeaker;
        private ILimitService _limitService;

        // Обработать чтобы не было вариантов Null

        /// <summary>
        /// Конструктор
        /// </summary>
        public BotSheduler(long confId, DbProxy db) 
        {
            _mainConfId = confId;
            _personalSpeaker = new PersonalitySpeaker(db);
            _groupSpeaker = new GroupChatSpeaker(db);
            _limitService = new LimitService(db);
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
                                    if (update.Message != null && update.Message.Text != null)
                                    {
                                        try { 
                                            string answer = _groupSpeaker.DoSmtg(
                                                update.Message.From.Username,
                                                update.Message.Text,
                                                botName, update.Message.Date);

                                            if (!string.IsNullOrEmpty(answer))
                                            {
                                                if (answer != CommonPhrases.DoneMessage)
                                                    answer += CommonPhrases.CanсelCommand;
                                                else
                                                {
                                                    answer += "\r\n" + CommonPhrases.LimitMessage + "\r\n" +
                                                        _limitService.GetActiveLimitResult();
                                                }
                                                await _tClient.SendTextMessageAsync(update.Message.Chat, answer);
                                            }
                                        
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
                                    string answer = _personalSpeaker.DoSmtg(update.Message?.From.Username ,update.Message?.Text);
                                    if (answer != CommonPhrases.DoneMessage)
                                        answer += CommonPhrases.CanсelCommand;
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

        // TODO: Переаботать систему выбора категории. Пользователь может сам выбрать из предложенных
    }
}
