Feature: Admininistration of Accounts and Categories

--SelectAccountById
Scenario: 001. Try to get account with wrong ID (E)
Scenario: 002. Get the account by ID (S)

--SaveOrUpdateAccount
Scenario: 101. Save account without name (E)
Scenario: 102. Save account with just yellow limit (E)
Scenario: 103. Save account with just red limit (E)
Scenario: 104. Save account with red limit bigger than yellow limit (E)
Scenario: 105. Save account with red limit equal to yellow limit (E)
Scenario: 106. Save account with name that already exists (E)
Scenario: 107. Save account with info all right (S)

--CloseAccount
Scenario: 200. Close an account already closed (E)
Scenario: 201. Close an account that doesn't exist (E)
Scenario: 201. Close an account with info all right (S)

--DeleteAccount
Scenario: 300. Delete an account already deleted (E)
Scenario: 301. Delete an account that doesn't exist (E)
Scenario: 301. Delete an account with info all right (S)


--SelectCategoryById
Scenario: 401. Try to get category with wrong ID (E)
Scenario: 402. Get the category by ID (S)

--SaveOrUpdateCategory
Scenario: 501. Save category without name (E)
Scenario: 502. Save category with name that already exists (E)
Scenario: 503. Save category with info all right (S)

--DisableCategory
Scenario: 600. Disable an category already disabled (E)
Scenario: 601. Disable an category that doesn't exist (E)
Scenario: 601. Disable an category with info all right (S)

--EnableCategory
Scenario: 700. Enable an category already enabled (E)
Scenario: 701. Enable an category that doesn't exist (E)
Scenario: 701. Enable an category with info all right (S)