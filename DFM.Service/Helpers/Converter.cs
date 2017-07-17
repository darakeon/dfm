using System.Collections.Generic;
using System.Linq;
using DFM.Service.Entities;
using CE = DFM.Core.Entities;

namespace DFM.Service.Helpers
{
    internal static class Converter
    {
        public static Account Cast(this CE.Account account)
        {
            return new Account(account);
        }

        public static Category Cast(this CE.Category category)
        {
            return new Category(category);
        }

        public static Detail Cast(this CE.Detail detail)
        {
            return new Detail(detail);
        }

        public static Month Cast(this CE.Month month)
        {
            return new Month(month);
        }

        public static Move Cast(this CE.Move move)
        {
            return new Move(move);
        }

        public static Summary Cast(this CE.Summary summary)
        {
            return new Summary(summary);
        }

        public static User Cast(this CE.User user)
        {
            return new User(user);
        }

        public static Year Cast(this CE.Year year)
        {
            return new Year(year);
        }



        public static IList<Account> Cast(this IList<CE.Account> accountList)
        {
            return accountList.Select(a => new Account(a)).ToList();
        }

        public static IList<Category> Cast(this IList<CE.Category> categoryList)
        {
            return categoryList.Select(c => new Category(c)).ToList();
        }

        public static IList<Detail> Cast(this IList<CE.Detail> detailList)
        {
            return detailList.Select(d => new Detail(d)).ToList();
        }

        public static IList<Month> Cast(this IList<CE.Month> monthList)
        {
            return monthList.Select(m => new Month(m)).ToList();
        }

        public static IList<Move> Cast(this IList<CE.Move> moveList)
        {
            return moveList.Select(m => new Move(m)).ToList();
        }

        public static IList<Summary> Cast(this IList<CE.Summary> summaryList)
        {
            return summaryList.Select(s => new Summary(s)).ToList();
        }

        public static IList<User> Cast(this IList<CE.User> userList)
        {
            return userList.Select(u => new User(u)).ToList();
        }

        public static IList<Year> Cast(this IList<CE.Year> yearList)
        {
            return yearList.Select(y => new Year(y)).ToList();
        }

    }
}
