Feature: A. Move to CSV object

Scenario: A01. Transform move
	Given this move data
			| Guid | Description | Date       | CategoryName | Nature   | InName | OutName | Value | Conversion |
			| 1    | Move 1      | 2021-05-11 | Cat          | Transfer | In     | Out     | 10    |            |
			| 2    | Move 2      | 2021-05-10 | Cat          | Out      |        | Out     | 20    |            |
			| 3    | Move 3      | 2021-05-09 | Cat          | In       | In     |         | 30    |            |
			| 4    | Move 4      | 2021-05-08 |              | Transfer | In     | Out     | 40    |            |
			| 5    | Move 5      | 2021-05-07 |              | Transfer | In     | Out     | 40    | 200        |
	When convert to csv
	Then the file will have these lines
			| File                                                             |
			| Description,Date,Category,Nature,In,Out,Value,Conversion,Details |
			| Move 5,2021-05-07,,Transfer,In,Out,40.00,200.00,                 |
			| Move 4,2021-05-08,,Transfer,In,Out,40.00,,                       |
			| Move 3,2021-05-09,Cat,In,In,,30.00,,                             |
			| Move 2,2021-05-10,Cat,Out,,Out,20.00,,                           |
			| Move 1,2021-05-11,Cat,Transfer,In,Out,10.00,,                    |

Scenario: A02. Transform move with detail
	Given this move data
			| Guid | Description | Date       | CategoryName | Nature   | InName | OutName |
			| 5    | Move 5      | 2021-05-11 | Cat          | Transfer | In     | Out     |
			| 6    | Move 6      | 2021-05-10 | Cat          | Out      |        | Out     |
			| 7    | Move 7      | 2021-05-09 | Cat          | In       | In     |         |
			| 8    | Move 8      | 2021-05-08 | Cat          | Transfer | In     | Out     |
		And this detail data
			| Parent | Description | Amount | Value | Conversion |
			| Move 5 | Detail 4    | 4      | 0.40  |            |
			| Move 6 | Detail 5    | 5      | 0.50  |            |
			| Move 6 | Detail 6    | 6      | 0.60  |            |
			| Move 7 | Detail 7    | 7      | 0.70  |            |
			| Move 7 | Detail 8    | 8      | 0.80  |            |
			| Move 7 | Detail 9    | 9      | 0.90  |            |
			| Move 8 | Detail 10   | 10     | 1.00  | 5.00       |
	When convert to csv
	Then the file will have these lines
			| File                                                                                     |
			| Description,Date,Category,Nature,In,Out,Value,Conversion,Details                         |
			| Move 8,2021-05-08,Cat,Transfer,In,Out,,,Detail 10 [10x1.00(5.00)]                        |
			| Move 7,2021-05-09,Cat,In,In,,,,Detail 7 [7x0.70] + Detail 8 [8x0.80] + Detail 9 [9x0.90] |
			| Move 6,2021-05-10,Cat,Out,,Out,,,Detail 5 [5x0.50] + Detail 6 [6x0.60]                   |
			| Move 5,2021-05-11,Cat,Transfer,In,Out,,,Detail 4 [4x0.40]                                |
