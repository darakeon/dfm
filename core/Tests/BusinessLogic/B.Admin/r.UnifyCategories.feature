Feature: Br. Unify categories

Background:
	Given test user login
		And I enable Categories use

Scenario: Br01. Category to keep invalid
	Given I have this category
			| Name |
			| CatB |
	When unify categories CatB to CatA
	Then I will receive this core error: InvalidCategory
		And category CatB will exist

Scenario: Br02. Category to delete invalid
	Given I have this category
			| Name |
			| CatA |
	When unify categories CatB to CatA
	Then I will receive this core error: InvalidCategory
		And category CatA will exist

Scenario: Br03. Category use disabled
	Given I have these categories
			| Name |
			| CatA |
			| CatB |
		But I disable Categories use
	When unify categories CatB to CatA
	Then I will receive this core error: CategoriesDisabled
		And category CatA will exist
		And category CatB will exist

Scenario: Br04. Logged out
	Given I have these categories
			| Name |
			| CatA |
			| CatB |
		And I have no logged user (logoff)
	When unify categories CatB to CatA
	Then I will receive this core error: Uninvited
		And category CatA will exist
		And category CatB will exist

Scenario: Br05. User is marked for deletion
	Given I have these categories
			| Name |
			| CatA |
			| CatB |
		But the user is marked for deletion
	When unify categories CatB to CatA
	Then I will receive this core error: UserDeleted
		And category CatA will exist
		And category CatB will exist

Scenario: Br06. User requested wipe
	Given I have these categories
			| Name |
			| CatA |
			| CatB |
		But the user asked data wipe
	When unify categories CatB to CatA
	Then I will receive this core error: UserAskedWipe
		And category CatA will exist
		And category CatB will exist

Scenario: Br07. All info right
	Given I have these categories
			| Name |
			| CatA |
			| CatB |
	When unify categories CatB to CatA
	Then I will receive no core error
		And category CatA will exist
		And category CatB will not exist

Scenario: Br08. Category to delete has moves
	Given I have these categories
			| Name |
			| CatA |
			| CatB |
		And I have an account
		And I have moves of
			| Value | Category | Detail   | Nature | Date       |
			| 10    | CatA     |          | Out    | 2021-08-15 |
			| 20    | CatA     |          | In     | 2021-08-15 |
			| 30    | CatA     | CatA Out | Out    | 2021-08-15 |
			| 40    | CatA     | CatA In  | In     | 2021-08-15 |
			| 50    | CatB     |          | Out    | 2021-08-15 |
			| 60    | CatB     |          | In     | 2021-08-15 |
			| 70    | CatB     | CatB Out | Out    | 2021-08-15 |
			| 80    | CatB     | CatB In  | In     | 2021-08-15 |
	When unify categories CatB to CatA
	Then I will receive no core error
		And category CatA will exist
		And category CatB will not exist
		And category CatA will have 8 moves

Scenario: Br09. Category to delete has a defective summary
	Given I have these categories
			| Name |
			| CatA |
			| CatB |
		And I have an account
		And category CatB has a defective summary
	When unify categories CatB to CatA
	Then I will receive this core error: CategoryUnifyFail
		And category CatA will exist
		And category CatB will exist

Scenario: Br10. Category to delete has schedules
	Given I have these categories
			| Name |
			| CatA |
			| CatB |
		And I have an account
		And I have schedules of
			| Category | Description   | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| CatA     | Schedule CatA | 2021-08-16 | Out    | 10    | 1     | False     | Monthly   | True            |
			| CatB     | Schedule CatB | 2021-08-16 | In     | 10    | 1     | False     | Monthly   | True            |
	When unify categories CatB to CatA
	Then I will receive no core error
		And category CatA will exist
		And category CatB will not exist
		And category CatA will have 2 schedules

Scenario: Br11. Category to delete same as to keep
	Given I have this category
			| Name |
			| CatA |
	When unify categories CatA to CatA
	Then I will receive this core error: CannotMergeSameCategory
		And category CatA will exist

Scenario: Br12. Category to keep is disabled
	Given I have these categories
			| Name | Enabled |
			| CatA | false   |
			| CatB | true    |
	When unify categories CatB to CatA
	Then I will receive this core error: DisabledCategory
		And category CatA will exist
		And category CatB will exist

Scenario: Br13. New contract not signed
	Given I have these categories
			| Name |
			| CatA |
			| CatB |
		But there is a new contract
	When unify categories CatB to CatA
	Then I will receive this core error: NotSignedLastContract
		And category CatA will exist
		And category CatB will exist
