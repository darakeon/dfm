<img src="../site/MVC/Assets/images/pig.svg" width="85" align="right"/>

# TO-DO LIST

This is the task list for the project.

- Typ(e): how does it changes the version number (check [RELEASES](RELEASES.md))
- Dif(ficulty): how hard is to do the job \[1-4\]
- Imp(ortance): how positive is the impact caused on system \[1-4\]
- Pts (points): calculation based on Difficulty and Importance, to decide priority (4-D+I) \[1-7\]
- Dependency: which other task it depends on (if it exists)
- Issue: github issue about the task (if it exists)

| Task                                                                                    | Typ | Dif | Imp | Pts | Dependency         | Issue                                            |
| --------------------------------------------------------------------------------------- | --- | --- | --- | --- | ------------------ | ------------------------------------------------ |
| Make/find app to verify DFM status                                                      |  1  |  2  |  4  |  6  |                    |                                                  |
| Make DTO use guid, not db id (delete fakeid)                                            |  1  |  2  |  4  |  6  |                    |                                                  |
| Handle CoreError at Robot                                                               |  1  |  2  |  4  |  6  |                    |                                                  |
| Adjust tab order at move screen (mobile)                                                |  2  |  2  |  4  |  6  |                    |                                                  |
| Search by move/detail                                                                   |  3  |  2  |  4  |  6  |                    |                                                  |
| Last login date                                                                         |  2  |  1  |  3  |  6  |                    |                                                  |
| Enable reopen account                                                                   |  2  |  1  |  3  |  6  |                    |                                                  |
| Add change language icon to site                                                        |  2  |  1  |  3  |  6  |                    |                                                  |
| Add navigate gestures to reports (left to future, right to past)                        |  3  |  1  |  3  |  6  |                    |                                                  |
| Add moves gestures (left delete, right check/uncheck, hold edit, click show options)    |  3  |  1  |  3  |  6  |                    |                                                  |
| Show future moves and foreseen balance                                                  |  3  |  3  |  4  |  5  |                    | [#21](https://github.com/darakeon/dfm/issues/21) |
| Check if android tests are really testing all methods                                   |  1  |  2  |  3  |  5  |                    |                                                  |
| Add code of conduct to repo                                                             |  2  |  2  |  3  |  5  |                    | [#44](https://github.com/darakeon/dfm/issues/44) |
| If lost authy, send e-mail to remove authy                                              |  2  |  2  |  3  |  5  |                    |                                                  |
| Link unsubscribe for moves (ask password if logged out?)                                |  3  |  2  |  3  |  5  |                    |                                                  |
| Add update gesture to all screens                                                       |  3  |  2  |  3  |  5  |                    |                                                  |
| Report with values: select to see sum                                                   |  2  |  1  |  2  |  5  |                    |                                                  |
| Show account state on account area                                                      |  2  |  1  |  2  |  5  |                    |                                                  |
| Add Negative / Positive / Sum to Year summary                                           |  2  |  1  |  2  |  5  |                    |                                                  |
| Add Balance Negative / Positive / Sum to Year summary                                   |  2  |  1  |  2  |  5  |                    |                                                  |
| Use Authy as password                                                                   |  2  |  1  |  2  |  5  |                    |                                                  |
| Remove Move on delete at Android (and change total)                                     |  2  |  1  |  2  |  5  |                    |                                                  |
| Change server to linux                                                                  |  1  |  4  |  4  |  4  |                    |                                                  |
| Store data locally, to use when there is no internet (Mobile)                           |  4  |  4  |  4  |  4  |                    |                                                  |
| Separate validators from service/repository                                             |  1  |  3  |  3  |  4  |                    |                                                  |
| Run robot each midnight and remove it from request (no C#)                              |  2  |  3  |  3  |  4  |                    |                                                  |
| Robot to delete not accessed users                                                      |  2  |  3  |  3  |  4  | robot / login date | [#49](https://github.com/darakeon/dfm/issues/49) |
| Add spendable value - how much one can spend by day                                     |  3  |  3  |  3  |  4  |                    | [#28](https://github.com/darakeon/dfm/issues/28) |
| Year and Month report by category [(chartjs)](http://www.chartjs.org/)                  |  3  |  3  |  3  |  4  |                    |                                                  |
| Report with date filters                                                                |  3  |  3  |  3  |  4  |                    |                                                  |
| Feature to unify Categories                                                             |  3  |  3  |  3  |  4  |                    |                                                  |
| Add system tips (based on user "age" = times accessed) (disable: unsubscribe or config) |  3  |  3  |  3  |  4  |                    |                                                  |
| Put tip that you can disable mobile login if your cellphone is stolen                   |  2  |  1  |  4  |  7  | tips               |                                                  |
| Make e-mail tests to check file generated by e-mail mock                                |  1  |  2  |  2  |  4  |                    |                                                  |
| Fix first day of account according to first move, and last day according to last move   |  1  |  2  |  2  |  4  |                    |                                                  |
| Add priority account (listed above others)                                              |  2  |  2  |  2  |  4  |                    | [#50](https://github.com/darakeon/dfm/issues/50) |
| Add priority category (listed above others)                                             |  2  |  2  |  2  |  4  |                    | [#50](https://github.com/darakeon/dfm/issues/50) |
| Add contributing rules to repo                                                          |  2  |  2  |  2  |  4  |                    | [#44](https://github.com/darakeon/dfm/issues/44) |
| Add account creation (mobile)                                                           |  3  |  2  |  2  |  4  |                    |                                                  |
| Add category creation (mobile)                                                          |  3  |  2  |  2  |  4  |                    |                                                  |
| Add user creation (mobile)                                                              |  3  |  2  |  2  |  4  |                    |                                                  |
| Add schedule creation (mobile)                                                          |  2  |  2  |  2  |  4  |                    |                                                  |
| Enable Copy Move (web+mobile)                                                           |  3  |  2  |  2  |  4  |                    |                                                  |
| Add different icon for test at android                                                  |  1  |  1  |  1  |  4  |                    |                                                  |
| If session drop, call history.go(-2) after re-login                                     |  2  |  1  |  1  |  4  |                    |                                                  |
| Change date to field with datepicker button (mobile)                                    |  2  |  1  |  1  |  4  |                    |                                                  |
| Make default language EN if browser is not PT, ES, IT or FR                             |  2  |  1  |  1  |  4  |                    |                                                  |
| Change category to autocomplete (mobile)                                                |  3  |  1  |  1  |  4  |                    |                                                  |
| Replace XML at UI android by compose jetpack                                            |  1  |  4  |  3  |  3  |                    |                                                  |
| Add misc to users                                                                       |  4  |  3  |  2  |  3  |                    |                                                  |
| Reset Misc on password change and send e-mail                                           |  2  |  1  |  3  |  6  | misc               |                                                  |
| Add Misc reset manually                                                                 |  2  |  1  |  2  |  5  | misc               |                                                  |
| Report with more than one account                                                       |  3  |  3  |  2  |  3  |                    |                                                  |
| Create Market - group of moves, each one of one category, to shopping (mobile)          |  3  |  3  |  2  |  3  |                    |                                                  |
| Implement entities limits (acc opened, cat enabled, moves/month, active sched)          |  4  |  3  |  2  |  3  |                    |                                                  |
| Separate MVC projects: site and api                                                     |  1  |  3  |  2  |  3  |                    |                                                  |
| Test android tests payloads x .net api objects                                          |  1  |  3  |  2  |  3  | break site/api     |                                                  |
| Link account and category (combo)                                                       |  2  |  2  |  1  |  3  |                    |                                                  |
| Add issue template to repo                                                              |  2  |  2  |  1  |  3  |                    | [#43](https://github.com/darakeon/dfm/issues/43) |
| Deny double use of authy code                                                           |  2  |  2  |  1  |  3  |                    |                                                  |
| Allow to choose first screen (accounts/move) (mobile)                                   |  3  |  2  |  1  |  3  |                    |                                                  |
| Add auth by cellphone                                                                   |  3  |  2  |  1  |  3  |                    |                                                  |
| OCR to add values to system                                                             |  4  |  4  |  2  |  2  |                    |                                                  |
| "Guess" the category                                                                    |  2  |  3  |  1  |  2  |                    |                                                  |
| Remove GetOrCreate                                                                      |  1  |  3  |  1  |  2  |                    |                                                  |
| Data import and export (csv, json)                                                      |  4  |  3  |  1  |  2  |                    |                                                  |
| Config max request length                                                               |  1  |  1  |  2  |  5  | import/export      |                                                  |
| Create service to translate site ("put in your language")                               |  4  |  4  |  1  |  1  |                    |                                                  |
