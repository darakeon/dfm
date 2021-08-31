Feature: A. Transform into url

Scenario: A01. Remove punctuation
	Given the text ah:sgd.ka?sdh
	When remove special characters
	Then the result will be ah_sgd_ka_sdh

Scenario: A02. Remove spaces
	Given the text ah sgd ka sdh
	When remove special characters
	Then the result will be ah_sgd_ka_sdh

Scenario: A03. Replace diacritics
	Given the text pavê
	When remove special characters
	Then the result will be pave

Scenario: A04. Replace uppercase
	Given the text Lica
	When remove special characters
	Then the result will be lica
