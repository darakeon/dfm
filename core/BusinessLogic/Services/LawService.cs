using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Services
{
	public class LawService : Service
	{
		internal LawService(ServiceAccess serviceAccess, Repos repos)
			: base(serviceAccess, repos) { }

		public ContractInfo GetContract()
		{
			var contract = getContract();

			if (contract == null)
				return null;

			return new ContractInfo(contract);
		}

		private Contract getContract()
		{
			return repos.Contract.GetContract();
		}

		public Boolean IsLastContractAccepted()
		{
			var user = parent.Auth.GetCurrent();

			if (user.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (user.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			return IsLastContractAccepted(user);
		}

		internal Boolean IsLastContractAccepted(User user)
		{
			var contract = getContract();

			if (contract == null)
				return true;

			return repos.Acceptance.IsAccepted(user, contract);
		}

		public void AcceptContract()
		{
			inTransaction("AcceptContract", () =>
				AcceptContract(parent.Auth.GetCurrent())
			);
		}

		internal void AcceptContract(User user)
		{
			if (user.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (user.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			var contract = getContract();
			var acceptedNow = repos.Acceptance.Accept(user, contract);

			if (!acceptedNow) return;

			var control = user.Control;
			repos.Control.ResetWarnCounter(control);
		}

		public void SaveAccess()
		{
			var key = parent.Current.TicketKey;
			var ticket = repos.Ticket.GetByKey(key);

			if (ticket == null)
				return;

			var user = ticket.User;

			if (user.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (user.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			inTransaction("SaveAccess", () =>
			{
				ticket.LastAccess = DateTime.UtcNow;
				repos.Ticket.SaveOrUpdate(ticket);

				repos.Control.SaveAccess(user.Control);
			});
		}
	}
}
