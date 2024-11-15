Feature: Be. Get Plan

Background:
	Given test user login

Scenario: Be01. Default Plan
	When get the plan
	Then I will receive no core error
		And the plan will be
			| Name | Price | AccountOpened | CategoryEnabled | ScheduleActive | AccountMonthMove | MoveDetail | ArchiveMonthUpload | ArchiveLine | ArchiveSize | OrderMonth | OrderMove |
			| Test | 0     | 10            | 10              | 10             | 10               | 30         | 3                  | 10          | 3000        | 3          | 16        |

Scenario: Be02. User marked for deletion
	Given the user is marked for deletion
	When get the plan
	Then I will receive this core error: UserDeleted

Scenario: Be03. User requested wipe
	Given the user asked data wipe
	When get the plan
	Then I will receive this core error: UserAskedWipe

Scenario: Be04. No user is logged in
	Given I logoff the user
	When get the plan
	Then I will receive this core error: Uninvited

Scenario: Be05. Without signing contract
	Given there is a new contract
	When get the plan
	Then I will receive this core error: NotSignedLastContract

Scenario: Be06. New Plan
	Given these limits in user plan
			| AccountOpened | CategoryEnabled | ScheduleActive | AccountMonthMove | MoveDetail | ArchiveMonthUpload | ArchiveLine | ArchiveSize | OrderMonth | OrderMove |
			| 27            | 27              | 27             | 27               | 27         | 27                 | 27          | 27          | 27         | 27        |
	When get the plan
	Then I will receive no core error
		And the plan will be
			| Name      | Price | AccountOpened | CategoryEnabled | ScheduleActive | AccountMonthMove | MoveDetail | ArchiveMonthUpload | ArchiveLine | ArchiveSize | OrderMonth | OrderMove |
			| Plan be06 | 0     | 27            | 27              | 27             | 27               | 27         | 27                 | 27          | 27          | 27         | 27        |
