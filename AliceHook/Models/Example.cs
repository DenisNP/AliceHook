using System;
using System.Collections.Generic;
using System.Linq;

namespace AliceHook.Models
{
    public class Example
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string DescriptionTts { get; set; }

        private static readonly List<Example> Examples = new List<Example>
        {
            new Example
            {
                Title = "Отправить письмо с заданным текстом",
                Description = "Вы можете отправить письмо на заранее заданный адрес с тем текстом, который скажете.",
                DescriptionTts = "Вы можете отправить письмо на заранее заданный адрес с тем текстом, который скажете.",
                Link = "https://youtu.be/Cs43jCh8VEA"
            },
            new Example
            {
                Title = "Добавить задачу в GitHub",
                Description =
                    "Добавление нового issue в GitHub к заданному репозиторию с тем текстом, который вы произнесёте.",
                DescriptionTts =
                    "Добавление нового ишью в гитхаб к заданному репозиторию с тем текстом, который вы произнесёте.",
                Link = "https://youtu.be/9TA5xvn1IFI"
            },
            new Example
            {
                Title = "Дописать строчку в Google таблицу",
                Description = "Если вы составляете какой-то список, для которого нужен быстрый ввод голосом, то " +
                              "вам поможет комбинация данного навыка и Google таблиц.",
                DescriptionTts = "Если вы составляете какой-то список, для которого нужен быстрый ввод голосом, то " +
                                 "вам поможет комбинация данного навыка и гугл таблиц.",
                Link = ""
            },
            new Example
            {
                Title = "Отправить сообщение в Telegram чат",
                Description = "Вы можете создать Telegram-бота, который будет отправлять в заданный чат фразы, " +
                              "сказанные голосом. Это может быть полезно для уведомления группы людей о чём-то срочном.",
                DescriptionTts = "Вы можете создать телеграм б+ота, который будет отправлять в заданный чат фразы, " +
                                 "сказанные голосом. Это может быть полезно для уведомления группы людей о чём-то срочном.",
                Link = ""
            },
            new Example
            {
                Title = "Найти и прочитать заметку Evernote",
                Description = "Можно найти и прочитать с помощью Алисы заметку из сервиса Evernote, не прикасаясь " +
                              "к экрану и не смотря на него.",
                DescriptionTts = "Можно найти и прочитать с помощью Алисы заметку из сервиса эверноут, не прикасаясь " +
                                 "к экрану и не смотря на него.",
                Link = ""
            },
            new Example
            {
                Title = "Узнать телефон из книги контактов",
                Description =
                    "Функция быстрого чтения вслух любого номера телефона по имени из книги контактов Google.",
                DescriptionTts =
                    "Функция быстрого чтения вслух любого номера телефона по имени из книги контактов гугл.",
                Link = "https://youtu.be/ugM0HiI28wM"
            },
            new Example
            {
                Title = "Узнать последнее видео на YouTube канале",
                Description = "По голосовому запросу вы можете узнать название и дату добавления последнего ролика " +
                              "на заданном канале YouTube.",
                DescriptionTts =
                    "По голосовому запросу вы можете узнать название и дату добавления последнего ролика " +
                    "на заданном канале ютуб.",
                Link = "https://youtu.be/NBfIgCVQrnw"
            }
        };

        public static IEnumerable<Example> List()
        {
            return Examples.Where(x => !x.Link.IsNullOrEmpty());
        }

        public bool Check(string input)
        {
            return string.Equals(input, Title, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}