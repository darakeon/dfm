Feature: Bn. Change theme

Background:
	Given I have a complete user logged in

Scenario: Bn01. Change system Theme to Dark Magic
	Given a theme DarkMagic
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be DarkMagic

Scenario: Bn02. Change system Theme to Light Magic
	Given a theme LightMagic
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be LightMagic

Scenario: Bn03. Change system Theme to Dark Sober
	Given a theme DarkSober
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be DarkSober

Scenario: Bn04. Change system Theme to Light Sober
	Given a theme LightSober
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be LightSober

Scenario: Bn05. Change system Theme to Dark Nature
	Given a theme DarkNature
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be DarkNature

Scenario: Bn06. Change system Theme to Light Nature
	Given a theme LightNature
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be LightNature

Scenario: Bn07. Change system Theme to None
	Given a theme None
	When I try to change the Theme
	Then I will receive this core error: InvalidTheme
