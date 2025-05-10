using System;
using DFM.BusinessLogic.Response;
using DFM.MVC.Models.Views;

namespace DFM.MVC.Models;

public class OrderRowModel(
	OrderItem order,
	Boolean isUsingCategories,
	WizardHL wizardHL = null
)
{
	public OrderItem Order { get; } = order;
	public Boolean IsUsingCategories { get; } = isUsingCategories;

	# nullable enable
	public WizardHL? WizardHL { get; } = wizardHL;
}