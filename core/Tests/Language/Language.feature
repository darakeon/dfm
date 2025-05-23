﻿Feature: Language

Background:
	Given the dictionary is initialized
		And I have these languages
			| Language |
			| en-US    |
			| pt-BR    |

Scenario: 01. Get layouts of e-mails
	Given I have the e-mail types
		And I have the themes
	When I try get the layout
	Then I will receive no language error

Scenario: 02. Get translations of Enums
	Given I have these enums
			| Section   | Project       | Path       | Enum              |
			| Accounts  | Entities      | Enums      | AccountSign       |
			| General   | Entities      | Enums      | Currency          |
			| Archives  | Entities      | Enums      | ImportStatus      |
			| Moves     | Entities      | Enums      | MoveNature        |
			| Moves     | Entities      | Enums      | PrimalMoveNature  |
			| General   | Entities      | Enums      | OperationType     |
			| Users     | Entities      | Enums      | RemovalReason     |
			| Schedules | Entities      | Enums      | ScheduleFrequency |
			| Schedules | Entities      | Enums      | ScheduleStatus    |
			| Tokens    | Entities      | Enums      | SecurityAction    |
			| Summary   | Entities      | Enums      | SummaryNature     |
			| Logins    | Entities      | Enums      | TicketType        |
			| Tips      | Entities      | Enums      | TipType           |
			| Tips      | Entities      | Enums      | TipBrowser        |
			| Tips      | Entities      | Enums      | TipMobile         |
			| Archives  | Entities      | Enums      | ImportStatus      |
			| Settings  | Generic       |            | Theme             |
			| Settings  | Generic       |            | ThemeBrightness   |
			| Settings  | Generic       |            | ThemeColor        |
			| General   | BusinessLogic | Exceptions | Error             |
			| Email     | Email         |            | EmailStatus       |
	When I try get the translate
	Then I will receive no language error

Scenario: 03. Keys should be in all languages (site)
	Then all keys should be available in all languages at site dictionary

Scenario: 04. Keys should be in all languages (e-mail)
	Then all keys should be available in all languages at e-mail dictionary

Scenario: 05. Keys should not be repeated (site)
	Then no keys should be repeated at site dictionary

Scenario: 06. Keys should not be repeated (e-mail)
	Then no keys should be repeated at e-mail dictionary

Scenario: 07. List translations
	Given I have the phrase Wizard_Test of Wizard section
	When get the list of translations
	Then I will receive no language error
		And it will return these values
			| Language | Value |
			| pt-BR    | F1    |
			| pt-BR    | F2    |
			| pt-BR    | F3    |
			| en-US    | P1    |
			| en-US    | P2    |
			| en-US    | P3    |

Scenario: 08. List translations equal for languages
	Then all lists should be same size in all languages at site dictionary
	Then all lists should be same size in all languages at e-mail dictionary
