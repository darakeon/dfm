<img src="../site/MVC/Assets/images/pig-on.svg" height="85" align="right"/>

# TO-DO LIST

This is the task list for the project.

- Typ(e): Security, Bug, Feature or Maintenance
- Siz(e): how does it changes the version number (check [RELEASES](RELEASES.md))
- Dif(ficulty): how hard is to do the job \[1-4\]
- Imp(ortance): how positive is the impact caused on system \[1-4\]
- Pts (points): calculation based on Difficulty and Importance, to decide priority (4-D+I) \[1-7\]
- Dependency: which other task it depends on (if it exists)
- Issue: github issue about the task (if it exists)

The tasks are ordered by: Points then Importance then [Size](RELEASES.md#legend) then Type.

| Task (48)                                                                      | Typ | Siz | Dif | Imp | Pts |
| ------------------------------------------------------------------------------ | --- | --- | --- | --- | --- |
| Implement entities limits (acc opened, cat enabled, moves/month, active sched) |  F  | 🐉 |  2  |  4  |  6  |
| Add semgrep to dfm, dk-lib, meak and server                                    |  S  | 🐳 |  2  |  4  |  6  |
| [clear headers on nginx](todo/clear-headers.png)                               |  S  | 🐳 |  2  |  4  |  6  |
| Check [nginx example configuration](todo/nginx-example.conf)                   |  S  | 🐑 |  2  |  4  |  6  |
| Expire tickets with more than 30 days without access (add to contract)         |  F  | 🐑 |  2  |  4  |  6  |
| Implement [password rules]                                                     |  F  | 🐑 |  2  |  4  |  6  |
| Create measure of recovering after lost authy                                  |  F  | 🐑 |  2  |  4  |  6  |
| Move all logs to CloudWatch                                                    |  S  | 🐜 |  2  |  4  |  6  |
| Check [android error socket](todo/android-error-socket-closed.log)             |  B  | 🐜 |  2  |  4  |  6  |
| Check [android error no internet](todo/android-error-no-internet.log)          |  B  | 🐜 |  2  |  4  |  6  |
| Inject ServiceAccess with AddScoped at MVC (because of Session NH)             |  M  | 🐜 |  2  |  4  |  6  |
| Add rate limit to API and site                                                 |  S  | 🐳 |  1  |  3  |  6  |
| Add [swagger] to API                                                           |  F  | 🐳 |  1  |  3  |  6  |
| Refactor Core Steps to improve their division to entities and given/when/then  |  M  | 🐑 |  1  |  3  |  6  |
| Replace XML at UI android by [compose jetpack]                                 |  M  | 🐉 |  3  |  4  |  5  |
| Implement security measures suggested by github                                |  S  | 🐑 |  3  |  4  |  5  |
| Add schedule creation (mobile)                                                 |  F  | 🐉 |  2  |  3  |  5  |
| add snyk/clair check to docker containers                                      |  S  | 🐳 |  2  |  3  |  5  |
| Add plain text to emails                                                       |  F  | 🐳 |  2  |  3  |  5  |
| Add weekly scheduling                                                          |  F  | 🐑 |  2  |  3  |  5  |
| Complete all screens in E2E, with all its cases (empty/filled, normal,error)   |  M  | 🐑 |  2  |  3  |  5  |
| [Lint C#]                                                                      |  M  | 🐜 |  2  |  3  |  5  |
| Make default language EN if browser is not PT, ES, IT or FR, otherwise PT      |  F  | 🐑 |  1  |  2  |  5  |
| Remove unused/duplicated errors                                                |  M  | 🐑 |  1  |  2  |  5  |
| Add user last access reports to Admin (hide username)                          |  M  | 🐑 |  1  |  2  |  5  |
| Look at empty screen buttons to check if they are sticked                      |  M  | 🐜 |  1  |  2  |  5  |
| Add test for unifying categories used in closed accounts                       |  M  | 🐜 |  1  |  2  |  5  |
| Add accessibility to automated tests                                           |  F  | 🐳 |  4  |  4  |  4  |
| Test android tests payloads x .net api objects                                 |  M  | 🐑 |  4  |  4  |  4  |
| Report with more than one account                                              |  F  | 🐉 |  3  |  3  |  4  |
| Add schedule anticipation                                                      |  F  | 🐳 |  3  |  3  |  4  |
| Handle split screen layout at android app                                      |  F  | 🐳 |  3  |  3  |  4  |
| Create service to translate site ("put in your language")                      |  F  | 🐉 |  2  |  2  |  4  |
| Add category creation (mobile)                                                 |  F  | 🐉 |  2  |  2  |  4  |
| Add account creation (mobile)                                                  |  F  | 🐉 |  2  |  2  |  4  |
| Enable Copy Move (web+mobile)                                                  |  F  | 🐳 |  2  |  2  |  4  |
| Add nickname field - show it at every screen and e-mail                        |  F  | 🐳 |  2  |  2  |  4  |
| If session drop, call history.go(-2) after re-login                            |  F  | 🐑 |  1  |  1  |  4  |
| Add category url calculate without diacritics                                  |  F  | 🐑 |  1  |  1  |  4  |
| Add error for schedule with 0 times                                            |  F  | 🐜 |  1  |  1  |  4  |
| Change putJson to putSerializable/Parcelable at extras in android              |  M  | 🐜 |  1  |  1  |  4  |
| Add logs to Admin                                                              |  M  | 🐑 |  1  |  2  |  3  |
| Transform Error Logs notification into widget                                  |  M  | 🐜 |  2  |  1  |  3  |
| Replace hardcoded scenario codes from tests                                    |  M  | 🐜 |  2  |  1  |  3  |
| Remove GetOrCreate                                                             |  M  | 🐜 |  2  |  1  |  3  |
| Separate validators from service/repository                                    |  M  | 🐑 |  4  |  2  |  2  |
| [OCR] to add values to system                                                  |  F  | 🐉 |  4  |  2  |  2  |
| Add [gherkin jest]                                                             |  M  | 🐑 |  4  |  1  |  1  |

[compose jetpack]: https://medium.com/@nglauber/jetpack-compose-o-framework-de-ui-do-android-para-os-pr%C3%B3ximos-10-anos-e19adf28e57e
[password rules]: https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html#implement-proper-password-strength-controls
[gherkin jest]: https://www.npmjs.com/package/gherkin-jest
[Lint C#]: https://medium.com/@michaelparkerdev/linting-c-in-2019-stylecop-sonar-resharper-and-roslyn-73e88af57ebd
[OCR]: https://developers.google.com/ml-kit/vision/text-recognition/android
[swagger]: https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-8.0
