Feature: Hp. Download order

Background:
	Given test user login
		And I have these accounts
			| Name    |
			| Account |
		And I have moves of
			| Description | Date       | Category | Nature | Out     | In | Value | Conversion | Detail |
			| Sample Move | 1986-03-27 |          | Out    | Account |    | 1     |            |        |

Scenario: Hp01. Unlogged user
	Given order start date 2024-09-19
		And order end date 2024-09-19
		And order account account
		And an export is ordered
		But I have no logged user (logoff)
	When download order
	Then I will receive this core error: Uninvited

Scenario: Hp02. User marked for deletion
	Given order start date 2024-09-19
		And order end date 2024-09-19
		And order account account
		And an export is ordered
		But the user is marked for deletion
	When download order
	Then I will receive this core error: UserDeleted

Scenario: Hp03. User requested wipe
	Given order start date 2024-09-19
		And order end date 2024-09-19
		And order account account
		And an export is ordered
		But the user asked data wipe
	When download order
	Then I will receive this core error: UserAskedWipe

Scenario: Hp04. Without sign last contract
	Given order start date 2024-09-19
		And order end date 2024-09-19
		And order account account
		And an export is ordered
		But there is a new contract
	When download order
	Then I will receive this core error: NotSignedLastContract

Scenario: Hp05. Invalid order
	When download order
	Then I will receive this core error: OrderNotFound

Scenario: Hp06. Order wrong user
	Given order start date 2024-09-19
		And order end date 2024-09-19
		And order account account
		And an export is ordered
		But there is a bad person logged in
	When download order
	Then I will receive this core error: OrderNotFound
		And order status will be Pending

Scenario: Hp07. Order pending
	Given order start date 2024-09-19
		And order end date 2024-09-19
		And order account account
		And an export is ordered
	When download order
	Then I will receive this core error: OrderDownloadOnlySuccess
		And order will not be downloaded

Scenario: Hp08. Order success
	Given order start date 1986-03-27
		And order end date 1986-03-27
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
	When download order
	Then I will receive no core error
		And order will be downloaded

Scenario: Hp09. Order error
	Given order start date 2024-09-19
		And order end date 2024-09-19
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
	When download order
	Then I will receive this core error: OrderDownloadOnlySuccess
		And order will not be downloaded

Scenario: Hp10. Order canceled
	Given order start date 2024-09-19
		And order end date 2024-09-19
		And order account account
		And an export is ordered
		But the order is Canceled
	When download order
	Then I will receive this core error: OrderDownloadOnlySuccess
		And order will not be downloaded

Scenario: Hp11. Order expired
	Given order start date 1986-03-27
		And order end date 1986-03-27
		And order account account
		And an export is ordered
		And robot export the order
		And test user login
		But the order is Expired
	When download order
	Then I will receive this core error: OrderDownloadOnlySuccess
		And order will not be downloaded

Scenario: Hp12. Order file deleted
	Given order start date 1986-03-27
		And order end date 1986-03-27
		And order account account
		And an export is ordered
		And robot export the order
		But the order file is deleted
		And test user login
	When download order
	Then I will receive this core error: OrderFileDeleted
		And order will not be downloaded
