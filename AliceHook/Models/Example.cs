using System.Collections.Generic;

namespace AliceHook.Models
{
    public class Example
    {
        public string Title { get; set; }
        public string[] Keywords { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string DescriptionTts { get; set; }

        public static List<Example> List()
        {
            return new List<Example>
            {
                new Example
                {
                    Title = "Отправить письмо с заданным текстом",
                    Keywords = new []
                    {
                        "отправить письмо с текстом",
                        "отправить письмо",
                        "письмо"
                    },
                    Description = "",
                    DescriptionTts = "",
                    Link = ""
                },
                new Example
                {
                    Title = "Добавить задачу в GitHub",
                    Keywords = new []
                    {
                        "добавить задачу в гитхаб",
                        "добавить задачу",
                        "задачу",
                        "гитхаб",
                        "github"
                    },
                    Description = "",
                    DescriptionTts = "",
                    Link = "",
                },
                new Example
                {
                    Title = "Дописать строчку в Google таблицу",
                    Keywords = new []
                    {
                        "дописать строчку в гугл таблицу",
                        "дописать строчку",
                        "строчку",
                        "гугл",
                        "google",
                        "таблица",
                        "гугл таблица",
                        "google таблица"
                    },
                    Description = "",
                    DescriptionTts = "",
                    Link = "",
                },
                new Example
                {
                    Title = "Отправить сообщение в Telegram чат",
                    Keywords = new []
                    {
                        "отправить сообщение",
                        "сообщение в telegram",
                        "сообщение telegram",
                        "сообщение в телеграм",
                        "сообщение телеграм",
                        "telegram",
                        "телеграм"
                    },
                    Description = "",
                    DescriptionTts = "",
                    Link = "",
                },
                new Example
                {
                    Title = "Найти и прочитать заметку Evernote",
                    Keywords = new []
                    {
                        "найти и прочитать заметку эверноут",
                        "найти заметку evernote",
                        "найти заметку эверноут",
                        "прочитать заметку evernote",
                        "прочитать заметку эверноут",
                        "найти заметку",
                        "прочитать заметку",
                        "заметка",
                        "эверноут",
                        "evernote"
                    },
                    Description = "",
                    DescriptionTts = "",
                    Link = "",
                },
                new Example
                {
                    Title = "Узнать телефон из книги контактов",
                    Keywords = new []
                    {
                        "узнать телефон из контактов",
                        "узнать телефон",
                        "контакты"
                    },
                    Description = "",
                    DescriptionTts = "",
                    Link = "",
                },
                new Example
                {
                    Title = "Узнать последнее видео на YouTube канале",
                    Keywords = new []
                    {
                        "узнать последнее видео на ютуб канале",
                        "узнать последнее видео на youtube",
                        "узнать последнее видео на ютуб",
                        "последнее видео на youtube",
                        "последнее видео на ютуб",
                        "последнее видео",
                        "видео",
                        "ютуб",
                        "ютьюб",
                        "youtube",
                        "youtube канал",
                        "ютуб канал",
                        "ютьюб канал"
                    },
                    Description = "",
                    DescriptionTts = "",
                    Link = "",
                }
            };
        }

        public bool Check(string input, bool keywords)
        {
            throw new System.NotImplementedException();
        }
    }
}