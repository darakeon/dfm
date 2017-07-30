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
		| Section | Phrase           |
		| Move    | In               |
		| Move    | Out              |
		| Move    | Transfer         |
		| Move    | Daily            |
		| Move    | Monthly          |
		| Move    | Yearly           |
		| Token   | UserVerification |
		| Token   | PasswordReset    |
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
		| Error   | InvalidTicket                  |
		| Error   | AccountNameRequired            |
		| Error   | AccountUrlRequired             |
		| Error   | AccountUrlInvalid              |
		| Error   | AccountAlreadyExists           |
		| Error   | AccountUrlAlreadyExists        |
		| Error   | CantCloseEmptyAccount          |
		| Error   | CantDeleteAccountWithMoves     |
		| Error   | ClosedAccount                  |
		| Error   | DuplicatedAccountName          |
		| Error   | DuplicatedAccountUrl           |
		| Error   | RedLimitAboveYellowLimit       |
		| Error   | InvalidAccount                 |
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
		| Error   | DetailWithoutMove              |
		| Error   | MoveDetailDescriptionRequired  |
		| Error   | MoveDetailAmountRequired       |
		| Error   | MoveDetailValueRequired        |
		| Error   | InvalidDetail                  |
		| Error   | ScheduleRequired               |
		| Error   | ScheduleFrequencyNotRecognized |
		| Error   | ScheduleWithNoMoves            |
		| Error   | ScheduleTimesCantBeZero        |
		| Error   | SummaryNatureNotFound          |
		| Error   | InvalidYear                    |
		| Error   | InvalidMonth                   |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 05. Get translate of Interface (move)
	Given I have these keys
		| Section | Phrase          |
		| Move    | Create          |
		| Move    | Edit            |
		| Move    | Schedule        |
		| Move    | Move            |
		| Move    | DayNames        |
		| Move    | Frequency       |
		| Move    | Boundless       |
		| Move    | Repeat          |
		| Move    | Times           |
		| Move    | ShowInstallment |
		| Move    | Description     |
		| Move    | CharactersMax   |
		| Move    | Date            |
		| Move    | Category        |
		| Move    | Select          |
		| Move    | Create          |
		| Move    | Category        |
		| Move    | Nature          |
		| Move    | JustOneValue    |
		| Move    | Detailed        |
		| Move    | Add             |
		| Move    | Detail          |
		| Move    | Value           |
		| Move    | Amount          |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 06. Get translate of Interface (report)
	Given I have these keys
		| Section | Phrase            |
		| Report  | ShortDateFormat   |
		| Report  | Summary           |
		| Report  | Go                |
		| Report  | NoMonthMoves      |
		| Report  | Description       |
		| Report  | Category          |
		| Report  | Date              |
		| Report  | In                |
		| Report  | Out               |
		| Report  | TODO              |
		| Report  | Edit              |
		| Report  | Move              |
		| Report  | Delete            |
		| Report  | ConfirmDeleteMove |
		| Report  | NoYearMoves       |
		| Report  | Month             |
		| Report  | Value             |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 07. Get translate of Interface (account)
	Given I have these keys
		| Section | Phrase                |
		| Account | Create                |
		| Account | Edit                  |
		| Account | Account               |
		| Account | Name                  |
		| Account | CharactersMax         |
		| Account | AccountHasLimit       |
		| Account | RedLimit              |
		| Account | YellowLimit           |
		| Account | LimitExplanation      |
		| Account | Accounts              |
		| Account | NoAccounts            |
		| Account | BeginDate             |
		| Account | Value                 |
		| Account | TODO                  |
		| Account | Create                |
		| Account | Move                  |
		| Account | Schedule              |
		| Account | Edit                  |
		| Account | Account               |
		| Account | Delete                |
		| Account | Close                 |
		| Account | CloseConfirm          |
		| Account | DeleteConfirm         |
		| Account | ClosedAccounts        |
		| Account | NoClosedAccounts      |
		| Account | Name                  |
		| Account | BeginDate             |
		| Account | EndDate               |
		| Account | Value                 |
		| Account | GOTO                  |
		| Account | MonthMoves            |
		| Account | YearMoves             |
		| Account | Url                   |
		| Account | AcceptedUrlCharacters |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 08. Get translate of Interface (category)
	Given I have these keys
		| Section  | Phrase        |
		| Category | Create        |
		| Category | Edit          |
		| Category | Category      |
		| Category | CharactersMax |
		| Category | Categories    |
		| Category | NoCategories  |
		| Category | Name          |
		| Category | TODO          |
		| Category | Enable        |
		| Category | Disable       |
		| Category | Create        |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 09. Get translate of Interface (token)
	Given I have these keys
		| Section | Phrase                  |
		| Token   | RetypeWrong             |
		| Token   | NotRecognizedAction     |
		| Token   | RetypeWrong             |
		| Token   | NotRecognizedAction     |
		| Token   | TokenDisable            |
		| Token   | TokenDisableSuccess     |
		| Token   | TokenInvalid            |
		| Token   | TokenInvalidMessage     |
		| Token   | PasswordReset           |
		| Token   | Password                |
		| Token   | RetypePassword          |
		| Token   | Reset                   |
		| Token   | PasswordReset           |
		| Token   | PasswordResetSuccess    |
		| Token   | TokenReceivedByEmail    |
		| Token   | Token                   |
		| Token   | SecurityAction          |
		| Token   | Select                  |
		| Token   | Go                      |
		| Token   | UserVerification        |
		| Token   | UserVerificationSuccess |
	When I try get the translate
	Then I will receive no multilanguage error


Scenario: 10. Get translate of Interface (user)
	Given I have these keys
		| Section | Phrase                 |
		| User    | ForgotPassword         |
		| User    | Email                  |
		| User    | Send                   |
		| User    | ForgotPassword         |
		| User    | FollowEmailIntructions |
		| User    | Welcome                |
		| User    | Index_Introdution      |
		| User    | Index_PossibleNowADays |
		| User    | Index_Moves            |
		| User    | Index_Transfer         |
		| User    | Index_Categories       |
		| User    | Index_Reports          |
		| User    | Index_ReportMonth      |
		| User    | Index_ReportYear       |
		| User    | Index_Close            |
		| User    | Index_Finish           |
		| User    | Logon                  |
		| User    | Enter                  |
		| User    | Email                  |
		| User    | Password               |
		| User    | RememberMe             |
		| User    | ForgotPassword         |
		| User    | TokenReceivedByEmail   |
		| User    | LogonDisabled          |
		| User    | FollowEmailIntructions |
		| User    | SignUp                 |
		| User    | Email                  |
		| User    | CharactersMax          |
		| User    | Password               |
		| User    | RetypePassword         |
		| User    | Create                 |
		| User    | SignUpSuccess          |
		| User    | FollowEmailIntructions |
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
		| Section | Phrase               |
		| General | LogOff               |
		| General | MonthMoves           |
		| General | YearMoves            |
		| General | Create               |
		| General | Schedule             |
		| General | Move                 |
		| General | Categories           |
		| General | Accounts             |
		| General | WeirdMenuExplanation |
	When I try get the translate
	Then I will receive no multilanguage error