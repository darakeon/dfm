Feature: Robot that execute periodic tasks

@SaveOrUpdateSchedule
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

Scenario: 020. Save without Schedule (E)
Scenario: 021. Save with Schedule Times zero and bounded (E)

Scenario: 096. Save with info all right (value) (S)
Scenario: 097. Save with info all right (details) (S)
Scenario: 098. Save negative (value) (S)
Scenario: 099. Save negative (details) (S)

@RunSchedule
Scenario: 101. Run with invalid user (E)
Scenario: 199. Run with info all right (S)
