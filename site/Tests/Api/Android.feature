Feature: Android Jsons
	To test if jsons android is waiting for is same site is generating

Scenario Outline: 01. Cast android jsons to .NET objects
	Given android json <json_name>
		And .NET object <net_class>
	When I deserialize json to .NET
	Then the new content will be same as default

Examples:
	| json_name     | net_class         |
	| account_list  | AccountsListModel |
	| config_get    | UserConfigModel   |
	| extract       | MovesExtractModel |
	| login         | AuthModel         |
	| move_get      | MovesCreateModel  |
	| move_get_edit | MovesCreateModel  |
	| move_get_new  | MovesCreateModel  |
	| summary       | MovesSummaryModel |
	| empty         | object            |
	| error         | object            |

Scenario Outline: 02. Cast .NET objects to android jsons
	Given android json <json_name>
		And .NET object <net_class>
	When I serialize .NET to json
	Then the new json will be same as original json

Examples:
	| json_name     | net_class         |
	| account_list  | AccountsListModel |
	| config_get    | UserConfigModel   |
	| extract       | MovesExtractModel |
	| login         | AuthModel         |
	| move_get      | MovesCreateModel  |
	| move_get_edit | MovesCreateModel  |
	| move_get_new  | MovesCreateModel  |
	| summary       | MovesSummaryModel |
	| empty         | object            |
	| error         | object            |

Scenario: 03. All jsons tested
	Given I have a list of all jsons
	When I check against this file list
	Then I verify that all jsons are included
