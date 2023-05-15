Feature: De. Change theme

Background:
	Given test user login

Scenario: De01. Change system Theme to Dark Magic
	Given a theme DarkMagic
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be DarkMagic

Scenario: De02. Change system Theme to Light Magic
	Given a theme LightMagic
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be LightMagic

Scenario: De03. Change system Theme to Dark Sober
	Given a theme DarkSober
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be DarkSober

Scenario: De04. Change system Theme to Light Sober
	Given a theme LightSober
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be LightSober

Scenario: De05. Change system Theme to Dark Nature
	Given a theme DarkNature
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be DarkNature

Scenario: De06. Change system Theme to Light Nature
	Given a theme LightNature
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be LightNature

Scenario: De07. Change system Theme to None
	Given a theme None
	When I try to change the Theme
	Then I will receive this core error: InvalidTheme

Scenario: De08. Not change system Theme if user is marked for deletion
	Given a theme DarkMagic
		But the user is marked for deletion
	When I try to change the Theme
	Then I will receive this core error: UserDeleted

Scenario: De09. Not change system Theme if user requested wipe
	Given a theme DarkMagic
		But the user asked data wipe
	When I try to change the Theme
	Then I will receive this core error: UserAskedWipe

Scenario: De10. Not change system Theme if not signed last contract
	Given a theme DarkMagic
		But there is a new contract
	When I try to change the Theme
	Then I will receive this core error: NotSignedLastContract
