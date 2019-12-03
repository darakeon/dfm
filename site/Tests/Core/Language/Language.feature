Feature: Language

Background:
	Given the dictionary is initialized
		And I have these languages
			| Language |
			| en-US    |
			| pt-BR    |

Scenario: L01. Get layouts of e-mails
	Given I have these e-mail types
			| Phrase           |
			| MoveNotification |
			| SecurityAction   |
		And I have these themes
			| Phrase |
			| Dark   |
			| Light  |
	When I try get the layout
	Then I will receive no language error

Scenario: L02. Get translations of Enums
	Given I have these entity enums
			| Section   | Enum              |
			| Moves     | MoveNature        |
			| Schedules | ScheduleFrequency |
			| Tokens    | SecurityAction    |
			| General   | OperationType     |
	When I try get the translate
	Then I will receive no language error

Scenario: L03. Get translations of Errors
	Given I have the error enum
	When I try get the translate
	Then I will receive no language error

Scenario: L04. Get translate of Interface (moves)
	Given I have these keys
			| Section | Phrase                  |
			| Moves   | Create                  |
			| Moves   | Edit                    |
			| Moves   | Move                    |
			| Moves   | DayNames                |
			| Moves   | Description             |
			| Moves   | CharactersMax           |
			| Moves   | Date                    |
			| Moves   | Category                |
			| Moves   | Select                  |
			| Moves   | Create                  |
			| Moves   | Category                |
			| Moves   | Nature                  |
			| Moves   | JustOneValue            |
			| Moves   | Detailed                |
			| Moves   | Add                     |
			| Moves   | Detail                  |
			| Moves   | Value                   |
			| Moves   | Amount                  |
			| Moves   | ToMove                  |
			| Moves   | MoveSave                |
			| Moves   | MoveNotFound            |
			| Moves   | MoveDeleted             |
			| Moves   | MoveDeletedWithoutEmail |
	When I try get the translate
	Then I will receive no language error

Scenario: L05. Get translate of Interface (reports)
	Given I have these keys
			| Section | Phrase            |
			| Reports | ShortDateFormat   |
			| Reports | Summary           |
			| Reports | Go                |
			| Reports | NoMonthMoves      |
			| Reports | Description       |
			| Reports | Category          |
			| Reports | Date              |
			| Reports | In                |
			| Reports | Out               |
			| Reports | TODO              |
			| Reports | Edit              |
			| Reports | Move              |
			| Reports | Delete            |
			| Reports | ConfirmDeleteMove |
			| Reports | NoYearMoves       |
			| Reports | Month             |
			| Reports | Value             |
			| Reports | ToMove            |
			| Reports | Check             |
			| Reports | Uncheck           |
			| Reports | Checked           |
			| Reports | Unchecked         |
	When I try get the translate
	Then I will receive no language error

Scenario: L06. Get translate of Interface (accounts)
	Given I have these keys
			| Section  | Phrase                |
			| Accounts | Create                |
			| Accounts | Edit                  |
			| Accounts | Account               |
			| Accounts | Name                  |
			| Accounts | CharactersMax         |
			| Accounts | AccountHasLimit       |
			| Accounts | RedLimit              |
			| Accounts | YellowLimit           |
			| Accounts | LimitExplanation      |
			| Accounts | Accounts              |
			| Accounts | NoAccounts            |
			| Accounts | BeginDate             |
			| Accounts | Value                 |
			| Accounts | TODO                  |
			| Accounts | Create                |
			| Accounts | Move                  |
			| Accounts | Schedule              |
			| Accounts | Edit                  |
			| Accounts | Account               |
			| Accounts | Delete                |
			| Accounts | Close                 |
			| Accounts | DeleteConfirm         |
			| Accounts | CloseConfirm          |
			| Accounts | ClosedAccounts        |
			| Accounts | NoClosedAccounts      |
			| Accounts | Name                  |
			| Accounts | BeginDate             |
			| Accounts | EndDate               |
			| Accounts | Value                 |
			| Accounts | GOTO                  |
			| Accounts | MonthMoves            |
			| Accounts | YearMoves             |
			| Accounts | Url                   |
			| Accounts | AcceptedUrlCharacters |
	When I try get the translate
	Then I will receive no language error

Scenario: L07. Get translate of Interface (categories)
	Given I have these keys
			| Section    | Phrase        |
			| Categories | Create        |
			| Categories | Edit          |
			| Categories | Category      |
			| Categories | CharactersMax |
			| Categories | Categories    |
			| Categories | NoCategories  |
			| Categories | Name          |
			| Categories | TODO          |
			| Categories | Enable        |
			| Categories | Disable       |
			| Categories | Create        |
	When I try get the translate
	Then I will receive no language error

Scenario: L08. Get translate of Interface (tokens)
	Given I have these keys
			| Section | Phrase                  |
			| Tokens  | NotRecognizedAction     |
			| Tokens  | TokenDisable            |
			| Tokens  | TokenDisableSuccess     |
			| Tokens  | TokenInvalid            |
			| Tokens  | TokenInvalidMessage     |
			| Tokens  | PasswordReset           |
			| Tokens  | Password                |
			| Tokens  | RetypePassword          |
			| Tokens  | Reset                   |
			| Tokens  | PasswordResetSuccess    |
			| Tokens  | TokenReceivedByEmail    |
			| Tokens  | Token                   |
			| Tokens  | SecurityAction          |
			| Tokens  | Select                  |
			| Tokens  | Go                      |
			| Tokens  | UserVerification        |
			| Tokens  | UserVerificationSuccess |
	When I try get the translate
	Then I will receive no language error

