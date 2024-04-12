Feature: B. Schedule to CSV object

Scenario: B01. Already runned create no file
	Given this schedule data
			| Description | Date       | CategoryName | Nature   | InName | OutName | Value | Times | Frequency | Boundless | LastRun |
			| Schedule 1  | 1986-03-27 | Cat          | Transfer | In     | Out     | 10    | 7     | Monthly   | False     | 10      |
	When convert to csv
	Then there will be no file generation

Scenario: B02. Partially runned add only future part
	Given this schedule data
			| Description | Date       | CategoryName | Nature   | InName | OutName | Value | Times | Frequency | Boundless | LastRun |
			| Schedule 2  | 2021-05-13 | Cat          | Transfer | In     | Out     | 10    | 7     | Daily     | False     | 4       |
	When convert to csv
	Then the file will have these lines
			| File                                                             |
			| Description,Date,Category,Nature,In,Out,Value,Conversion,Details |
			| Schedule 2,2021-05-17,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 2,2021-05-18,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 2,2021-05-19,Cat,Transfer,In,Out,10.00,,                |

Scenario: B03. Not runned add all
	Given this schedule data
			| Description | Date       | CategoryName | Nature   | InName | OutName | Value | Times | Frequency | Boundless | LastRun |
			| Schedule 3  | 2021-05-13 | Cat          | Transfer | In     | Out     | 10    | 7     | Daily     | False     | 0       |
	When convert to csv
	Then the file will have these lines
			| File                                                             |
			| Description,Date,Category,Nature,In,Out,Value,Conversion,Details |
			| Schedule 3,2021-05-13,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 3,2021-05-14,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 3,2021-05-15,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 3,2021-05-16,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 3,2021-05-17,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 3,2021-05-18,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 3,2021-05-19,Cat,Transfer,In,Out,10.00,,                |

Scenario: B04. With detail
	Given this schedule data
			| Description | Date       | CategoryName | Nature   | InName | OutName | Value | Times | Frequency | Boundless |
			| Schedule 4  | 2021-05-13 | Cat          | Transfer | In     | Out     |       | 1     | Daily     | False     |
		And this detail data
			| Parent     | Description | Amount | Value |
			| Schedule 4 | Detail 10   | 10     | 1.00  |
	When convert to csv
	Then the file will have these lines
			| File                                                             |
			| Description,Date,Category,Nature,In,Out,Value,Conversion,Details |
			| Schedule 4,2021-05-13,Cat,Transfer,In,Out,,,Detail 10 [10x1.00]  |

Scenario: B05. Bounded and boundless add boundless until bounded finishes
	Given this schedule data
			| Description | Date       | CategoryName | Nature   | InName | OutName | Value | Times | Frequency | Boundless |
			| Schedule 5  | 2021-05-13 | Cat          | Transfer | In     | Out     | 10    | 7     | Daily     | False     |
			| Schedule 6  | 2021-05-14 | Cat          | Transfer | In     | Out     | 10    |       | Daily     | True      |
	When convert to csv
	Then the file will have these lines
			| File                                                             |
			| Description,Date,Category,Nature,In,Out,Value,Conversion,Details |
			| Schedule 5,2021-05-13,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 5,2021-05-14,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 6,2021-05-14,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 5,2021-05-15,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 6,2021-05-15,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 5,2021-05-16,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 6,2021-05-16,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 5,2021-05-17,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 6,2021-05-17,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 5,2021-05-18,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 6,2021-05-18,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 5,2021-05-19,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 6,2021-05-19,Cat,Transfer,In,Out,10.00,,                |

Scenario: B06. Just boundless don't add moves, because scheduler have runned it until now
	Given this schedule data
			| Description | Date       | CategoryName | Nature   | InName | OutName | Value | Times | Frequency | Boundless |
			| Schedule 7  | 2021-05-13 | Cat          | Transfer | In     | Out     | 10    |       | Daily     | True      |
	When convert to csv
	Then there will be no file generation

Scenario: B07. Duplicated schedule
	Given this schedule data
			| Guid | Description | Date       | CategoryName | Nature   | InName | OutName | Value | Times | Frequency | Boundless |
			| 8    | Schedule 8  | 2021-05-13 | Cat          | Transfer | In     | Out     | 10    | 7     | Daily     | False     |
			| 8    | Schedule 8  | 2021-05-13 | Cat          | Transfer | In     | Out     | 10    | 7     | Daily     | False     |
	When convert to csv
	Then the file will have these lines
			| File                                                             |
			| Description,Date,Category,Nature,In,Out,Value,Conversion,Details |
			| Schedule 8,2021-05-13,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 8,2021-05-14,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 8,2021-05-15,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 8,2021-05-16,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 8,2021-05-17,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 8,2021-05-18,Cat,Transfer,In,Out,10.00,,                |
			| Schedule 8,2021-05-19,Cat,Transfer,In,Out,10.00,,                |
