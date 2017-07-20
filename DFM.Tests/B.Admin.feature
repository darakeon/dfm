Feature: Admininistration of Accounts and Categories

--SaveOrUpdateAccount
Scenario: 001. Save account without name (E)
Scenario: 002. Save account with just yellow limit (E)
Scenario: 003. Save account with just red limit (E)
Scenario: 004. Save account with red limit bigger than yellow limit (E)
Scenario: 005. Save account with red limit equal to yellow limit (E)
Scenario: 006. Save account with name that already exists (E)
Scenario: 007. Save account with info all right (S)

--SelectAccountById
Scenario: 101. Try to get account with wrong ID (E)
Scenario: 102. Get the account by ID (S)

--CloseAccount
Scenario: 200. Close an account already closed (E)
Scenario: 201. Close an account that doesn't exist (E)
Scenario: 201. Close an account with info all right (S)

--DeleteAccount
Scenario: 300. Delete an account already deleted (E)
Scenario: 301. Delete an account that doesn't exist (E)
Scenario: 301. Delete an account with info all right (S)


--SaveOrUpdateCategory
Scenario: 401. Save category without name (E)
Scenario: 402. Save category with name that already exists (E)
Scenario: 403. Save category with info all right (S)

--SelectCategoryById
Scenario: 501. Try to get category with wrong ID (E)
Scenario: 502. Get the category by ID (S)

--DisableCategory
Scenario: 600. Disable an category already disabled (E)
Scenario: 601. Disable an category that doesn't exist (E)
Scenario: 601. Disable an category with info all right (S)

--EnableCategory
Scenario: 700. Enable an category already enabled (E)
Scenario: 701. Enable an category that doesn't exist (E)
Scenario: 701. Enable an category with info all right (S)