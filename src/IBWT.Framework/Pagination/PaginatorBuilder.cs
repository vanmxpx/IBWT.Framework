using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace IBWT.Framework.Pagination
{
    public class PaginatorBuilder<T>
    {
        private readonly int itemsPerPage;
        private readonly int pageButtonsCount;
        private readonly string command;
        private PaginatorMessageBuilder<T> messageBuilder;
        private List<InlineKeyboardButton[]> prependRows;
        private List<InlineKeyboardButton[]> appendRows;
        private bool disableBackButton = false;

        public PaginatorBuilder(
            int itemsPerPage,
            int pageButtonsCount,
            string command
        )
        {
            messageBuilder = PaginatorMessageBuilders.DefaultListBuilder;
            this.pageButtonsCount = pageButtonsCount;
            this.command = command;
            this.itemsPerPage = itemsPerPage;
        }

        public PaginatorBuilder<T> MessageBuilder(PaginatorMessageBuilder<T> messageBuilder)
        {
            this.messageBuilder = messageBuilder;
            return this;
        }

        public PaginatorBuilder<T> PrependKeyboardRow(InlineKeyboardButton[] row)
        {
            if(prependRows == null)
                prependRows = new List<InlineKeyboardButton[]>();

            prependRows.Add(row);
            return this;
        }

        public PaginatorBuilder<T> AppendKeyboardRow(InlineKeyboardButton[] row)
        {
            if(appendRows == null)
                appendRows = new List<InlineKeyboardButton[]>();

            appendRows.Add(row);
            return this;
        }
        public PaginatorBuilder<T> DisableBackButton(InlineKeyboardButton[] row)
        {
            disableBackButton = true;
            return this;
        }


        public PaginatorData Build(T[] data, int page)
        {
            int pageDataIndex = itemsPerPage * (page - 1);

            if (pageDataIndex >= data.Length)
                throw new InvalidOperationException("Pagination crossed the boundaries of the data array.");

            PaginatorData paginatorData = new PaginatorData();
            paginatorData.Message = messageBuilder(data, pageDataIndex, itemsPerPage);

            double pagesCount = Math.Ceiling((double)(data.Length / itemsPerPage));
            double startPagination = page - Math.Ceiling((double)(pageButtonsCount / 2));
            if (startPagination < 1)
                startPagination = 1;

            List<InlineKeyboardButton> paginatorRow = new List<InlineKeyboardButton>();
            if (startPagination > 1)
                paginatorRow.Add(InlineKeyboardButton.WithCallbackData("<<", $"{command}::{startPagination - 1}"));

            for (int i = (int)startPagination; i < pageButtonsCount + startPagination; i++)
            {   
                string buttonText;
                if(i == page)
                    buttonText = $"*{i.ToString()}*";
                else 
                    buttonText = i.ToString();
                
                paginatorRow.Add(InlineKeyboardButton.WithCallbackData(buttonText, $"{command}::{i.ToString()}"));
            }

            if (startPagination + pageButtonsCount < pagesCount)
                paginatorRow.Add(InlineKeyboardButton.WithCallbackData(">>", $"{command}::{startPagination + pageButtonsCount + 1}"));

            List<InlineKeyboardButton[]> keyboardRows = new List<InlineKeyboardButton[]>();
            if(prependRows != null)
                keyboardRows.AddRange(prependRows.ToArray());
            keyboardRows.Add(paginatorRow.ToArray());
            if(appendRows != null)
                keyboardRows.AddRange(appendRows.ToArray());

            if(!disableBackButton)
                keyboardRows.Add(new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("back", "back::")
                });

            paginatorData.ReplyMarkup = new InlineKeyboardMarkup(keyboardRows);;
            return paginatorData;
        }
    }
}