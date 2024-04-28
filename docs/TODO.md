<img src="../site/MVC/Assets/images/pig-on.svg" height="85" align="right"/>

# TO-DO LIST

This is the task list for the project.

- Typ(e): how does it changes the version number (check [RELEASES](RELEASES.md))
- Dif(ficulty): how hard is to do the job \[1-4\]
- Imp(ortance): how positive is the impact caused on system \[1-4\]
- Pts (points): calculation based on Difficulty and Importance, to decide priority (4-D+I) \[1-7\]
- Dependency: which other task it depends on (if it exists)
- Issue: github issue about the task (if it exists)

The tasks are ordered by: Points then Importance then [Type](RELEASES.md#legend).

| Task (46)                                                                      | Typ | Dif | Imp | Pts |
| ------------------------------------------------------------------------------ | --- | --- | --- | --- |
| Implement entities limits (acc opened, cat enabled, moves/month, active sched) | 🐉 |  2  |  4  |  6  |
| Add semgrep to dfm, dk-lib, meak and server                                    | 🐳 |  2  |  4  |  6  |
| [clear headers on nginx](todo/clear-headers.png)                               | 🐳 |  2  |  4  |  6  |
| Check [nginx example configuration](todo/nginx-example.conf)                   | 🐑 |  2  |  4  |  6  |
| Expire tickets with more than 30 days without access (add to contract)         | 🐑 |  2  |  4  |  6  |
| Implement [password rules]                                                     | 🐑 |  2  |  4  |  6  |
| Create measure of recovering after lost authy                                  | 🐑 |  2  |  4  |  6  |
| Check [android error socket](todo/android-error-socket-closed.log)             | 🐜 |  2  |  4  |  6  |
| Check [android error no internet](todo/android-error-no-internet.log)          | 🐜 |  2  |  4  |  6  |
| Inject ServiceAccess with AddScoped at MVC (because of Session NH)             | 🐜 |  2  |  4  |  6  |
| Add rate limit to API and site                                                 | 🐳 |  1  |  3  |  6  |
| Add [swagger] to API                                                           | 🐳 |  1  |  3  |  6  |
| Refactor Core Steps to improve their division to entities and given/when/then  | 🐑 |  1  |  3  |  6  |
| Replace XML at UI android by [compose jetpack]                                 | 🐉 |  3  |  4  |  5  |
| Implement security measures suggested by github                                | 🐑 |  3  |  4  |  5  |
| Add schedule creation (mobile)                                                 | 🐉 |  2  |  3  |  5  |
| add snyk/clair check to docker containers                                      | 🐳 |  2  |  3  |  5  |
| Add plain text to emails                                                       | 🐳 |  2  |  3  |  5  |
| Add weekly scheduling                                                          | 🐑 |  2  |  3  |  5  |
| [Lint C#]                                                                      | 🐜 |  2  |  3  |  5  |
| Make default language EN if browser is not PT, ES, IT or FR, otherwise PT      | 🐑 |  1  |  2  |  5  |
| Remove unused/duplicated errors                                                | 🐑 |  1  |  2  |  5  |
| Add user last access reports to Admin (hide username)                          | 🐑 |  1  |  2  |  5  |
| Look at empty screen buttons to check if they are sticked                      | 🐜 |  1  |  2  |  5  |
| Add test for unifying categories used in closed accounts                       | 🐜 |  1  |  2  |  5  |
| Add accessibility to automated tests                                           | 🐳 |  4  |  4  |  4  |
| Test android tests payloads x .net api objects                                 | 🐑 |  4  |  4  |  4  |
| Report with more than one account                                              | 🐉 |  3  |  3  |  4  |
| Add schedule anticipation                                                      | 🐳 |  3  |  3  |  4  |
| Handle split screen layout at android app                                      | 🐳 |  3  |  3  |  4  |
| Create service to translate site ("put in your language")                      | 🐉 |  2  |  2  |  4  |
| Add category creation (mobile)                                                 | 🐉 |  2  |  2  |  4  |
| Add account creation (mobile)                                                  | 🐉 |  2  |  2  |  4  |
| Enable Copy Move (web+mobile)                                                  | 🐳 |  2  |  2  |  4  |
| Add nickname field - show it at every screen and e-mail                        | 🐳 |  2  |  2  |  4  |
| If session drop, call history.go(-2) after re-login                            | 🐑 |  1  |  1  |  4  |
| Add category url calculate without diacritics                                  | 🐑 |  1  |  1  |  4  |
| Add error for schedule with 0 times                                            | 🐜 |  1  |  1  |  4  |
| Change putJson to putSerializable/Parcelable at extras in android              | 🐜 |  1  |  1  |  4  |
| Add logs to Admin                                                              | 🐑 |  1  |  2  |  3  |
| Transform Error Logs notification into widget                                  | 🐜 |  2  |  1  |  3  |
| Replace hardcoded scenario codes from tests                                    | 🐜 |  2  |  1  |  3  |
| Remove GetOrCreate                                                             | 🐜 |  2  |  1  |  3  |
| Separate validators from service/repository                                    | 🐑 |  4  |  2  |  2  |
| [OCR] to add values to system                                                  | 🐉 |  4  |  2  |  2  |
| Add [gherkin jest]                                                             | 🐑 |  4  |  1  |  1  |

[compose jetpack]: https://medium.com/@nglauber/jetpack-compose-o-framework-de-ui-do-android-para-os-pr%C3%B3ximos-10-anos-e19adf28e57e
[password rules]: https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html#implement-proper-password-strength-controls
[gherkin jest]: https://www.npmjs.com/package/gherkin-jest
[Lint C#]: https://medium.com/@michaelparkerdev/linting-c-in-2019-stylecop-sonar-resharper-and-roslyn-73e88af57ebd
[OCR]: https://developers.google.com/ml-kit/vision/text-recognition/android
[template]: dirigir-1tI0z29LBJJAQCYq1fptWCN8jgL6b2yj-
[swagger]: https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-8.0
