using DataInteraction;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot
{
    public class BotSheduler
    {
        private CancellationTokenSource _botWorkCT;

        private ITelegramBotClient _tClient;
        private ReceiverOptions _receiverOptions;

        private DbProxy _dbProxy;

        public async Task<bool> StartBot(string key, DbProxy db)
        {
            _dbProxy = db;
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
                            if (update.Message != null && update.Message.Text.Contains(botName))
                            {
                                Console.WriteLine($"Пришло сообщение для меня!\r\n{update.Message?.Text}");

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


    }
}
