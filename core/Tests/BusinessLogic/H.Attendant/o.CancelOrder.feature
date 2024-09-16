Feature: Ho. Cancel order

Background:
	Given test user login
		And I have these accounts
			| Name    |
			| Account |
		And I have moves of
			| Description | Date       | Category | Nature | Out     | In | Value | Conversion | Detail |
			| Sample Move | 1986-03-27 |          | Out    | Account |    | 1     |            |        |

Scenario: Ho01. Unlogged user
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		But I have no logged user (logoff)
	When cancel order
	Then I will receive this core error: Uninvited

Scenario: Ho02. User marked for deletion
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		But the user is marked for deletion
	When cancel order
	Then I will receive this core error: UserDeleted

Scenario: Ho03. User requested wipe
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		But the user asked data wipe
	When cancel order
	Then I will receive this core error: UserAskedWipe

Scenario: Ho04. Without sign last contract
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		But there is a new contract
	When cancel order
	Then I will receive this core error: NotSignedLastContract

Scenario: Ho05. Invalid order
	When cancel order
	Then I will receive this core error: OrderNotFound

Scenario: Ho06. Order wrong user
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		But there is a bad person logged in
	When cancel order
	Then I will receive this core error: OrderNotFound
		And order status will be Pending

Scenario: Ho07. Order pending
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
	When cancel order
	Then I will receive no core error
		And order status will be Canceled

Scenario: Ho08. Order success
	Given order start date 1986-03-27
		And order end date 1986-03-27
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
	When cancel order
	Then I will receive this core error: OrderCancelNoSuccessExpired
		And order status will be Success

Scenario: Ho09. Order error
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
	When cancel order
	Then I will receive no core error
		And order status will be Canceled

Scenario: Ho10. Order canceled
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		But the order is Canceled
	When cancel order
	Then I will receive no core error
		And order status will be Canceled
		
Scenario: Ho11. Order expired
	Given order start date 1986-03-27
		And order end date 1986-03-27
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
		But the order is Expired
	When cancel order
	Then I will receive this core error: OrderCancelNoSuccessExpired
		And order status will be Expired
