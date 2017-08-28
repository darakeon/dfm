Feature: Translation

Background: 
	Given the dictionary is initialized
	And I have these languages
		| Language |
		| en-US    |
		| pt-BR    |


Scenario: 01. Get translations of e-mail names
	Given I have these keys
		| Section | Phrase                   |
		| Email   | MoveNotificationIn       |
		| Email   | MoveNotificationOut      |
		| Email   | MoveNotificationTransfer |
		| Email   | UserVerification         |
		| Email   | PasswordReset            |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 02. Get layouts of e-mails
	Given I have these e-mail types
		| Phrase                   |
		| MoveNotificationIn       |
		| MoveNotificationOut      |
		| MoveNotificationTransfer |
		| UserVerification         |
		| PasswordReset            |
	When I try get the layout
	Then I will receive no multilanguage error


Scenario: 03. Get translations of Enums
	Given I have these keys
		| Section   | Phrase           |
		| Moves     | In               |
		| Moves     | Out              |
		| Moves     | Transfer         |
		| Schedules | Daily            |
		| Schedules | Monthly          |
		| Schedules | Yearly           |
		| Tokens    | UserVerification |
		| Tokens    | PasswordReset    |
	When I try get the translate
	Then I will receive no multilanguage error

Scenario: 04. Get translations of Errors
	Given I have these keys
		| Section | Phrase                         |
		| Error   | Unauthorized                   |
		| Error   | FailOnEmailSend                |
		| Error   | TooLargeData                   |
		| Error   | InvalidToken                   |
		| Error   | InvalidUser                    |
		| Error   | UserAlreadyExists              |
		| Error   | UserEmailInvalid               |
		| Error   | UserPasswordRequired           |
		| Error   | DisabledUser                   |
		| Error   | Uninvited                      |
		| Error   | RetypeWrong                    |
		| Error   | WrongPassword                  |
		| Error   | AccountNameRequired            |
		| Error   | AccountUrlRequired             |
		| Error   | AccountUrlInvalid              |
		| Error   | AccountNameAlreadyExists       |
		| Error   | AccountUrlAlreadyExists        |
		| Error   | CantCloseEmptyAccount          |
		| Error   | CantDeleteAccountWithMoves     |
		| Error   | ClosedAccount                  |
		| Error   | DuplicatedAccountName          |
		| Error   | DuplicatedAccountUrl           |
		| Error   | RedLimitAboveYellowLimit       |
		| Error   | InvalidAccount                 |
		| Error   | CategoriesDisabled             |
		| Error   | MoveCheckDisabled              |
		| Error   | CategoryNameRequired           |
		| Error   | CategoryAlreadyExists          |
		| Error   | DisabledCategory               |
		| Error   | EnabledCategory                |
		| Error   | DuplicatedCategoryName         |
		| Error   | InvalidCategory                |
		| Error   | InMoveWrong                    |
		| Error   | OutMoveWrong                   |
		| Error   | TransferMoveWrong              |
		| Error   | MoveCircularTransfer           |
		| Error   | MoveDescriptionRequired        |
		| Error   | MoveDateRequired               |
		| Error   | MoveDateInvalid                |
		| Error   | MoveValueOrDetailRequired      |
		| Error   | InvalidMove                    |
		| Error   | DetailWithoutParent            |
		| Error   | MoveDetailDescriptionRequired  |
		| Error   | MoveDetailAmountRequired       |
		| Error   | MoveDetailValueRequired        |
		| Error   | InvalidDetail                  |
		| Error   | MoveAlreadyChecked             |
		| Error   | MoveAlreadyUnchecked           |
		| Error   | ScheduleRequired               |
		| Error   | ScheduleFrequencyNotRecognized |
		| Error   | ScheduleWithNoMoves            |
		| Error   | ScheduleTimesCantBeZero        |
		| Error   | SummaryNatureNotFound          |
		| Error   | InvalidYear                    |
		| Error   | InvalidMonth                   |
		| Error   | InvalidSchedule                |
		| Error   | DisabledSchedule               |
		| Error   | LanguageUnknown                |
		| Error   | TimezoneUnknown                |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 05. Get translate of Interface (moves)
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
	Then I will receive no multilanguage error


Scenario: 06. Get translate of Interface (reports)
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
	Then I will receive no multilanguage error


Scenario: 07. Get translate of Interface (accounts)
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
	Then I will receive no multilanguage error


Scenario: 08. Get translate of Interface (categories)
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
	Then I will receive no multilanguage error


Scenario: 09. Get translate of Interface (tokens)
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
	Then I will receive no multilanguage error


Scenario: 10. Get translate of Interface (users)
	Given I have these keys
		| Section | Phrase                  |
		| Users   | ForgotPassword          |
		| Users   | Email                   |
		| Users   | Send                    |
		| Users   | FollowEmailIntructions  |
		| Users   | PayAttentionSender      |
		| Users   | Index_Welcome           |
		| Users   | Index_Introdution       |
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
		| Users   | AndroidRobotLicenseText |
		| Users   | CurrentPassword         |
		| Users   | NewPassword             |
		| Users   | EmailToChange           |
		| Users   | EmailUpdated            |
		| Users   | PasswordChanged         |
		| Users   | MainSettings            |
		| Users   | PasswordSettings        |
		| Users   | EmailSettings           |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 11. Get translate of Interface (ops)
	Given I have these keys
		| Section | Phrase                 |
		| Ops     | NotFound               |
		| Ops     | InternalMessageSent    |
		| Ops     | InternalMessageNotSent |
		| Ops     | CommunicationError     |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 12. Get translate of Interface (general)
	Given I have these keys
		| Section | Phrase      |
		| General | LogOff      |
		| General | MonthMoves  |
		| General | YearMoves   |
		| General | Create      |
		| General | Schedule    |
		| General | Move        |
		| General | Categories  |
		| General | Accounts    |
		| General | ScheduleRun |
		| General | Total       |
		| General | Schedules   |
		| General | TheSchedule |
		| General | Logins      |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 13. Get translate of Interface (schedules)
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
	Then I will receive no multilanguage error


Scenario: 14. Get translate of Interface (logins)
	Given I have these keys
		| Section | Phrase             |
		| Logins  | Creation           |
		| Logins  | Expiration         |
		| Logins  | Type               |
		| Logins  | Mobile             |
		| Logins  | Browser            |
		| Logins  | ConfirmDeleteLogin |
		| Logins  | LoginRegister      |
	When I try get the translate
	Then I will receive no multilanguage error



Scenario: 15. Get translations of E-mail Stati
	Given I have these keys
		| Section | Phrase         |
		| Email   | EmailDisabled  |
		| Email   | EmailSent      |
		| Email   | InvalidSubject |
		| Email   | InvalidBody    |
		| Email   | InvalidAddress |
		| Email   | EmailNotSent   |
	When I try get the translate
	Then I will receive no multilanguage error


