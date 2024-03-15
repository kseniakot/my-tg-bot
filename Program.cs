// See https://aka.ms/new-console-template for more information
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;
namespace TodoBot
{
    class Program
    {
        public static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            var client = new TelegramBotClient("7183225185:AAE9yLWZQ3DoHWgrSNGn-8vS101RVOuw_8M");
            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        private static bool cex = true;

        private static int teamlead = 0;
        private static int developer = 0;
        private static int tester = 0;
        private static int dataAnalyst = 0;
        private static int neIT = 0;

        private static Dictionary<long, int> userQuestionIndices = new Dictionary<long, int>();
        private static List<string> questions = new List<string> {
            "Какой способ коммуникации вам ближе?:\n1 - Письменная, через электронные сообщения или чаты.\n"+
            "2 - Устная, предпочитаю обсуждать все лично или по телефону.\n"+
            "3 - Не люблю людей\n",

            "Вам нравится исследовать и экспериментировать с новыми технологиями?:\n" +
            "1 - Да, я люблю изучать новые технологии и находить способы их применения.\n" +
            "2 - Нет, я предпочитаю работать с тем, что уже знаю и что хорошо зарекомендовало себя.\n" +
            "3 - Мне ничего не нравится.\n",

            "Как вы относитесь к работе в команде?:\n" +
            "1 - Я предпочитаю работать самостоятельно.\n" +
            "2 - Я люблю работать в команде и считаю, что совместные усилия приводят к лучшим результатам.\n" +
            "3 -  Я не люблю работать\n",

            "Какой аспект разработки ПО вас больше всего привлекает?:\n" +
            "1 - Создание эффективного и оптимизированного кода.\n" +
            "2 - Работа над архитектурой проекта и распределение задач в команде.\n" +
            "3 - Анализ данных и поиск новых закономерностей.\n" +
            "4 - Что такое ПО ?\n",

            "Вам нравится исследовать и экспериментировать с новыми технологиями?:\n" +
            "1 - Да, я люблю изучать новые технологии и находить способы их применения.\n" +
            "2 - Нет, я предпочитаю работать с тем, что уже знаю и что хорошо зарекомендовало себя.\n" +
            "3 - Мне ничего не нравится.\n",

             "Какие мероприятия вы предпочитаете проводить в свободное время?:\n" +
             "1 - Я предпочитаю посещать технические конференции и митапы.\n" +
             "2 - Я предпочитаю проводить время с друзьями или заниматься спортом.\n " +
             "3 - Спать.\n", 
        
             "Что вы считаете важным в процессе разработки ПО?:\n" +
             "1 - Соблюдение сроков и высокое качество кода.\n" +
             "2 - Коммуникация и взаимодействие между членами команды.\n " +
             "3 - Анализ данных и выявление трендов для принятия решений.\n" +
             "4 - Я не хочу в этом участвовать.", 
            
             "Что для вас важнее: инновации или стабильность?:\n" +
             "1 - Инновации - двигатель прогресса!\n" +
             "2 - Стабильность - безопасность и надежность важнее всего.\n ", 
        
             "Что вас больше всего мотивирует в работе?:\n" +
             "1 - Возможность создавать что-то новое и полезное.\n" +
             "2 - Успешное достижение целей и признание ваших достижений.\n " +
             "3 - Зарплата $.\n",
        };
        private static List<List<(string, string)>> options = new List<List<(string, string)>>
     {
        new List<(string, string)> { ("1", "question1_1"), (" 2", "question1_2"), ("3", "question1_3")},
        new List<(string, string)> { ("1", "question2_1"), ("2", "question2_2"), ("3", "question2_3")},
        new List<(string, string)> { ("1", "question3_1"), ("2", "question3_2"), ("3", "question3_3") },
        new List<(string, string)> { ("1", "question4_1"), ("2", "question4_2"), ("3", "question4_3"), ("4", "question4_4") },
        new List<(string, string)> { ("1", "question5_1"), ("2", "question5_2"), ("3", "question5_3") },
        new List<(string, string)> { ("1", "question6_1"), ("2", "question6_2"), ("3", "question6_3") }, 
        new List<(string, string)> { ("1", "question7_1"), ("2", "question7_2"), ("3", "question7_3"), ("4", "question7_4") }, 
        new List<(string, string)> { ("1", "question8_1"), ("2", "question8_2") }, 
        new List<(string, string)> { ("1", "question9_1"), ("2", "question9_2"), ("3", "question9_3") }
    };

        public static int MaxOfFive(int a, int b, int c, int d, int e)
        {
            return Math.Max(Math.Max(Math.Max(Math.Max(a, b), c), d), e);
        }
        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            Console.WriteLine(arg2); // Write the exception to the console
            return Task.CompletedTask;
        }


