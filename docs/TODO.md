<img src="../site/MVC/Assets/images/pig-on.svg" height="85" align="right"/>

# TO-DO LIST

This is the task list for the project.

- Type: how does it changes the version number (check [RELEASES](RELEASES.md))
- Dif(ficulty): how hard is to do the job \[1-4\]
- Imp(ortance): how positive is the impact caused on system \[1-4\]
- Pts (points): calculation based on Difficulty and Importance, to decide priority (4-D+I) \[1-7\]
- Dependency: which other task it depends on (if it exists)
- Issue: github issue about the task (if it exists)

The tasks are ordered by: Points then Importance then [Type](RELEASES.md#legend).

| Task (49)                                                                      | Type     | Dif | Imp | Pts |
| ------------------------------------------------------------------------------ | -------- | --- | --- | --- |
| Add category creation (mobile)                                                 | :dragon: |  2  |  4  |  6  |
| Add account creation (mobile)                                                  | :dragon: |  2  |  4  |  6  |
| Add schedule creation (mobile)                                                 | :dragon: |  2  |  4  |  6  |
| Add semgrep to dfm, dk-lib, meak and server                                    | :whale:  |  2  |  4  |  6  |
| [clear headers on nginx](todo/clear-headers.png)                               | :whale:  |  2  |  4  |  6  |
| Check [nginx example configuration](todo/nginx-example.conf)                   | :sheep:  |  2  |  4  |  6  |
| Expire tickets with more than 30 days without access (add to contract)         | :sheep:  |  2  |  4  |  6  |
| Implement [password rules]                                                     | :sheep:  |  2  |  4  |  6  |
| Create measure of recovering after lost authy                                  | :sheep:  |  2  |  4  |  6  |
| Look at empty screen buttons to check if they are sticked                      | :ant:    |  2  |  4  |  6  |
| Check [android error socket](todo/android-error-socket-closed.log)             | :ant:    |  2  |  4  |  6  |
| Check [android error no internet](todo/android-error-no-internet.log)          | :ant:    |  2  |  4  |  6  |
| Inject ServiceAccess with AddScoped at MVC (because of Session NH)             | :ant:    |  2  |  4  |  6  |
| Add rate limit to API and site                                                 | :whale:  |  1  |  3  |  6  |
| Add [swagger] to API                                                           | :whale:  |  1  |  3  |  6  |
| warning about 15 minutes windows for runnings schedules                        | :sheep:  |  1  |  3  |  6  |
| Refactor Core Steps to improve their division to entities and given/when/then  | :sheep:  |  1  |  3  |  6  |
| Fix print contract                                                             | :ant:    |  1  |  3  |  6  |
| Replace XML at UI android by [compose jetpack]                                 | :dragon: |  3  |  4  |  5  |
| Implement security measures suggested by github                                | :sheep:  |  3  |  4  |  5  |
| add snyk/clair check to docker containers                                      | :whale:  |  2  |  3  |  5  |
| Add plain text to emails                                                       | :whale:  |  2  |  3  |  5  |
| Enable Copy Move (web+mobile)                                                  | :whale:  |  2  |  3  |  5  |
| Add weekly scheduling                                                          | :sheep:  |  2  |  3  |  5  |
| [Lint C#]                                                                      | :ant:    |  2  |  3  |  5  |
| Make default language EN if browser is not PT, ES, IT or FR, otherwise PT      | :sheep:  |  1  |  2  |  5  |
| Remove unused/duplicated errors                                                | :sheep:  |  1  |  2  |  5  |
| Add test for unifying categories used in closed accounts                       | :ant:    |  1  |  2  |  5  |
| Add user last access reports to Admin (hide username)                          | :sheep:  |  1  |  2  |  5  |
| Add accessibility to automated tests                                           | :whale:  |  4  |  4  |  4  |
| Test android tests payloads x .net api objects                                 | :sheep:  |  4  |  4  |  4  |
| Report with more than one account                                              | :dragon: |  3  |  3  |  4  |
| Add schedule anticipation                                                      | :whale:  |  3  |  3  |  4  |
| Handle split screen layout at android app                                      | :whale:  |  3  |  3  |  4  |
| Data import and export (csv, json) (with max request length) [template]        | :dragon: |  2  |  2  |  4  |
| Create service to translate site ("put in your language")                      | :dragon: |  2  |  2  |  4  |
| Add nickname field - show it at every screen and e-mail                        | :whale:  |  2  |  2  |  4  |
| If session drop, call history.go(-2) after re-login                            | :sheep:  |  1  |  1  |  4  |
| Add category url calculate without diacritics                                  | :sheep:  |  1  |  1  |  4  |
| Add error for schedule with 0 times                                            | :ant:    |  1  |  1  |  4  |
| Change putJson to putSerializable/Parcelable at extras in android              | :ant:    |  1  |  1  |  4  |
| Add logs to Admin                                                              | :sheep:  |  1  |  1  |  4  |
| Implement entities limits (acc opened, cat enabled, moves/month, active sched) | :dragon: |  3  |  2  |  3  |
| Transform Error Logs notification into widget                                  | :ant:    |  2  |  1  |  3  |
| Replace hardcoded scenario codes from tests                                    | :ant:    |  2  |  1  |  3  |
| Remove GetOrCreate                                                             | :ant:    |  2  |  1  |  3  |
| Separate validators from service/repository                                    | :sheep:  |  4  |  2  |  2  |
| [OCR] to add values to system                                                  | :dragon: |  4  |  2  |  2  |
| Add [gherkin jest]                                                             | :sheep:  |  4  |  1  |  1  |

[compose jetpack]: https://medium.com/@nglauber/jetpack-compose-o-framework-de-ui-do-android-para-os-pr%C3%B3ximos-10-anos-e19adf28e57e
[password rules]: https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html#implement-proper-password-strength-controls
[gherkin jest]: https://www.npmjs.com/package/gherkin-jest
[Lint C#]: https://medium.com/@michaelparkerdev/linting-c-in-2019-stylecop-sonar-resharper-and-roslyn-73e88af57ebd
[OCR]: https://developers.google.com/ml-kit/vision/text-recognition/android
[template]: dirigir-1tI0z29LBJJAQCYq1fptWCN8jgL6b2yj-
[swagger]: https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-8.0
