using System;
using System.Linq;
using Ak.Generic.Exceptions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Generic;

namespace DFM.BusinessLogic.Services
{
    internal class BaseMoveSaverService : BaseService
    {
        private readonly MoveRepository moveRepository;
        private readonly DetailRepository detailRepository;
        private readonly SummaryRepository summaryRepository;
        private readonly MonthRepository monthRepository;
        private readonly YearRepository yearRepository;

        internal BaseMoveSaverService(ServiceAccess serviceAccess, MoveRepository moveRepository, DetailRepository detailRepository, SummaryRepository summaryRepository, MonthRepository monthRepository, YearRepository yearRepository)
            : base(serviceAccess)
        {
            this.moveRepository = moveRepository;
            this.detailRepository = detailRepository;
            this.summaryRepository = summaryRepository;
            this.monthRepository = monthRepository;
            this.yearRepository = yearRepository;
        }



        internal ComposedResult<Move, EmailStatus> SaveOrUpdateMove(Move move, String accountOutUrl, String accountInUrl, String categoryName)
        {
            var operationType =
                move.ID == 0
                    ? OperationType.Creation
                    : OperationType.Edit;

            resetSchedule(move);

            linkEntities(move, accountOutUrl, accountInUrl, categoryName);

            var oldMove = moveRepository.GetOldById(move.ID);

            if (move.ID == 0 || !move.IsDetailed())
            {
                move = moveRepository.SaveOrUpdate(move);
                detailRepository.SaveDetails(move);
            }
            else
            {
                detailRepository.SaveDetails(move);
                move = moveRepository.SaveOrUpdate(move);
            }


            if (oldMove != null)
                breakSummaries(oldMove);

            breakSummaries(move);

            var emailStatus = SendEmail(move, operationType);

            return new ComposedResult<Move, EmailStatus>(move, emailStatus);
        }

        internal Account GetAccountByUrl(String accountUrl)
        {
            return accountUrl == null
                ? null
                : Parent.Admin.GetAccountByUrl(accountUrl);
        }

        internal Category GetCategoryByName(String categoryName)
        {
            if (Parent.Current.User.Config.UseCategories)
                return Parent.Admin.GetCategoryByName(categoryName);

            if (!String.IsNullOrEmpty(categoryName))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CategoriesDisabled);

            return null;
        }

        private void resetSchedule(Move move)
        {
            if (move.ID == 0) return;

            var oldMove = moveRepository.GetOldById(move.ID);

            move.Schedule = oldMove == null
                ? null : oldMove.Schedule;
        }



        private void linkEntities(Move move, String accountOutUrl, String accountInUrl, String categoryName)
        {
            move.Category = GetCategoryByName(categoryName);

            var accountOut = GetAccountByUrl(accountOutUrl);
            var monthOut = accountOut == null ? null : getMonth(move, accountOut);

            var accountIn = GetAccountByUrl(accountInUrl);
            var monthIn = accountIn == null ? null : getMonth(move, accountIn);

            moveRepository.PlaceMonthsInMove(move, monthOut, monthIn);
        }

        private Month getMonth(Move move, Account account)
        {
            if (move.Date == DateTime.MinValue)
                return null;

            var year = yearRepository.GetOrCreateYearWithSummary((Int16)move.Date.Year, account, move.Category);

            return monthRepository.GetOrCreateMonthWithSummary((Int16)move.Date.Month, year, move.Category);
        }



        internal void BreakSummaries(Move move)
        {
            var oldMove = moveRepository.GetOldById(move.ID);

            breakSummaries(oldMove);
        }

        private void breakSummaries(Move move)
        {
            if (move.Nature != MoveNature.Out)
            {
                summaryRepository.CreateIfNotExists(move.In, move.Category);

                summaryRepository.Break(move.In, move.Category);
                summaryRepository.Break(move.In.Year, move.Category);
            }

            if (move.Nature != MoveNature.In)
            {
                summaryRepository.CreateIfNotExists(move.Out, move.Category);

                summaryRepository.Break(move.Out, move.Category);
                summaryRepository.Break(move.Out.Year, move.Category);
            }

        }


        internal EmailStatus SendEmail(Move move, OperationType operationType)
        {
            var emailAction = getEmailAction(operationType);

            return moveRepository.SendEmail(move, emailAction);
        }

        private static string getEmailAction(OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Creation:
                    return "create_move";
                case OperationType.Edit:
                    return "edit";
                case OperationType.Delete:
                    return "delete";
                default:
                    throw new NotImplementedException();
            }
        }

        

        internal void FixSummaries()
        {
            BeginTransaction();

            try
            {
                var summaryList = 
                    summaryRepository
                        .List(s => s.Broken)
                        .Where(s => s.User() == Parent.Current.User);

                foreach (var summary in summaryList)
                {
                    summaryRepository.Fix(summary);
                }

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }


        }


    }
}
