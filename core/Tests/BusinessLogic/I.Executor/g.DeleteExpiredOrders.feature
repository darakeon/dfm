Feature: Ig. Delete Expired Orders

Background:
	Given test user login
		And I have these accounts
			| Name    |
			| Account |
		And I have moves of
			| Description | Date       | Category | Nature | Out     | In | Value |
			| Sample Move | 2024-05-02 |          | Out    | Account |    | 1     |

Scenario: Ig01. Unlogged user
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 90 days ago
		But I have no logged user (logoff)
	When delete expired orders
	Then I will receive this core error: Uninvited
		And order status will be Success

Scenario: Ig02. No robot user
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 90 days ago
		But test user login
	When delete expired orders
	Then I will receive this core error: Uninvited
		And order status will be Success

Scenario: Ig03. User marked for deletion
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 90 days ago
		But the user is marked for deletion
	When robot user login
		And delete expired orders
	Then I will receive this core error: UserDeleted
		And order status will be Success

Scenario: Ig04. User requested wipe
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 90 days ago
		But the user asked data wipe
	When robot user login
		And delete expired orders
	Then I will receive this core error: UserAskedWipe
		And order status will be Success

Scenario: Ig05. Without sign last contract
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 90 days ago
		But there is a new contract
	When robot user login
		And delete expired orders
	Then I will receive no core error
		And order status will be Expired
		And the order file will not exist

Scenario: Ig06. Normal user
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 90 days ago
	When robot user login
		And delete expired orders
	Then I will receive no core error
		And order status will be Expired
		And the order file will not exist

Scenario: Ig07. Not yet exported
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
	When robot user login
		And delete expired orders
	Then I will receive no core error
		And order status will be Pending

Scenario: Ig08. Not yet expired
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 27 days ago
	When robot user login
		And delete expired orders
	Then I will receive no core error
		And order status will be Success
		And the order file will exist

Scenario: Ig09. Order errored
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 90 days ago
		But the order is Error
	When robot user login
		And delete expired orders
	Then I will receive no core error
		And order status will be Expired
		And the order file will not exist

Scenario: Ig10. Order canceled
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 90 days ago
		But the order is Canceled
	When robot user login
		And delete expired orders
	Then I will receive no core error
		And order status will be Expired
		And the order file will not exist

Scenario: Ig11. Order expired
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 90 days ago
		But the order is Expired
	When robot user login
		And delete expired orders
	Then I will receive no core error
		And order status will be Expired

Scenario: Ig12. Order out of limit
	Given order start date 2023-01-01
		And order end date 2024-09-06
		And order account account
		And an export is ordered
		And the order is exported 90 days ago
		But the order is OutOfLimit
	When robot user login
		And delete expired orders
	Then I will receive no core error
		And order status will be Expired