        private static InlineKeyboardMarkup CreateMenu(List<(string ButtonText, string CallbackData)> buttons)
        {
            var keyboardButtons = buttons.Select(button => new[] { InlineKeyboardButton.WithCallbackData(button.ButtonText, button.CallbackData) }).ToArray();

            var keyboard = new InlineKeyboardMarkup(keyboardButtons);

            return keyboard;
        }

        private async static Task SendOptions(ITelegramBotClient botClient, long chatId, string messageText, List<(string ButtonText, string CallbackData)> buttons)
        {
            var keyboard = CreateMenu(buttons);
            await botClient.SendTextMessageAsync(chatId, messageText, replyMarkup: keyboard);
        }



        private static async void StartTest(ITelegramBotClient botClient, long chatId)
        {

            if (!userQuestionIndices.ContainsKey(chatId))
            {
                userQuestionIndices[chatId] = 0;
            }

            if (userQuestionIndices[chatId] < questions.Count)
            {
                await SendOptions(botClient, chatId, questions[userQuestionIndices[chatId]], options[userQuestionIndices[chatId]]);
                userQuestionIndices[chatId]++;
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, "You have answered all the questions.");
                userQuestionIndices[chatId] = 0;
                int result = MaxOfFive(teamlead, developer, tester, dataAnalyst, neIT);

               if (result == teamlead)
                {
                    
                }
               else if (result == developer)
                {
                    if (cex == true)
                    {
                        using (var stream = new FileStream("Images\\girl-developer.jpg", FileMode.Open))
                        {
                            await botClient.SendPhotoAsync(
                                chatId: chatId,
                                photo: new InputFileStream(stream),
                                caption: "Вы - разработчик!"
                            );
                        }
                    }
                    else
                    {
                        using (var stream = new FileStream("Images\\man-developer.jpeg", FileMode.Open))
                        {
                            await botClient.SendPhotoAsync(
                                chatId: chatId,
                                photo: new InputFileStream(stream),
                                caption: "Вы - разработчик!"
                            );
                        }
                    }
                }
               else if (result == tester)
                {
                    if (cex == true)
                    {
                        using (var stream = new FileStream("Images\\girl-developer.jpg", FileMode.Open))
                        {
                            await botClient.SendPhotoAsync(
                                chatId: chatId,
                                photo: new InputFileStream(stream),
                                caption: "Вы - тестировщик!"
                            );
                        }
                    }
                    else
                    {
                        using (var stream = new FileStream("Images\\man-tester.png", FileMode.Open))
                        {
                            await botClient.SendPhotoAsync(
                                chatId: chatId,
                                photo: new InputFileStream(stream),
                                caption: "Вы - тестировщик!"
                            );
                        }
                    }
                }
               else if (result == dataAnalyst)
                {

                }
               else if (result == neIT)
                {

                }

            }

            
             }

            async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)

        {
           
                //if (update.Message?.Sticker != null)
                //{
                //    Console.WriteLine("Sticker file ID: " + update.Message.Sticker.FileId);
                //}
            
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
                var chatId = callbackQuery.Message.Chat.Id;

                switch (callbackQuery.Data)
                {
                    case "callbackYes":
                        var buttons = new List<(string ButtonText, string CallbackData)>
                        {
                            ("Женский", "callbackW"),
                            ("Мужской", "callbackM")
                        };

                        var keyboard = CreateMenu(buttons);

                        await botClient.SendTextMessageAsync(
                               chatId: callbackQuery.Message.Chat.Id,
                               text: "Укажите свой пол: ",
                               replyMarkup: keyboard
                           );
                        break;

                    case "callbackNo":
                        await botClient.SendStickerAsync(
                                chatId: callbackQuery.Message.Chat.Id,
                                sticker: InputFile.FromString("CAACAgIAAxkBAAOeZfMRnr_778fru7djH6LSaCGt4XUAAoI1AAKGOkBJymwZQAAB6IpbNAQ"));
                        break;

                    case "callbackM":
                        cex = false;
                        await botClient.EditMessageTextAsync(
                        chatId: callbackQuery.Message.Chat.Id,
                        messageId: callbackQuery.Message.MessageId,
                        text: "Пол: Мужской",
                        replyMarkup: callbackQuery.Message.ReplyMarkup);
                        StartTest(botClient, chatId);
                        break;

                    case "callbackW":
                        cex = true;
                        await botClient.EditMessageTextAsync(
                         chatId: callbackQuery.Message.Chat.Id,
                         messageId: callbackQuery.Message.MessageId,
                         text: "Пол: Женский",
                         replyMarkup: callbackQuery.Message.ReplyMarkup);
                        StartTest(botClient, chatId);
                        break;

                    case "question1_1":
                        tester++;
                        dataAnalyst++;
                        developer++;
                        StartTest(botClient, chatId);
                        break;

                    case "question1_2":
                        teamlead++;
                        StartTest(botClient, chatId);
                        break;

                    case "question1_3":
                        neIT++;
                        StartTest(botClient, chatId);
                        break;

                    case "question2_1":
                        developer++;
                        teamlead++;
                        dataAnalyst++;
                        StartTest(botClient, chatId);
                        break;

                    case "question2_2":
                        tester++;
                        neIT++;
                        StartTest(botClient, chatId);
                        break;

                    case "question2_3":
                        neIT++;
                        StartTest(botClient, chatId);
                        break;

                    case "question3_1":
                        dataAnalyst++;
                        developer++;
                        StartTest(botClient, chatId);
                        break;

                    case "question3_2":
                        teamlead++;
                        tester++;
                        StartTest(botClient, chatId);
                        break;

                    case "question3_3":
                        neIT++;
                        StartTest(botClient, chatId);
                        break;

                    case "question4_1":
                        tester++;
                        teamlead++;
                        developer++;
                        StartTest(botClient, chatId);
                        break;

                    case "question4_2":
                        teamlead++;
                        developer++;
                        StartTest(botClient, chatId);
                        break;

                    case "question4_3":
                        dataAnalyst++;
                        StartTest(botClient, chatId);
                        break;

                    case "question4_4":
                        neIT++;
                        StartTest(botClient, chatId);
                        break;

                    case "question5_1":
                        developer++;
                        
                        StartTest(botClient, chatId);
                        break;

                    case "question5_2":
                        tester++;
                        dataAnalyst++;
                        developer++;
                        StartTest(botClient, chatId);
                        break;

                    case "question5_3":
                        teamlead++;
                        neIT++;
                        StartTest(botClient, chatId);
                        break;

                    case "question6_1":
                        teamlead++;
                        developer++;
                        dataAnalyst++;
                        tester++;
                        StartTest(botClient, chatId);
                        break;

                    case "question6_2":
                        teamlead++;
                        neIT++;
                        tester++;
                        StartTest(botClient, chatId);
                        break;

                    case "question6_3":
                        neIT++;
                        StartTest(botClient, chatId);
                        break;

                    case "question7_1":
                        developer++;
                        tester++;
                        StartTest(botClient, chatId);
                        break;

                    case "question7_2":
                        teamlead++;
                       
                        StartTest(botClient, chatId);
                        break;

                    case "question7_3":
                        dataAnalyst++;
                        teamlead++;
                        StartTest(botClient, chatId);
                        break;

                    case "question7_4":
                        neIT++;
                        StartTest(botClient, chatId);
                        break;

                    case "question8_1":
                        teamlead++;
                        developer++;
                        dataAnalyst++;
                        StartTest(botClient, chatId);
                        break;

                    case "question8_2":
                        neIT++;
                        tester++;
                        dataAnalyst++;
                        StartTest(botClient, chatId);
                        break;

                    case "question9_1":
                        teamlead++;
                        developer++;
                        dataAnalyst++;
                        StartTest(botClient, chatId);
                        break;

                    case "question9_2":
                        teamlead++;
                        StartTest(botClient, chatId);
                        break;

                    case "question9_3":
                        neIT++;
                        StartTest(botClient, chatId);
                        break;
                }
               // Console.WriteLine(neIT);

                await botClient.EditMessageReplyMarkupAsync(
                      chatId: callbackQuery.Message.Chat.Id,
                      messageId: callbackQuery.Message.MessageId,
                      replyMarkup: null
                 );
            }   

            else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text != null)
                {
                    if (message.Text.StartsWith("/start"))
                    {
                        var buttons = new List<(string ButtonText, string CallbackData)>
                        {
                            ("Да", "callbackYes"),
                            ("Нет", "callbackNo")
                        };

                        var keyboard = CreateMenu(buttons);

                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Привет! Готов начать тест:",
                            replyMarkup: keyboard
                        );
                    }
                    else if (message.Text.ToLower().Replace(" ", "").Contains("непока"))
                    {
                        var buttons = new List<(string ButtonText, string CallbackData)>
                        {
                            ("Да", "callbackYes"),
                            ("Нет", "callbackNo")
                        };
                        var keyboard = CreateMenu(buttons);

                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Ну, может, тогда тестик ;)",
                            replyMarkup: keyboard
                        );
                    }
                    else if (message.Text.ToLower().Contains("пока") || message.Text.ToLower().Contains("пака"))
                    {
                        await botClient.SendStickerAsync(
                                chatId: message.Chat.Id,
                                sticker: InputFile.FromString("CAACAgIAAxkBAAOeZfMRnr_778fru7djH6LSaCGt4XUAAoI1AAKGOkBJymwZQAAB6IpbNAQ"));
                    }
                }
            }
        }
    }
}
