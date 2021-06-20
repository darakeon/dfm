Feature: Language

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
			| Moves     | Entities      | Enums      | MoveNature        |
			| Schedules | Entities      | Enums      | ScheduleFrequency |
			| Users     | Entities      | Enums      | RemovalReason     |
			| Tokens    | Entities      | Enums      | SecurityAction    |
			| General   | Entities      | Enums      | OperationType     |
			| Users     | Generic       |            | Theme             |
			| Users     | Generic       |            | ThemeBrightness   |
			| Users     | Generic       |            | ThemeColor        |
			| General   | BusinessLogic | Exceptions | Error             |
	When I try get the translate
	Then I will receive no language error

Scenario: 03. Get translations of E-mail Stati
	Given I have these keys
			| Section | Phrase         |
			| Email   | EmailDisabled  |
			| Email   | EmailSent      |
			| Email   | InvalidSubject |
			| Email   | InvalidBody    |
			| Email   | InvalidAddress |
			| Email   | EmailNotSent   |
	When I try get the translate
	Then I will receive no language error

Scenario: 04. Keys should be in all languages (site)
	Then all keys should be available in all languages at site dictionary

Scenario: 05. Keys should be in all languages (e-mail)
	Then all keys should be available in all languages at e-mail dictionary

Scenario: 06. Keys should not be repeated (site)
	Then no keys should be repeated at site dictionary

Scenario: 07. Keys should not be repeated (e-mail)
	Then no keys should be repeated at e-mail dictionary
