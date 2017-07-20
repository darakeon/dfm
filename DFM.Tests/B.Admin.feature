Feature: Admininistration of Accounts and Categories

@SaveOrUpdateAccount
Scenario: 001. Save Account without name (E)
Scenario: 002. Save Account with just yellow limit (E)
Scenario: 003. Save Account with just red limit (E)
Scenario: 004. Save Account with red limit bigger than yellow limit (E)
Scenario: 005. Save Account with red limit equal to yellow limit (E)
Scenario: 006. Save Account with name that already exists (E)
Scenario: 099. Save Account with info all right (S)

@SelectAccountById
Scenario: 101. Try to get Account with wrong ID (E)
Scenario: 199. Get the Account by ID (S)

@CloseAccount
Scenario: 200. Close an Account already closed (E)
Scenario: 201. Close an Account that doesn't exist (E)
Scenario: 299. Close an Account with info all right (S)

@DeleteAccount
Scenario: 300. Delete an Account already deleted (E)
Scenario: 301. Delete an Account that doesn't exist (E)
Scenario: 399. Delete an Account with info all right (S)


@SaveOrUpdateCategory
Scenario: 401. Save Category without name (E)
Scenario: 402. Save Category with name that already exists (E)
Scenario: 499. Save Category with info all right (S)

@SelectCategoryById
Scenario: 501. Try to get Category with wrong ID (E)
Scenario: 599. Get the Category by ID (S)

@DisableCategory
Scenario: 600. Disable an Category already disabled (E)
Scenario: 601. Disable an Category that doesn't exist (E)
Scenario: 699. Disable an Category with info all right (S)

@EnableCategory
Scenario: 700. Enable an Category already enabled (E)
Scenario: 701. Enable an Category that doesn't exist (E)
Scenario: 799. Enable an Category with info all right (S)