Scenario: L09. Get translate of Interface (users)
	Given I have these keys
			| Section | Phrase                  |
			| Users   | ForgotPassword          |
			| Users   | Email                   |
			| Users   | Send                    |
			| Users   | FollowEmailInstructions |
			| Users   | PayAttentionSender      |
			| Users   | Index_Welcome           |
			| Users   | Index_Introduction      |
			| Users   | Index_Explanation       |
			| Users   | Index_Close             |
			| Users   | Logon                   |
			| Users   | Login                   |
			| Users   | Enter                   |
			| Users   | Email                   |
			| Users   | Password                |
			| Users   | RememberMe              |
			| Users   | ForgotPassword          |
			| Users   | TokenReceivedByEmail    |
			| Users   | LogonDisabled           |
			| Users   | SignUp                  |
			| Users   | Email                   |
			| Users   | CharactersMax           |
			| Users   | Password                |
			| Users   | RetypePassword          |
			| Users   | Create                  |
			| Users   | SignUpSuccess           |
			| Users   | Settings                |
			| Users   | UseCategories           |
			| Users   | SendMoveEmail           |
			| Users   | MoveCheck               |
			| Users   | Language                |
			| Users   | TimeZone                |
			| Users   | LanguageEnus            |
			| Users   | LanguagePtbr            |
			| Users   | Save                    |
			| Users   | ConfigChanged           |
			| Users   | CurrentPassword         |
			| Users   | NewPassword             |
			| Users   | EmailToChange           |
			| Users   | EmailUpdated            |
			| Users   | PasswordChanged         |
			| Users   | OptionsSettings         |
			| Users   | PasswordSettings        |
			| Users   | EmailSettings           |
	When I try get the translate
	Then I will receive no language error

Scenario: L10. Get translate of Interface (ops)
	Given I have these keys
			| Section | Phrase                 |
			| Ops     | NotFound               |
			| Ops     | InternalMessageSent    |
			| Ops     | InternalMessageNotSent |
			| Ops     | CommunicationError     |
	When I try get the translate
	Then I will receive no language error

Scenario: L11. Get translate of Interface (general)
	Given I have these keys
			| Section | Phrase                  |
			| General | LogOff                  |
			| General | MonthMoves              |
			| General | YearMoves               |
			| General | Create                  |
			| General | Schedule                |
			| General | Move                    |
			| General | Categories              |
			| General | Accounts                |
			| General | ScheduleRun             |
			| General | Total                   |
			| General | Schedules               |
			| General | TheSchedule             |
			| General | Logins                  |
			| General | AndroidRobotLicenseText |
			| General | Status                  |
			| General | Available               |
			| General | Invisible               |
	When I try get the translate
	Then I will receive no language error

Scenario: L12. Get translate of Interface (schedules)
	Given I have these keys
			| Section   | Phrase                |
			| Schedules | Create                |
			| Schedules | Edit                  |
			| Schedules | Schedule              |
			| Schedules | Move                  |
			| Schedules | DayNames              |
			| Schedules | Frequency             |
			| Schedules | Boundless             |
			| Schedules | Repeat                |
			| Schedules | Times                 |
			| Schedules | ShowInstallment       |
			| Schedules | Description           |
			| Schedules | CharactersMax         |
			| Schedules | Date                  |
			| Schedules | Category              |
			| Schedules | Select                |
			| Schedules | Create                |
			| Schedules | Category              |
			| Schedules | Nature                |
			| Schedules | JustOneValue          |
			| Schedules | Detailed              |
			| Schedules | Add                   |
			| Schedules | Detail                |
			| Schedules | Value                 |
			| Schedules | Amount                |
			| Schedules | ToMove                |
			| Schedules | NoSchedules           |
			| Schedules | ConfirmDeleteSchedule |
	When I try get the translate
	Then I will receive no language error

Scenario: L13. Get translate of Interface (logins)
	Given I have these keys
			| Section | Phrase             |
			| Logins  | Creation           |
			| Logins  | Expiration         |
			| Logins  | Type               |
			| Logins  | Mobile             |
			| Logins  | Browser            |
			| Logins  | ConfirmDeleteLogin |
			| Logins  | LoginRegister      |
			| Logins  | NoLogins           |
	When I try get the translate
	Then I will receive no language error

Scenario: L14. Get translations of E-mail Stati
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

Scenario: L15. Keys should be in all languages (site)
	Then all keys should be available in all languages at site dictionary

Scenario: L16. Keys should be in all languages (e-mail)
	Then all keys should be available in all languages at e-mail dictionary

Scenario: L17. Keys should not be repeated (site)
	Then no keys should be repeated at site dictionary

Scenario: L18. Keys should not be repeated (e-mail)
	Then no keys should be repeated at e-mail dictionary
