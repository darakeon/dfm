Feature: Route
	Background: 
		Given I have these patterns
			| Name       | Pattern                                                         |
			| Account    | Account/{accountUrl}/{controller=Reports}/{action=Index}/{id?}  |
			| Mobile     | @{activity}                                                     |
			| Default    | {controller=Users}/{action=Index}/{id?}                         |

Scenario: 01. Routes in use at MVC
	When I ask the route for these values
			| Name    | Controller | Action  | ID  |
			| Default | Ops        | Code    | 500 |
			| Default | Tokens     | Disable |     |
	Then my routes would be
			| Route           |
			| /Ops/Code/500   |
			| /Tokens/Disable |
