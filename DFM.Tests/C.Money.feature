Feature: Money Movements

@SaveOrUpdateMove
Scenario: 001. Save without Description (E)
Scenario: 002. Save without Date (E)
Scenario: 003. Save with future Date (E)
Scenario: 004. Save without Category (E)
Scenario: 005. Save with not known Category (E)

Scenario: 006. Save with (Nature: Out) (AccountIn:No) (AccountOut:No) (E)
Scenario: 007. Save with (Nature: Out) (AccountIn:Yes) (AccountOut:Yes) (E)
Scenario: 008. Save with (Nature: Out) (AccountIn:Yes) (AccountOut:No) (E)
Scenario: 009. Save with (Nature: In) (AccountIn:No) (AccountOut:No) (E)
Scenario: 010. Save with (Nature: In) (AccountIn:Yes) (AccountOut:Yes) (E)
Scenario: 011. Save with (Nature: In) (AccountIn:No) (AccountOut:Yes) (E)
Scenario: 012. Save with (Nature: Transfer) (AccountIn:No) (AccountOut:No) (E)
Scenario: 013. Save with (Nature: Transfer) (AccountIn:Yes) (AccountOut:No) (E)
Scenario: 014. Save with (Nature: Transfer) (AccountIn:No) (AccountOut:Yes) (E)

Scenario: 015. Save without Value or Details (E)
Scenario: 016. Save with Value zero (E)
Scenario: 017. Save without Description in Detail (E)
Scenario: 018. Save with Amount zero in Detail (E)
Scenario: 019. Save with Value zero in Detail (E)

Scenario: 096. Save with info all right (value) (S)
Scenario: 097. Save with info all right (details) (S)
Scenario: 098. Save negative (value) (S)
Scenario: 099. Save negative (details) (S)

@SelectMoveById
Scenario: 101. Try to get Move with wrong ID (E)
Scenario: 199. Get the Move by ID (S)

@SelectDetailById
Scenario: 201. Try to get Detail with wrong ID (E)
Scenario: 299. Get the Detail by ID (S)

@DeleteMove
Scenario: 101. Try to delete Move with wrong ID (E)
Scenario: 199. Delete the Move by ID (S)
