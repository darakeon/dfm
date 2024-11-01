Feature: Hn. Retry order

Background:
	Given test user login
		And I have these accounts
			| Name    |
			| Account |
		And I have moves of
			| Description | Date       | Category | Nature | Out     | In | Value | Conversion | Detail |
			| Sample Move | 1986-03-27 |          | Out    | Account |    | 1     |            |        |

Scenario: Hn01. Unlogged user
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		And robot export the order
		But I have no logged user (logoff)
	When retry order
	Then I will receive this core error: Uninvited

Scenario: Hn02. User marked for deletion
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
		But the user is marked for deletion
	When retry order
	Then I will receive this core error: UserDeleted

Scenario: Hn03. User requested wipe
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
		But the user asked data wipe
	When retry order
	Then I will receive this core error: UserAskedWipe

Scenario: Hn04. Without sign last contract
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
		But there is a new contract
	When retry order
	Then I will receive this core error: NotSignedLastContract

Scenario: Hn05. Invalid order
	When retry order
	Then I will receive this core error: OrderNotFound

Scenario: Hn06. Order wrong user
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		And robot export the order
		But there is a bad person logged in
	When retry order
	Then I will receive this core error: OrderNotFound
		And order status will be Error

Scenario: Hn07. Order pending
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
	When retry order
	Then I will receive this core error: OrderRetryOnlyErrorOutOfLimitCanceled
		And order status will be Pending

Scenario: Hn08. Order success
	Given order start date 1986-03-27
		And order end date 1986-03-27
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
	When retry order
	Then I will receive this core error: OrderRetryOnlyErrorOutOfLimitCanceled
		And order status will be Success

Scenario: Hn09. Order error
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
	When retry order
	Then I will receive no core error
		And order status will be Pending

Scenario: Hn10. Order canceled
	Given order start date 2024-09-15
		And order end date 2024-09-15
		And order account account
		And an export is ordered
		But the order is Canceled
	When retry order
	Then I will receive no core error
		And order status will be Pending

Scenario: Hn11. Order expired
	Given order start date 1986-03-27
		And order end date 1986-03-27
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
		But the order is Expired
	When retry order
	Then I will receive this core error: OrderRetryOnlyErrorOutOfLimitCanceled
		And order status will be Expired

Scenario: Hn12. Order out of limit
	Given order start date 1986-03-27
		And order end date 1986-03-27
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
		And the order is OutOfLimit
	When retry order
	Then I will receive no core error
		And order status will be Pending
