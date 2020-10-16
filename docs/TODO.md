<img src="../site/MVC/Assets/images/pig.svg" width="85" align="right"/>

# TO-DO LIST

This is the task list for the project.

- Type: how does it changes the version number (check [RELEASES](RELEASES.md))
- Dif(ficulty): how hard is to do the job \[1-4\]
- Imp(ortance): how positive is the impact caused on system \[1-4\]
- Pts (points): calculation based on Difficulty and Importance, to decide priority (4-D+I) \[1-7\]
- Dependency: which other task it depends on (if it exists)
- Issue: github issue about the task (if it exists)

| Task (68)                                                                               | Type     | Dif | Imp | Pts | Dependency         | Issue                                            |
| --------------------------------------------------------------------------------------- | -------- | --- | --- | --- | ------------------ | ------------------------------------------------ |
| Add create new task list at version changer (q2 add tasks 1 and 2, numbers add by line) | :ant:    |  2  |  3  |  5  |                    |                                                  |
| Check if android tests are really testing all methods                                   | :ant:    |  2  |  3  |  5  |                    |                                                  |
| Add code of conduct to repo                                                             | :sheep:  |  2  |  3  |  5  |                    | [#44](https://github.com/darakeon/dfm/issues/44) |
| If lost authy, send e-mail to remove authy                                              | :sheep:  |  2  |  3  |  5  |                    |                                                  |
| Link unsubscribe for moves (ask password if logged out?)                                | :whale:  |  2  |  3  |  5  |                    |                                                  |
| Android null body                                                                       | :ant:    |  2  |  3  |  5  |                    | [#81](https://github.com/darakeon/dfm/issues/81) |
| Timeout                                                                                 | :ant:    |  2  |  3  |  5  |                    | [#82](https://github.com/darakeon/dfm/issues/82) |
| Fix error on reports                                                                    | :ant:    |  2  |  3  |  5  |                    | [#83](https://github.com/darakeon/dfm/issues/83) |
| Problem with receiving html instead of json                                             | :ant:    |  2  |  3  |  5  |                    | [#84](https://github.com/darakeon/dfm/issues/84) |
| Report with values: select to see sum                                                   | :sheep:  |  1  |  2  |  5  |                    |                                                  |
| Show account state on account area                                                      | :sheep:  |  1  |  2  |  5  |                    |                                                  |
| Add Year Negative / Positive / Sum to Year summary                                      | :sheep:  |  1  |  2  |  5  |                    |                                                  |
| Use Authy as password                                                                   | :sheep:  |  1  |  2  |  5  |                    |                                                  |
| Remove Move on delete at Android (and change total)                                     | :sheep:  |  1  |  2  |  5  |                    |                                                  |
| Review columns of Schedule List (add deleted qty)                                       | :sheep:  |  1  |  2  |  5  |                    |                                                  |
| Calculate installment move by position                                                  | :ant:    |  1  |  2  |  5  |                    |                                                  |
| Add modal for moves with schedule, to show schedule details                             | :sheep:  |  1  |  2  |  5  |                    |                                                  |
| Change server to linux                                                                  | :ant:    |  4  |  4  |  4  |                    |                                                  |
| Store data locally, to use when there is no internet (Mobile)                           | :dragon: |  4  |  4  |  4  |                    |                                                  |
| Adjust [accessibilitity]                                                                | :sheep:  |  4  |  4  |  4  |                    |                                                  |
| Separate validators from service/repository                                             | :ant:    |  3  |  3  |  4  |                    |                                                  |
| Run robot each midnight and remove it from request (no C#)                              | :sheep:  |  3  |  3  |  4  |                    |                                                  |
| Robot to delete not accessed users                                                      | :sheep:  |  3  |  3  |  4  | robot / login date | [#49](https://github.com/darakeon/dfm/issues/49) |
| Add spendable value - how much one can spend by day                                     | :whale:  |  3  |  3  |  4  |                    | [#28](https://github.com/darakeon/dfm/issues/28) |
| Year and Month report by category [(chartjs)](http://www.chartjs.org/)                  | :whale:  |  3  |  3  |  4  |                    |                                                  |
| Report with date filters                                                                | :whale:  |  3  |  3  |  4  |                    |                                                  |
| Feature to unify Categories                                                             | :whale:  |  3  |  3  |  4  |                    |                                                  |
| Add system tips (based on user "age" = times accessed) (disable: unsubscribe or config) | :whale:  |  3  |  3  |  4  |                    |                                                  |
| Put tip that you can disable mobile login if your cellphone is stolen                   | :sheep:  |  1  |  4  |  7  | tips               |                                                  |
| Cache account/category at login + refresh button at app                                 | :whale:  |  3  |  3  |  4  |                    |                                                  |
| Make e-mail tests to check file generated by e-mail mock                                | :ant:    |  2  |  2  |  4  |                    |                                                  |
| Add priority account (listed above others)                                              | :sheep:  |  2  |  2  |  4  |                    | [#50](https://github.com/darakeon/dfm/issues/50) |
| Add priority category (listed above others)                                             | :sheep:  |  2  |  2  |  4  |                    | [#50](https://github.com/darakeon/dfm/issues/50) |
| Add contributing rules to repo                                                          | :sheep:  |  2  |  2  |  4  |                    | [#44](https://github.com/darakeon/dfm/issues/44) |
| Add account creation (mobile)                                                           | :whale:  |  2  |  2  |  4  |                    |                                                  |
| Add category creation (mobile)                                                          | :whale:  |  2  |  2  |  4  |                    |                                                  |
| Add user creation (mobile)                                                              | :whale:  |  2  |  2  |  4  |                    |                                                  |
| Add schedule creation (mobile)                                                          | :sheep:  |  2  |  2  |  4  |                    |                                                  |
| Enable Copy Move (web+mobile)                                                           | :whale:  |  2  |  2  |  4  |                    |                                                  |
| Implement [password rules]                                                              | :sheep:  |  2  |  2  |  4  |                    |                                                  |
| If session drop, call history.go(-2) after re-login                                     | :sheep:  |  1  |  1  |  4  |                    |                                                  |
| Make default language EN if browser is not PT, ES, IT or FR                             | :sheep:  |  1  |  1  |  4  |                    |                                                  |
| Add error when schedule with 0 times                                                    | :ant:    |  1  |  1  |  4  |                    |                                                  |
| Add tests to moves order at report                                                      | :ant:    |  1  |  1  |  4  |                    |                                                  |
| If try to register an already existed user, send recover password link                  | :sheep:  |  1  |  1  |  4  |                    |                                                  |
| Change putJson to putSerializable/Parcelable at extras in android                       | :ant:    |  1  |  1  |  4  |                    |                                                  |
| Replace XML at UI android by [compose jetpack]                                          | :ant:    |  4  |  3  |  3  |                    |                                                  |
| Add misc to users                                                                       | :dragon: |  3  |  2  |  3  |                    |                                                  |
| Reset Misc on password change and send e-mail                                           | :sheep:  |  1  |  3  |  6  | misc               |                                                  |
| Add Misc reset manually                                                                 | :sheep:  |  1  |  2  |  5  | misc               |                                                  |
| Report with more than one account                                                       | :whale:  |  3  |  2  |  3  |                    |                                                  |
| Create Market - group of moves, each one of one category, to shopping (mobile)          | :whale:  |  3  |  2  |  3  |                    |                                                  |
| Implement entities limits (acc opened, cat enabled, moves/month, active sched)          | :dragon: |  3  |  2  |  3  |                    |                                                  |
| Separate MVC projects: site and api                                                     | :ant:    |  3  |  2  |  3  |                    |                                                  |
| Test android tests payloads x .net api objects                                          | :ant:    |  3  |  2  |  3  | break site/api     |                                                  |
| [Lint C#]                                                                               | :ant:    |  3  |  2  |  3  |                    |                                                  |
| Link account and category (combo)                                                       | :sheep:  |  2  |  1  |  3  |                    |                                                  |
| Add issue template to repo                                                              | :sheep:  |  2  |  1  |  3  |                    | [#43](https://github.com/darakeon/dfm/issues/43) |
| Deny double use of authy code                                                           | :sheep:  |  2  |  1  |  3  |                    |                                                  |
| Add auth by cellphone                                                                   | :whale:  |  2  |  1  |  3  |                    |                                                  |
| Check entire account moves                                                              | :whale:  |  2  |  1  |  3  |                    |                                                  |
| OCR to add values to system                                                             | :dragon: |  4  |  2  |  2  |                    |                                                  |
| Add [gherkin jest]                                                                      | :ant:    |  4  |  2  |  2  |                    |                                                  |
| "Guess" the category                                                                    | :sheep:  |  3  |  1  |  2  |                    |                                                  |
| Remove GetOrCreate                                                                      | :ant:    |  3  |  1  |  2  |                    |                                                  |
| Data import and export (csv, json)                                                      | :dragon: |  3  |  1  |  2  |                    |                                                  |
| Config max request length                                                               | :ant:    |  1  |  2  |  5  | import/export      |                                                  |
| Create service to translate site ("put in your language")                               | :dragon: |  4  |  1  |  1  |                    |                                                  |

[compose jetpack]: https://medium.com/@nglauber/jetpack-compose-o-framework-de-ui-do-android-para-os-pr%C3%B3ximos-10-anos-e19adf28e57e
[password rules]: https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html#implement-proper-password-strength-controls
[accessibilitity]: https://chrome.google.com/webstore/detail/axe-coconut-web-accessibi/iobddmbdndbbbfjopjdgadphaoihpojp?hl=en
[gherkin jest]: https://www.npmjs.com/package/gherkin-jest
[Lint C#]: https://medium.com/@michaelparkerdev/linting-c-in-2019-stylecop-sonar-resharper-and-roslyn-73e88af57ebd
