<img src="../site/MVC/Assets/images/pig-on.svg" height="85" align="right"/>

# RELEASES

This is the list of project releases, past and current. To see tasks that are still on hold, go to [TODO](TODO.md).

- [go to published version](#10.0.1.0)
- [go to version in development](#10.1.0.0)
- [dev version state](../../10.1.0.0/docs/RELEASES.md#10.1.0.0)

## Legend
- :dragon:: at least one huge change (increases first number)
- :whale:: at least one big change, like a new feature (increases second number)
- :sheep:: at least one little change, as a change at an existing feature (increases third number)
- :ant:: the developer is improving stuff or fixing some sh\*t it did (increases last number)

## <a name="10.1.0.0"></a>10.1.0.0 :whale: <sup>`2`</sup>
- [ ] Check accessibility of email layouts
- [ ] Fix alternate text in pig (add pink pig, which is the real description)

## <a name="10.0.1.0"></a>10.0.1.0 :sheep: <sup>`10`</sup>
- [x] `230619>230719` Add admin to nginx with production settings ([django-prod]/[django-nginx])
- [x] `230619>230619` Fix emails to remove email contact and tell how to recover and delete csv
- [x] `230618>230619` Fix terms to remove email contact and tell how to recover and delete csv
- [x] `230530>230617` Add link to purge csv into email with csv
- [x] `230409>230409` Fix acceptance check bug for calling inside another transaction
- [x] `230407>230409` Distribute tests of operations with not signed contract
- [x] `230407>230530` Add option of sending CSV to user e-mail
- [x] `230203>230207` Use hexadecimal of hash email in deleted account CSV name
- [x] `230127>230205` Hash email after wipe in the wipe history table
- [x] `230124>230127` Add django admin to search deleted accounts

## <a name="10.0.0.0"></a>10.0.0.0 :dragon: <sup>`2`</sup>
- [x] `230112>230120` Add monitoring with Grafana and Prometheus ([.NET Lib])
- [x] `230103>230103` Fix date calculation problem

## <a name="9.0.1.0"></a>9.0.1.0 :sheep: <sup>`5`</sup>
- [x] `221229>221230` Improve empty category chart (just a blank circle is confusing)
- [x] `221015>221229` Add build step and make the others depend on it, not on tests step
- [x] `221013>221013` warning mobile and browser logins stay active
- [x] `221005>221005` Add Rust security check do CI
- [x] `221001>221005` Add android security check do CI

## <a name="9.0.0.1"></a>9.0.0.1 :ant: <sup>`2`</sup>
- [x] `220928>220928` Fix android settings
- [x] `220927>220928` Fix android app icon

## <a name="9.0.0.0"></a>9.0.0.0 :dragon: <sup>`6`</sup>
- [x] `220919>220919` Remove variable stuff from terms
- [x] `220719>220918` Fix version program option `-g`
- [x] `220715>220927` Add Mono themes - Dark and Light - ACCESSIBILITY
- [x] `220715>220715` warning about schedules not running if not active
- [x] `220715>220715` warning about contract being only with who signs, not related people
- [x] `220715>220715` highlight new or changed parts of terms

## <a name="8.0.0.0"></a>8.0.0.0 :dragon: <sup>`9`</sup>
- [x] `220713>220713` Change 'Configs' to 'Settings' in the whole system
- [x] `220713>220713` Remove social media from contact modal
- [x] `220713>220713` Add footer link warning to terms
- [x] `220711>220711` Add clause about rights of image to terms
- [x] `220711>220711` Add clause about no invalidation of terms because of one clause or one exception made
- [x] `220711>220711` Hide tips while in wizard
- [x] `220709>220709` Focus to input when open search
- [x] `220704>220704` Improve categories chart accessibility for ADHD
- [x] `211228>220710` Make wizard interactive

## <a name="7.4.0.6"></a>7.4.0.6 :ant: <sup>`1`</sup>
- [x] `220623>220624` Fix robot mysql failure

## <a name="7.4.0.5"></a>7.4.0.5 :ant: <sup>`4`</sup>
- [x] `220604>220604` Add security policy to repository
- [x] `220605>220610` Fix .NET vulnerable dependencies
- [x] `220529>220604` Fix Node vulnerable dependencies
- [x] `211228>211228` Add service interruption clause

## <a name="7.4.0.4"></a>7.4.0.4 :ant: <sup>`1`</sup>
- [x] `220426>220426` Upgrade .NET version

## <a name="7.4.0.3"></a>7.4.0.3 :ant: <sup>`1`</sup>
- [x] `220326>220326` Fix wizard of misc

## <a name="7.4.0.2"></a>7.4.0.2 :ant: <sup>`1`</sup>
- [x] `220220>220220` Fix dependabot issues

## <a name="7.4.0.1"></a>7.4.0.1 :ant: <sup>`2`</sup>
- [x] `211226>211226` Add change version to CI ([how to commit ci])
- [x] `211123>211226` Fix ops page to ignore contract

## <a name="7.4.0.0"></a>7.4.0.0 :whale: <sup>`6`</sup>
- [x] `210911>210911` Hide closed accounts button if there are no closed accounts
- [x] `210901>210911` When there are accounts, show "create another account" (and category)
- [x] `210831>210831` Change "tudo bem" by "saudável" in portuguese translation
- [x] `210831>210831` Warning of "just numbers" in just numbers fields
- [x] `210829>210831` Remove account url field (calculate no diacritics - take care equal url)
- [x] `210829>210831` Add configuration for use account sign

## <a name="7.3.1.1"></a>7.3.1.1 :ant: <sup>`1`</sup>
- [x] `210902>210902` Fix robot deleting itself

## <a name="7.3.1.0"></a>7.3.1.0 :sheep: <sup>`5`</sup>
- [x] `210828>210828` Add "back to home" button at wizard end
- [x] `210828>210828` Look for "account" words at system to check ambiguity
- [x] `210828>210828` Change info / warning icon
- [x] `210828>210828` Button to create move when there are no moves
- [x] `210828>210828` Change password field name to "create your password"

## <a name="7.3.0.0"></a>7.3.0.0 :whale: <sup>`2`</sup>
- [x] `210822>210828` Put tip that you can disable mobile login if your cellphone is stolen
- [x] `210819>210823` Add system tips (user "age" = times accessed)

## <a name="7.2.0.1"></a>7.2.0.0 :ant: <sup>`1`</sup>
- [x] `210818>210818` Fix chart in dark theme

## <a name="7.2.0.0"></a>7.2.0.0 :whale: <sup>`4`</sup>
- [x] `210817>210817` Fix moving tasks from TODO to RELEASE
- [x] `210815>210717` Feature to unify Categories
- [x] `210815>210816` Hide warning about being inactive in the first three days
- [x] `210814>210815` Add edit move button to search result

## <a name="7.1.0.0"></a>7.1.0.0 :whale: <sup>`3`</sup>
- [x] `210813>210813` Fix misc at e-mails - the parts are coming misplaced
- [x] `210813>210813` Fix buttons with two icons
- [x] `210807>210813` Year and Month report by category [chart]

## <a name="7.0.0.0"></a>7.0.0.0 :dragon: <sup>`3`</sup>
- [x] `210726>210727` Fix ssl certificate
- [x] `210724>210724` Fix nature color at mobile
- [x] `210716>210728` Cache account/category at login + refresh button at app

## <a name="6.1.0.0"></a>6.1.0.0 :dragon: <sup>`3`</sup>
- [x] `210714>210715` Add commit linter
- [x] `210710>210714` Add Misc reset manually
- [x] `210710>210713` Add misc to users

## <a name="6.0.2.0"></a>6.0.2.0 :sheep: <sup>`3`</sup>
- [x] `210629>210703` Login after create account, disable if not confirm e-mail in 1 week
- [x] `210603>210628` Delete user on demand
- [x] `210603>210603` Clarify that only moves are exported in case of purge

## <a name="6.0.1.0"></a>6.0.1.0 :sheep: <sup>`3`</sup>
- [x] `210430>210602` Robot to delete not accessed users [#49]
- [x] `210421>210530` Run robot each 15 minutes and remove it from request
- [x] `210421>210421` Fix problem closing account (one of the locals is not closing)

## <a name="6.0.0.0"></a>6.0.0.0 :dragon: <sup>`7`</sup>
- [x] `210418>210418` Fix calendar selected day color for light theme
- [x] `210418>210418` Add link to create account at login app screen
- [x] `210417>210418` Adjust e-mail layouts to new themes
- [x] `210417>210417` Fix sum move after check it
- [x] `210325>210417` Store move locally when there is no internet (Mobile)
- [x] `210322>210406` Make machine ubuntu to build android
- [x] `210322>210322` Remove capitalize from toggle button

## <a name="5.0.1.0"></a>5.0.1.0 :sheep: <sup>`7`</sup>
- [x] `210319>210319` Fix color at start screen at mobile
- [x] `210317>210319` Fix color of dialogs at mobile
- [x] `210317>210319` Fix color of inputs at mobile
- [x] `210316>210317` Store logs at host machine instead of just inside docker
- [x] `210316>210316` Add monochromatic flag before hover
- [x] `210316>210316` Fix accept contract solo screen
- [x] `210316>210316` Fix stop wizard at configuration screen

## <a name="5.0.0.0"></a>5.0.0.0 :dragon: <sup>`1`</sup>
- [x] `210304>210314` Adjust [accessibilitity]

## <a name="4.6.3.6"></a>4.6.3.6 :ant: <sup>`1`</sup>
- [x] `210304>210304` Adjust version size

## <a name="4.6.3.5"></a>4.6.3.5 :ant: <sup>`1`</sup>
- [x] `210117>210302` Change server to linux

## <a name="4.6.3.4"></a>4.6.3.4 :ant: <sup>`1`</sup>
- [x] `210116>210116` Remove code that makes the log grow insanely

## <a name="4.6.3.3"></a>4.6.3.3 :ant: <sup>`2`</sup>
- [x] `210116>210116` Fix weird screen orientation change at requests
- [x] `210116>210116` Return logs grouped from server
- [x] `210116>210116` Fix app month navigation between years

## <a name="4.6.3.2"></a>4.6.3.2 :ant: <sup>`4`</sup>
- [x] `210116>210116` Fix schedules site screen
- [x] `210105>210114` Migrate e-mail to SES
- [x] `210104>210105` Remove unused libraries at android
- [x] `210103>210104` Add update log translations to version program (and commit)

## <a name="4.6.3.1"></a>4.6.3.1 :ant: <sup>`1`</sup>
- [x] `210102>210102` Migrate DB to RDS

## <a name="4.6.3.0"></a>4.6.3.0 :sheep: <sup>`6`</sup>
- [x] `210102>210102` Reset runned schedules flag if .net exception happens
- [x] `210102>210102` Add message of empty search result at site
- [x] `210101>210102` Add modal for moves with schedule, to show schedule details
- [x] `210101>210101` Fix move position at schedule
- [x] `210101>210101` Review columns of Schedule List (add deleted qty)
- [x] `201230>201231` Add modal for moves with details, to show details

## <a name="4.6.2.0"></a>4.6.2.0 :sheep: <sup>`8`</sup>
- [x] `201228>201229` Verify all docs links
- [x] `201228>201229` Avoid error on startup mvc
- [x] `201227>201228` Make program to finish process on git after PR
- [x] `201226>201227` Report with values: select to see sum
- [x] `201226>201226` Show account state on account area (totals)
- [x] `201225>201226` Add Year Negative / Positive / Sum to Year summary
- [x] `201223>201228` Use Authy as password
- [x] `201222>201223` Redesign TFA warning at config screen

## <a name="4.6.1.0"></a>4.6.1.0 :sheep: <sup>`4`</sup>
- [x] `201218>201218` Remove forgotten toast at android
- [x] `201214>201215` Replace close button by contact at app
- [x] `201214>201214` Fix error on reports [#83]
- [x] `201213>201218` Problem with receiving html instead of json [#84]

## <a name="4.6.0.1"></a>4.6.0.1 :sheep: <sup>`1`</sup>
- [x] `201213>201213` Fix recording log at mobile

## <a name="4.6.0.0"></a>4.6.0.0 :whale: <sup>`10`</sup>
- [x] `201211>201211` Add issue template to repo [#44]
- [x] `201211>201211` Add contributing to repo [#44]
- [x] `201210>201211` Fix contact modal from TASKS to RELEASES and TODOS
- [x] `201209>201210` Fix logic of version icon
- [x] `201204>201204` Replace {{Warning}} at e-mails by real message
- [x] `201209>201209` Remove forgotten toast at android
- [x] `201204>201204` Eliminate path from business logic, fixed path that MVC redirects
- [x] `201209>201209` Add code of conduct to repo [#44]
- [x] `201205>201208` Add contact link to when lose the TFA
- [x] `201130>201205` Link unsubscribe for moves (new security token)

## <a name="4.5.0.2"></a>4.5.0.2 :ant: <sup>`3`</sup>
- [x] `201128>201128` Fix android build number change
- [x] `201128>201128` Fix finish bugfix version
- [x] `201128>201128` Fix version check to do not fail at main

## <a name="4.5.0.1"></a>4.5.0.1 :ant: <sup>`6`</sup>
- [x] `201124>201124` Fix contract pt-br word
- [x] `201123>201127` Fix bug with android tests
- [x] `201122>201122` Fix bug at future moves reports, showing zero value for move
- [x] `201122>201123` Fix refresh gesture conflict with scroll of lists
- [x] `201121>201122` Fix edit detailed move at site
- [x] `201115>201123` Add create new task list at version changer (-q = quantity, -n = numbers)

## <a name="4.5.0.0"></a>4.5.0.0 :whale: <sup>`8`</sup>
- [x] `201111>201114` Fix robot running concurrently
- [x] `201101>201101` Add delete, check/uncheck and edit buttons on hold move (android)
- [x] `201031>201101` Fix problem with second call of ui wait (android)
- [x] `201018>201021` Add navigate gestures to reports (left to future, right to past)
- [x] `201018>201018` Add update gesture to all screens
- [x] `201018>201018` Fix error when trying to login at api
- [x] `201017>201018` Verify DFM status by error logs app
- [x] `201017>201017` Adapt android icon to new specifications

## <a name="4.4.0.0"></a>4.4.0.0 :whale: <sup>`4`</sup>
- [x] `201008>201016` Show future moves and foreseen balance [#21]
- [x] `201006>201006` Change android home screen to menu buttons only
- [x] `201006>201007` Group error logs at app
- [x] `201005>201005` Add [sitemap]

## <a name="4.3.0.0"></a>4.3.0.0 :whale: <sup>`9`</sup>
- [x] `201003>201004` Add change language icon to site
- [x] `201002>201003` Enable reopen account
- [x] `200929>200930` Last ACCESS date
- [x] `200927>201002` Search by move/detail
- [x] `200919>200926` Fix android intermitent tests
- [x] `200919>200919` Fix settings footer
- [x] `200919>200919` Add browser signup modal test
- [x] `200919>200919` Make cookie warning more showy
- [x] `200919>200919` Fix zero error code problem

## <a name="4.2.0.2"></a>4.2.0.2 :sheep: <sup>`1`</sup>
- [x] `200827>200915` Send site errors to cellphone

## <a name="4.2.0.1"></a>4.2.0.1 :sheep: <sup>`8`</sup>
- [x] `200825>200826` Add different icon for test at android
- [x] `200825>200825` Fix mobile Nature when open move app screen
- [x] `200825>200825` Fix tablet eternal rotation
- [x] `200821>200821` Add account name to \_layout of account area
- [x] `200821>200821` Fix numbers insertion at site (it's about language)
- [x] `200825>200825` Fix change account out to in at edit
- [x] `200825>200825` Put default amount to details field at mobile
- [x] `200820>200821` Fix error on add detail when editing move

## <a name="4.2.0.0"></a>4.2.0.0 :whale: <sup>`8`</sup>
- [x] `200701>200721` Make DTO use guid, not db id (delete fakeid)
- [x] `200628>200630` Handle CoreError at Robot
- [x] `200625>200626` Adjust tab order at move screen (mobile)
- [x] `200624>200624` Make nature a consequence of accounts choice
- [x] `200624>200624` Field names become hints (mobile)
- [x] `200623>200624` Change accounts to autocomplete (mobile)
- [x] `200620>200623` Change category to autocomplete (mobile)
- [x] `200616>200623` Change date to field with datepicker button (mobile)

## <a name="4.1.5.0"></a>4.1.5.0 :sheep: <sup>`5`</sup>
- [x] `200615>200615` Change menu github links to modal with options of all contact types (y, f, t, g) and release/todo
- [x] `200612>200613` Make it clear that clicking on account, you go to account area [#42]
- [x] `200611>200611` Fix accessibility of blue and red values at mobile
- [x] `200611>200611` Fix error testing url
- [x] `200611>200611` Fix environment variable to get server db config

## <a name="4.1.4.6"></a>4.1.4.6 :ant: <sup>`1`</sup>
- [x] `200610>200610` Reorganize e reprioritize task list

## <a name="4.1.4.5"></a>4.1.4.5 :ant: <sup>`3`</sup>
- [x] `200608>200608` Automatizate add tasks from docs to release list
- [x] `200605>200607` Change ci core tests machine to linux
- [x] `200604>200605` Fix android intermitent tests

## <a name="4.1.4.4"></a>4.1.4.4 :ant: <sup>`1`</sup>
- [x] `200415>200603` Change ci browser tests machine to linux

## <a name="4.1.4.3"></a>4.1.4.3 :ant: <sup>`2`</sup>
- [x] `200412>200414` Run browser tests at CI
- [x] `200409>200412` Run core tests at CI

## <a name="4.1.4.2"></a>4.1.4.2 :ant: <sup>`3`</sup>
- [x] `200217>200220` Run app tests at CI
- [x] `200218>200407` Make android automated tests
- [x] `200217>200221` Add android build to CI

## <a name="4.1.4.1"></a>4.1.4.1 :ant: <sup>`2`</sup>
- [x] `200208>200213` Build with circle CI
- [x] `200112>200208` Transform project into .NET Core

## <a name="4.1.4.0"></a>4.1.4.0 :sheep: <sup>`5`</sup>
- [x] `200112>200112` Enable to check moves on closed accounts
- [x] `200111>200112` Check move without reload screen - web
- [x] `200109>200110` Check move without reload screen - mobile
- [x] `191229>200108` Edit must uncheck move (warn before and after edit)
- [x] `191218>191218` Split move check to account in / out

## <a name="4.1.3.9"></a>4.1.3.9 :ant: <sup>`6`</sup>
- [x] `191203>191204` Fix logic of repeated account name (sql) [#32]
- [x] `191201>191201` Change account begin date when add a older move [#46]
- [x] `191130>191201` Fix gender on system resources
- [x] `191129>191129` Fix show terms to accept without TFA
- [x] `191129>191129` Fix android summary bug
- [x] `191128>191129` Review Wizard messages

## <a name="4.1.3.8"></a>4.1.3.8 :ant: <sup>`5`</sup>
- [x] `191121>191121` Add reference of where themes came from (docs / site settings)
- [x] `191121>191121` Original bootstrap on NONE theme
- [x] `191121>191121` Add PRIVACY to contract
- [x] `191120>191121` Get user timezone from browser
- [x] `191120>191120` Remove twitter timeline - use API (user data security)

## <a name="4.1.3.7"></a>4.1.3.7 :ant: <sup>`3`</sup>
- [x] `191115>191115` Fix MVC considering querystring as controller name on Language
- [x] `191115>191115` Handle Http HEAD method (return clean)
- [x] `190926>191116` Make puppeteer (browser) automated tests

## <a name="4.1.3.6"></a>4.1.3.6 :ant: <sup>`9`</sup>
- [x] `190924>190924` Fix account name repetition on area title
- [x] `190924>190924` Review contract, about account deletion and cookies
- [x] `190922>190922` Add test for empty ticket
- [x] `190920>190922` Transform SAFE service receiving / returns into DTO - check android
- [x] `190920>190920` Transform ROBOT service receiving / returns into DTO - check android
- [x] `190920>190920` Transform REPORT service receiving / returns into DTO - check android
- [x] `190918>190925` Transform MONEY service receiving / returns into DTO - check android
- [x] `190913>190918` Remove Month and Year (summaries goes to account)
- [x] `190908>190912` Transform ADMIN service receiving / returns into DTO - check android

## <a name="4.1.3.5"></a>4.1.3.5 :ant: <sup>`1`</sup>
- [x] `190912>190912` Fix bug on signup page

## <a name="4.1.3.4"></a>4.1.3.4 :ant: <sup>`3`</sup>
- [x] `190911>190911` Add contract text to DB
- [x] `190911>190911` Fix login problem on android
- [x] `190911>190911` Fix logins page bug

## <a name="4.1.3.3"></a>4.1.3.3 :ant: <sup>`4`</sup>
- [x] `190807>190808` IDs Int64
- [x] `190807>190807` Fix transaction call to avoid code duplication (Action/Func<>)
- [x] `190807>190807` Put warning of no content on year summary [#47]
- [x] `190906>190907` Fix register language half pt, half en [#41]

## <a name="4.1.3.2"></a>4.1.3.2 :ant: <sup>`4`</sup>
- [x] `190904>190905` Map repo issues to task list [#52]
- [x] `190903>190904` Cookies warning
- [x] `190901>190901` Fix detailed sum cents at e-mail
- [x] `190827>190831` Fix transient detail insertion

## <a name="4.1.3.1"></a>4.1.3.1 :ant: <sup>`1`</sup>
- [x] `190824>190826` Create automatic version change

## <a name="4.1.3.0"></a>4.1.3.0 :sheep: <sup>`4`</sup>
- [x] `190823>190825` Update SpecFlow version
- [x] `181213>190825` Tests for user permission
- [x] `181210>181211` Add press-and-hold to logout and exit
- [x] `181210>181210` Add IsAdm to User

## <a name="4.1.2.9"></a>4.1.2.9 :ant: <sup>`1`</sup>
- [x] `190703>190703` Adapt to new server

## <a name="4.1.2.8"></a>4.1.2.8 :ant: <sup>`5`</sup>
- [x] `190304>190304` Fix report when url year / month is out of range
- [x] `190304>190304` Fix duplicated month entities
- [x] `190304>190304` Add 404 from e-mail on web.config
- [x] `190304>190304` Add http method on 404 reporting e-mail
- [x] `190304>190304` Fix account not found on accounts area

## <a name="4.1.2.7"></a>4.1.2.7 :ant: <sup>`1`</sup>
- [x] `190222>190222` Update dependencies in .NET project

## <a name="4.1.2.6"></a>4.1.2.6 :ant: <sup>`1`</sup>
- [x] `181209>181209` Check and fix errors that arrived on e-mail

## <a name="4.1.2.5"></a>4.1.2.5 :ant: <sup>`5`</sup>
- [x] `181202>181202` Add action to Move Notification
- [x] `181201>181203` AntiForgery
- [x] `181127>181127` Avoid weird layout on changing theme
- [x] `181126>181127` Fix Image in Welcome
- [x] `181125>181125` Fix requests for pre-launch report

## <a name="4.1.2.4"></a>4.1.2.4 :ant: <sup>`1`</sup>
- [x] `181125>181125` Fix empty decryption

## <a name="4.1.2.3"></a>4.1.2.3 :ant: <sup>`1`</sup>
- [x] `181124>181124` Fix android max version number

## <a name="4.1.2.2"></a>4.1.2.2 :ant: <sup>`5`</sup>
- [x] `181121>181123` Encrypt sharedPreferences
- [x] `181121>181121` Given two security changes in this version, logoff all mobile
- [x] `181120>181121` Remove ticket from url (mobile)
- [x] `180526>181118` Kill state objects (mobile)
- [x] `180526>181118` Replace volley by retrofit

## <a name="4.1.2.1"></a>4.1.2.1 :ant: <sup>`3`</sup>
- [x] `180307>180307` Fix category repeated block at mobile/moves
- [x] `180307>180307` Fix nature issue on create at mobile/moves
- [x] `180306>180306` Fix add detail at mobile/moves

## <a name="4.1.2.0"></a>4.1.2.0 :sheep: <sup>`6`</sup>
- [x] `180302>180302` Remove license permission (not used anymore)
- [x] `180225>180225` Update Volley
- [x] `180226>180226` Remove findViewById
- [x] `180127>180226` Implement 2FA
- [x] `180119>180122` Create option of starting with wizard at sign up
- [x] `180118>180118` Add warning on move with category editing when categories are disabled

## <a name="4.1.1.3"></a>4.1.1.3 :ant: <sup>`3`</sup>
- [x] `180115>180115` Fix mobile to edit move with category when categories use are disabled
- [x] `180116>180116` Put list of feature on screen
- [x] `180114>180114` Fix button name on "You are offline"

## <a name="4.1.1.2"></a>4.1.1.2 :ant: <sup>`6`</sup>
- [x] `180107>180107` Fix date combo start value (mobile)
- [x] `180107>180107` Offer send e-mail on error (mobile)
- [x] `180107>180107` Fix move add from home (mobile)
- [x] `180105>180107` Fix month future year report
- [x] `180105>180105` Fix account list on transfer
- [x] `180105>180105` Fix task list duplication and already done

## <a name="4.1.1.1"></a>4.1.1.1 :ant: <sup>`2`</sup>
- [x] `171231>171231` Fix facebook image
- [x] `171231>171231` Fix android pig redirection

## <a name="4.1.1.0"></a>4.1.1.0 :sheep: <sup>`4`</sup>
- [x] `171229>171230` Mobile home link goes to app if it exists
- [x] `171022>171022` Fix show error at mobile
- [x] `171022>171022` Refactor android dialog handling
- [x] `171019>171019` Refactor android post/get handling

## <a name="4.1.0.1"></a>4.1.0.1 :ant: <sup>`2`</sup>
- [x] `171018>171018` Fix error when enter cents on mobile
- [x] `171008>171018` Refactor android activity helper classes

## <a name="4.1.0.0"></a>4.1.0.0 :whale: <sup>`3`</sup>
- [x] `171007>171008` Put titles on glyphicons
- [x] `171007>171007` Fix places where the understanding depends on colors
- [x] `171002>171003` Fix account stati icons - should not be all the same

## <a name="4.0.2.0"></a>4.0.2.0 :sheep: <sup>`5`</sup>
- [x] `171002>171002` Add package for JS libraries
- [x] `170924>171007` Upgrade to Android Studio 3.0
- [x] `170924>171007` Upgrade to VS 2017
- [x] `170924>170924` Add link to github Issues
- [x] `170924>170924` Add to github README the libraries used

## <a name="4.0.1.3"></a>4.0.1.3 :ant: <sup>`1`</sup>
- [x] `170927>170927` Fix THE NEW communication bug

## <a name="4.0.1.2"></a>4.0.1.2 :ant: <sup>`1`</sup>
- [x] `170924>170924` Fix mobile site communication

## <a name="4.0.1.1"></a>4.0.1.1 :ant: <sup>`7`</sup>
- [x] `170923>170923` Change password should drop logins
- [x] `170923>170923` Verify create / edit / schedule transfer move (browser)
- [x] `170919>170920` Contract acceptance warning at android
- [x] `170910>170910` Remove license check from android
- [x] `170916>170916` Fix delete empty account with detailed schedule
- [x] `170916>170916` Tests for delete empty account with detailed schedule
- [x] `170910>170910` Warning that you need to create categories on site to make moves

## <a name="4.0.1.0"></a>4.0.1.0 :sheep: <sup>`8`</sup>
- [x] `170909>170909` Make tasks file as MD
- [x] `170909>170909` Update twitter with recent versions
- [x] `170909>170909` Translate tasks file to english
- [x] `170909>170909` Make SVG pig
- [x] `170906>170906` Organize files used to manage project
- [x] `170906>170908` Create license file
- [x] `170906>170906` Make collaborate docs
- [x] `170906>170906` Config gitcop

## <a name="4.0.0.0"></a>4.0.0.0 :dragon: <sup>`4`</sup>
- [x] `170905>170905` Change password encryption
- [x] `170904>170905` Redirect known 404 to HELP
- [x] `170904>170904` Add Open Source to Contract
- [x] `170701>170904` Refactor commits to GIT

## <a name="3.0.0.7"></a>3.0.0.7 :ant: <sup>`1`</sup>
- [x] `170604>170826` Fix let's encrypt

## <a name="3.0.0.6"></a>3.0.0.6 :ant: <sup>`1`</sup>
- [x] `170521>170521` Fix schedule list natures - THE RETURN

## <a name="3.0.0.5"></a>3.0.0.5 :ant: <sup>`6`</sup>
- [x] `170501>170501` Fix e-mail urls when the e-mail is sent by API
- [x] `170501>170501` Fiz uncheck/check url at report
- [x] `170418>170418` Fix move single value edit at mobile
- [x] `170418>170418` Fix schedule list with categories disabled
- [x] `170418>170418` Fix schedule list natures
- [x] `170418>170418` Fix balance box size at mobile browser

## <a name="3.0.0.4"></a>3.0.0.4 :ant: <sup>`1`</sup>
- [x] `170418>170418` Fix schedule list (shows all the schedules of the system)

## <a name="3.0.0.3"></a>3.0.0.3 :ant: <sup>`1`</sup>
- [x] `170411>170411` Fix icon missing in the apps list

## <a name="3.0.0.2"></a>3.0.0.2 :ant: <sup>`7`</sup>
- [x] `170411>170411` Fix logged out 404
- [x] `170411>170411` New known 404 redirects
- [x] `170411>170411` Fix edit move the second time at mobile
- [x] `170410>170411` Reduce move form size at mobile
- [x] `170410>170410` Make red, green and blue fonts more visible mobile
- [x] `170410>170410` Fix of manifest warnings
- [x] `170410>170410` Shorten list app name

## <a name="3.0.0.1"></a>3.0.0.1 :ant: <sup>`11`</sup>
- [x] `170408>170408` Put date and category at same line on create/edit at mobile
- [x] `170408>170408` "Cancel" should not reload the page when cannot verify license
- [x] `170408>170408` Fix extract landscape layout
- [x] `170408>170408` Solve OutOfMemoryError when load welcome image
- [x] `170408>170408` Check android read state permission
- [x] `170406>170406` Fix mobile logo
- [x] `170406>170406` Highlight signup
- [x] `170406>170406` Fix move details (table) at e-mail
- [x] `170405>170406` Fix e-mail colors
- [x] `170405>170405` Fix empty account in / out at e-mail
- [x] `170405>170405` Shorten e-mail title

## <a name="3.0.0.0"></a>3.0.0.0 :dragon: <sup>`22`</sup>
- [x] `170401>170402` Pig video
- [x] `170329>170329` Change top menu by bottom menu (mobile)
- [x] `170329>170331` Remove blue from dark layout
- [x] `170329>170329` Fix icon list / menu mobile
- [x] `170328>170329` Fix move duplication (mobile)
- [x] `170327>170328` Android dark theme
- [x] `170327>170327` Fix cache on create/edit move (account/category) - and other crazy caches
- [x] `170327>170327` Fix move with schedule edition
- [x] `170326>170327` Change e-mail layouts - light / dark
- [x] `170325>170326` Theme config
- [x] `170324>170325` Add cookies warning at logon (remember me)
- [x] `170324>170324` Make new cover for youtube / facebook / twitter (?)
- [x] `170321>170324` Implement mandatory Term sign
- [x] `170320>170322` Write Terms of Use
- [x] `170319>170319` Allow negative account limit
- [x] `170319>170319` Change datepicker
- [x] `170301>170119` Layout Bootstrap
- [x] `170303>170305` Fix remember at login
- [x] `170224>170225` Fix android packages
- [x] `170223>170223` Add android start image
- [x] `170223>170223` Implement Admin/Robot GetList tests
- [x] `170209>170222` Android Kotlin

## <a name="2.1.4.4"></a>2.1.4.4 :ant: <sup>`1`</sup>
- [x] `170214>170215` Fix account / categories list when one more is created

## <a name="2.1.4.3"></a>2.1.4.3 :ant: <sup>`1`</sup>
- [x] `170209>170209` Fix NullReference

## <a name="2.1.4.2"></a>2.1.4.2 :ant: <sup>`1`</sup>
- [x] `170208>170208` Favicon and robots do not use session

## <a name="2.1.4.1"></a>2.1.4.1 :ant: <sup>`2`</sup>
- [x] `170208>170208` Fix android licensing
- [x] `170208>170208` Fix browser cookie

## <a name="2.1.4.0"></a>2.1.4.0 :sheep: <sup>`5`</sup>
- [x] `170207>170207` Making log at elmah in case of general fail / elmah test page
- [x] `170130>170130` SharedPreferences to store login / language
- [x] `170127>170128` Use Nuget packages
- [x] `170127>170127` Change apache by Volley
- [x] `170127>170127` Change licensing

## <a name="2.1.3.4"></a>2.1.3.4 :ant: <sup>`1`</sup>
- [x] `170127>170127` Fix duplication os details

## <a name="2.1.3.3"></a>2.1.3.3 :ant: <sup>`1`</sup>
- [x] `170115>170122` Adjust android layouts to use new format

## <a name="2.1.3.2"></a>2.1.3.2 :ant: <sup>`3`</sup>
- [x] `170117>170117` Adjust of number at move creation
- [x] `170117>170117` Adjust site (again)
- [x] `170117>170117` Adjust multilanguage

## <a name="2.1.3.1"></a>2.1.3.1 :ant: <sup>`1`</sup>
- [x] `170117>170117` Fix site address (published with error)

## <a name="2.1.3.0"></a>2.1.3.0 :sheep: <sup>`1`</sup>
- [x] `170115>170115` Hide column of check move (web/mobile)

## <a name="2.1.2.0"></a>2.1.2.0 :sheep: <sup>`2`</sup>
- [x] `170115>170115` Fix problem with out/in move
- [x] `170114>170115` Adjust NH to new forms

## <a name="2.1.1.0"></a>2.1.1.0 :sheep: <sup>`3`</sup>
- [x] `170114>170114` Remove front validation (date problem)
- [x] `170113>170114` Make e-mail change
- [x] `170113>170114` Make password change

## <a name="2.1.0.0"></a>2.1.0.0 :whale: <sup>`7`</sup>
- [x] `170113>170113` Fix e-mail urls
- [x] `170113>170113` Config to user enable move check - mobile
- [x] `170112>170113` Config to user enable move check - site
- [x] `160728>170112` Make check move - mobile
- [x] `160718>160718` Make check move - site
- [x] `160523>160524` Put account combos to enable change any account (out/in/transfer) - mobile
- [x] `160523>160523` Put account combos to enable change any account (out/in/transfer) - site

## <a name="2.0.3.1"></a>2.0.3.1 :ant: <sup>`9`</sup>
- [x] `160515>160515` Fix duplication when request reports (mobile)
- [x] `160515>160515` Show installment at mobile
- [x] `160515>160515` Leave Amazon sandbox
- [x] `160509>160515` Fix e-mail send by Amazon
- [x] `150919>160509` Config change tests
- [x] `150915>150917` Fix app at Lollipop
- [x] `150910>150910` Fix problem on open move creation (mobile)
- [x] `150910>150910` Fix delete account when it had moves
- [x] `150910>150910` Use colors at number on schedule screen

## <a name="2.0.3.0"></a>2.0.3.0 :sheep: <sup>`5`</sup>
- [x] `150907>150907` Let save the program at SD card
- [x] `150906>150907` Add move edit (mobile)
- [x] `150906>150906` Do not change orientation when its saving or deleting a move
- [x] `150906>150906` Do not let sleep when its waiting a request response
- [x] `150905>150905` Add close button (mobile)

## <a name="2.0.2.0"></a>2.0.2.0 :sheep: <sup>`13`</sup>
- [x] `150902>150903` Put project version at .NET
- [x] `150902>150902` Fix logo url
- [x] `150831>150831` Fix tablet error (no 3G)
- [x] `150830>150830` Migrate to android studio
- [x] `150214>150902` Put CANCEL button at Settings and Move (mobile)
- [x] `150214>150214` Put in and out at year report
- [x] `150214>150214` Put URLReferrer at 404 report
- [x] `150214>150214` Remove parent > child cascade from NH
- [x] `150214>150214` Fix e-mail and robots urls
- [x] `150111>150111` Review empty table layout
- [x] `150111>150111` Increase android version accordly with DFM downloads
- [x] `150111>150111` Fix menu click (right and middle mouse click)
- [x] `150111>150111` Handle logoff in the middle of ajax category creation

## <a name="2.0.1.2"></a>2.0.1.2 :ant: <sup>`2`</sup>
- [x] `150111>150111` Fix closed account list
- [x] `150108>150108` Fix move / schedule creation screen - value problem

## <a name="2.0.1.1"></a>2.0.1.1 :ant: <sup>`5`</sup>
- [x] `150101>150103` Fix test errors
- [x] `150103>150103` Change YellowLimit and RedLimit to Cents
- [x] `150101>150103` Fix error on mobile detailed move
- [x] `141230>141231` Fix move error with decimal value
- [x] `141230>141230` Fix login redirect bug

## <a name="2.0.1.0"></a>2.0.1.0 :sheep: <sup>`9`</sup>
- [x] `141221>141225` Put move delete (mobile)
- [x] `141207>141207` Extract: make year combo change if month combo restart
- [x] `141207>141207` Find TO-DOs at project
- [x] `141207>141207` Refactor at controllers and areas, inverse plural
- [x] `141207>141207` Change Extensions Sum to Total
- [x] `141206>141207` At Move, put Value as recorded field and use Total to show
- [x] `141201>141201` Put integer values at DB (double > int)
- [x] `141129>141129` Create unit test of deleting all the moves of a schedule
- [x] `141129>141129` Create unit test of schedule half past / half future

## <a name="2.0.0.3"></a>2.0.0.3 :ant: <sup>`12`</sup>
- [x] `141020>141021` Fix apk for kitkat
- [x] `140725>140725` Delete schedule of deleted account
- [x] `140725>140725` Lock schedule of closed account
- [x] `140706>140706` Security disable the older securities
- [x] `140706>140706` Deactivate user if it use wrong password 5 times
- [x] `140706>140706` Check if password reset disables user
- [x] `140706>140706` Handle error at e-mail send (Security)
- [x] `140706>140706` Fix error when requesting report that doesn't have Month/Year
- [x] `140705>140706` Move generic itens do Ak
- [x] `140629>140629` Make move without category fix its summaries
- [x] `140629>140629` Fix tests of e-mail error
- [x] `140626>140626` Fix detailed schedule

## <a name="2.0.0.2"></a>2.0.0.2 :ant: <sup>`10`</sup>
- [x] `140620>140620` Put version at footer
- [x] `140620>140620` Put logon link at password reset
- [x] `140618>140619` Stop request at activity destroy
- [x] `140611>140617` Update project: MVC 5
- [x] `140609>140609` Update project: .NET 4.5
- [x] `140609>140609` Update project: VS 2013
- [x] `140609>140609` Fix transform for rewrite
- [x] `140605>140616` Review rotate logic
- [x] `140604>140605` Handle differentely EConnReset
- [x] `140603>140604` Stop user google license is denied

## <a name="2.0.0.1"></a>2.0.0.1 :ant: <sup>`1`</sup>
- [x] `140601>140602` Adjust move data loosing when orientation changes

## <a name="2.0.0.0"></a>2.0.0.0 :dragon: <sup>`71`</sup>
- [x] `140531>140531` Force HTTPS access
- [x] `140531>140531` Remove BETA
- [x] `140528>140528` Put play link at site
- [x] `140525>140531` Create product at google play
- [x] `140525>140528` HTTPS!!!
- [x] `140519>140519` Orientation change at move must set fields functions
- [x] `140519>140519` Orientation change at reports must keep time filter
- [x] `140519>140519` Scroll at move details
- [x] `140519>140519` Fix mobile extract month and year do extract
- [x] `140519>140519` Fix transfer move edit
- [x] `140518>140519` Review intro text
- [x] `140512>140516` Remove Release from versioning, change db name, user and password
- [x] `140511>140511` Remove post data if it's passwords or values
- [x] `140510>140511` Fix server slowness
- [x] `140504>140504` Delete PageLog
- [x] `140504>140504` Fix language saving
- [x] `140504>140504` Fix table for too big texts
- [x] `140504>140504` Show just 2 digits as cents (mobile)
- [x] `140504>140504` Change language before screen render
- [x] `140504>140504` Record language locally
- [x] `140504>140504` Fix logoff (M)
- [x] `140418>140418` Make activity do not call the pig if it's just orientation change
- [x] `140418>140418` Fix nature translation
- [x] `140418>140418` Make zero show up as blue
- [x] `140418>140418` Fix e-mail layout links
- [x] `140414>140414` Fix when turn device
- [x] `140414>140414` List logins to logoff
- [x] `140414>140414` Put user default language as browser language
- [x] `140414>140414` Deny open dfm at iframe / X-frame-options
- [x] `140414>140414` List schedule / allow stop schedule
- [x] `140414>140414` Schedule limite label / Detailed label
- [x] `140414>140414` Show move date at landscape
- [x] `140408>140414` Change mobile language to user language
- [x] `140407>140407` Create config screen to user (Web)
- [x] `140407>140407` Redirect, after create move, to extract of that move
- [x] `140331>140331` Order Moves by ID too
- [x] `140331>140331` Fix amount default at move screen
- [x] `140323>140324` Put current balance at Extract
- [x] `140323>140323` Put categories / account combo list on alphabetical order
- [x] `140323>140323` Change Accounts button by Home image, Logoff button by Logoff image
- [x] `140323>140323` Fix session schema NH
- [x] `140323>140323` Fix tests from not changing summaries when there is no category
- [x] `140316>140323` Handle FailOnEmailSend at Robot / Move > record and warn that it doesn't sent the e-mail
- [x] `140305>140305` Create warning box
- [x] `140305>140305` Make config screen for user (Mobile)
- [x] `140305>140305` Hide category when user do not want categories
- [x] `140304>140305` Make possible to not choose category
- [x] `140304>140304` Separate User additional data (Config)
- [x] `140304>140304` Fix error at turning device at move creation screen (Mobile)
- [x] `140304>140304` Put amount default 1 (Mobile)
- [x] `140303>140303` Put "ok" at error alert (Mobile)
- [x] `140303>140303` Change "search by name" by "search by url" at account
- [x] `140225>140303` Make move recording (Mobile)
- [x] `140125>140223` Create screen to save move (Mobile)
- [x] `140125>140125` Create accounts summary (Mobile)
- [x] `140125>140125` Put scroll and loading
- [x] `140123>140123` Make assync site call
- [x] `140106>140106` Move strings to resource
- [x] `140101>140106` Create month report (Mobile)
- [x] `131230>140101` Put account list (Mobile)
- [x] `131228>131231` Remake app
- [x] `131108>131108` Fix move insertion date and time limit
- [x] `131004>131016` Make login by ticket at website
- [x] `131004>131004` Create App Mobile
- [x] `130904>131004` Decide about objects passing to external
- [x] `131003>131003` Fix bug when adding details to an already created move
- [x] `131003>131003` Fix detailed scheduling
- [x] `131002>131002` Hide Move and Schedule from closed accounts
- [x] `131002>131002` Change closed account to be same as opened, but without edit possibility
- [x] `130928>131002` Review layout [(goodui)]
- [x] `130902>130902` Create Json area

## <a name="1.1.0.0"></a>1.1.0.0 :whale: <sup>`5`</sup>
- [x] `130928>130928` Stop datepicker from filling today at edit
- [x] `130928>130928` Fix datepicker at chrome
- [x] `130927>130927` Put Enter / Logoff as buttons at top menu (external)
- [x] `130927>130927` Put Categories / Accounts / Logoff as buttons at top menu (internal)
- [x] `130927>130927` Remove loggedin user top menu

## <a name="1.0.4.0"></a>1.0.4.0 :sheep: <sup>`2`</sup>
- [x] `130926>130927` Fix detailed move creation
- [x] `130926>130926` Put dot at thousand

## <a name="1.0.3.1"></a>1.0.3.1 :ant: <sup>`2`</sup>
- [x] `130830>130830` [Fix this]
- [x] `130824>130830` Move Controllers logic to Models

## <a name="1.0.3.0"></a>1.0.3.0 :sheep: <sup>`18`</sup>
- [x] `130818>130822` Add Broken to summary, and use RobotService to fix
- [x] `130809>130822` Solve update Summaries problem when something change the value
- [x] `130806>130809` Use just one cookie to get everything from server
- [x] `130805>130806` Add link to login screen at User Verification screen
- [x] `130802>130802` Split Controller Move in Move / Schedule / Detail
- [x] `130722>130801` Review Schedule to implement IMove / kill FutureMove
- [x] `130717>130730` Fix tests
- [x] `130716>130716` Replace Select by Get
- [x] `130715>130716` Create SessionOld to use at GetByIdOld
- [x] `130715>130715` [put Index]
- [x] `130715>130715` Dic [(error 1)] / [(error 2)]
- [x] `130324>130324` Suggest account url
- [x] `130324>130324` Fix NullReference when access non-existent [account url]
- [x] `130318>130324` Use Identity (Generic) to store logged user
- [x] `130318>130318` Remove Authentication setter return
- [x] `130317>130317` Make ticket logout
- [x] `130316>130317` Replace SelectByEmail of User by SelectByTicket
- [x] `130311>130314` Make ValidateAndGet get Ticket String

## <a name="1.0.2.3"></a>1.0.2.3 :ant: <sup>`17`</sup>
- [x] `130307>130307` Protect hiddens edition, where its necessary
- [x] `130307>130307` Verify Move deletion with Schedule (ScheduleTimesCantBeZero)
- [x] `130307>130307` Do not send e-mail in case of error when edit move
- [x] `130307>130307` Protect object edit (user) at core
- [x] `130307>130307` Put numbers at enums (safer to DB)
- [x] `130303>130307` Use account url at MVC
- [x] `130303>130303` Add url choice to account
- [x] `130224>130303` Create Category / Account name edit (including mvc)
- [x] `130221>130222` Replace SelectById  of Account / Category by SelectByName
- [x] `130221>130222` Change places where any entity except move and detail are called by its ID (including MVC)
- [x] `130221>130221` Send Accounts / Category as name to save move
- [x] `130221>130221` Fix transfer e-mail
- [x] `130220>130221` User protection at Core (verify active too)
- [x] `130220>130220` Isolate authentication project
- [x] `121017>130220` Fix MVC to send category and schedule separated from move
- [x] `121017>121017` Put visit log (IP, Date/Time, Address)
- [x] `121017>121017` Fix e-mail config at web.config

## <a name="1.0.2.2"></a>1.0.2.2 :ant: <sup>`22`</sup>
- [x] `120820>120821` Multilanguage tests
- [x] `120820>120820` Test close account and  disable category (MVC)
- [x] `120817>120817` Add SMTP to web.config
- [x] `120816>120816` Test enter with invalid user (MVC)
- [x] `120816>120816` Put robot.txt for google
- [x] `120816>120816` Remake service method order
- [x] `120809>120809` Fix DB config at webconfig release
- [x] `120809>120809` Put Post data on error e-mail
- [x] `120718>120718` Send parent to superservices, instead of all services
- [x] `120718>120718` Create Session class to Begin/Commit/Rollback
- [x] `120718>120718` Change this[] to return new object if none is found
- [x] `120617>120704` Create move edit tests
- [x] `120614>120614` Add Assert to check whether month/year do not change in case of error (moneyservice)
- [x] `120614>120614` Verify at tests if summaries are not changed in case of error
- [x] `120614>120614` Create too big names test
- [x] `120614>120614` Create move/schedule creation at closed account test
- [x] `120614>120614` Create move/schedule creation at disabled category test
- [x] `120614>120614` Put category/schedule as move save parameter
- [x] `120603>120603` Remove Transaction return from Begin (use session transaction)
- [x] `120407>120407` Save User should receive e-mail and password
- [x] `120406>120613` Implement BDD
- [x] `120318>120406` Write BDD

## <a name="1.0.2.1"></a>1.0.2.1 :ant: <sup>`2`</sup>
- [x] `120321>120322` Send error details on report e-mail
- [x] `120320>120320` Refactor Resharper

## <a name="1.0.2.0"></a>1.0.2.0 :sheep: <sup>`13`</sup>
- [x] `120317>120317` Publish, test if logs are working, and remove error from report Year 18967
- [x] `120316>120317` Test Elmah with forced error (Report > SeeYear)
- [x] `120314>120316` Remove delegates from Getters to send e-mail
- [x] `120314>120314` Separate Multilanguage from MVC
- [x] `120314>120314` Remove delegate DeleteSummary / another tries duplicated
- [x] `120202>120314` When the schedule is boundless, when the futuremove becomes a move, it should create another future move
- [x] `120130>120131` Layout with shadows and 3D
- [x] `120130>120131` Option to put (current/total) on scheduled move descriptions
- [x] `120128>120128` Highlight chosen account (account area)
- [x] `120104>120127` Change Scheduler to use FutureMove
- [x] `111228>120103` Create superServices and migrate use to them
- [x] `111205>111219` Separate BusinessLogic from NHibernate
- [x] `111202>111205` Reorganizing layers

## <a name="1.0.1.0"></a>1.0.1.0 :sheep: <sup>`5`</sup>
- [x] `111125>111130` Tests and Publish
- [x] `111025>111025` Error handling
- [x] `111025>111025` Protection for Elmah log
- [x] `111024>111025` Try to pass Log4Net / Elmah to MySql: FAIL
- [x] `111019>111019` Log4Net / Elmah

## <a name="1.0.0.4"></a>1.0.0.4 :ant: <sup>`6`</sup>
- [x] `111111>111111` Enable ZERO for red and yellow signs
- [x] `111109>111109` Verify if password recover disable user
- [x] `111109>111122` Send e-mail on move creation (user will be able to disable in the future)
- [x] `111109>111109` Replace links by icons at categories
- [x] `111108>111108` Alphabetical order at accounts (menu, opened, closed) and categories
- [x] `111023>111023` Fix error on transfer move: was not getting accounts

## <a name="1.0.0.3"></a>1.0.0.3 :ant: <sup>`4`</sup>
- [x] `111013>111014` Fix error with move creation (foreign key fail)
- [x] `111011>111013` Use icons instead of words at accounts list
- [x] `111010>111010` Validate user by e-mail - hashcode (WARN ABOUT NO LINK TO FINANCIAL INSTITUTIONS)
- [x] `111002>111009` Password recover - hashcode

## <a name="1.0.0.2"></a>1.0.0.2 :ant: <sup>`5`</sup>
- [x] `111001>111001` Put BETA at site logo
- [x] `111001>111001` Make error message darker (inverse color?)
- [x] `111001>111001` User register date
- [x] `110928>111001` Password confirm
- [x] `110925>110926` Date format conflict

## <a name="1.0.0.1"></a>1.0.0.1 :ant: <sup>`2`</sup>
- [x] `110924>110924` Move, when change In>Out or Out>In, was showing error about the other account
- [x] `110924>110924` Fix NH access at publish site

## <a name="1.0.0.0"></a>1.0.0.0 :dragon: <sup>`52`</sup>
- [x] `110922>110922` Publish
- [x] `110922>110922` Verify IE
- [x] `110921>110922` Limit text field sizes
- [x] `110921>110921` Encrypt password
- [x] `110918>110919` Implement account limits (and warning about limits) - yellow / red sign
- [x] `110918>110918` Remove account nature. FORGET IT: Change account creation / edit to add details
- [x] `110915>110915` Robot for fix taxes
- [x] `110915>110915` Bank: taxes + date / Debt: grow percent + date  / Credit: taxes + data, limit  / Charge: daily spending, max charge
- [x] `110915>110915` Refactor: put error texts in enum
- [x] `110915>110915` Remove Login field
- [x] `110915>110915` Protect edit / delete that do not belongs to logged in user
- [x] `110913>110915` Break Summary Value by In / Out
- [x] `110912>110912` Implement frequency Daily at Scheduler (test functions that use Enum values)
- [x] `110911>110912` if the first schedule is future, insert Move and save new when run, to change Summary
- [x] `110910>110910` Arrancar IsValid de Summary
- [x] `110907>110911` Create schedule robot (control by request) - get last and copy
- [x] `110905>110906` Interface of boundless schedule
- [x] `110904>110905` Refactor: extensions of entities, so just the props stay with entities
- [x] `110902>110902` Refactor: kill SelectOne and Select with lambda
- [x] `110828>110902` Fix moves (create Schedule)
- [x] `110827>110827` Change color to yellow / put icon
- [x] `110823>110827` Review layout (verify top menu, if it fits)
- [x] `110823>110823` Submenu with reports
- [x] `110822>110822` Report link of closed account goes to last reports
- [x] `110822>110822` Empty account - delete / Account with Moves - close
- [x] `110822>110822` Put enter button, when already logged in
- [x] `110822>110822` Make EntityData static
- [x] `110819>110821` Unify layout page
- [x] `110818>110822` Navigation Month / Year
- [x] `110818>110818` Remove quantity from single value
- [x] `110816>110817` Layout
- [x] `110815>110816` WCF started
- [x] `110814>110814` Implement "keep logged in"
- [x] `110810>110814` Move schedule
- [x] `110809>110810` REFACTORING
- [x] `110803>110808` Select with lambda expression at BaseData
- [x] `110801>110803` Month balance (adapt move edit / creation)
- [x] `110801>110801` Category edit
- [x] `110801>110801` Category disable
- [x] `110731>110801` Categories list
- [x] `110725>110731` Translation
- [x] `110724>110725` Multilanguage
- [x] `110724>110724` Listing disabled accounts
- [x] `110724>110724` Account disable
- [x] `110723>110724` Account edit
- [x] `110723>110723` Details delete
- [x] `110723>110723` Implement quantity at details
- [x] `110723>110723` Move delete
- [x] `110718>110723` Remove Transfer entity
- [x] `110629>110718` Move edit
- [x] `110620>110629` Remake routing to have route by account
- [x] `110601>110620` Move details

[account url]: http://www.dontflymoney.com/Accounts/19/Report/ShowMoves
[(error 1)]: http://dontflymoney.com/?dgcr=1
[(error 2)]: http://dontflymoney.com/?author=1
[put Index]: http://www.dontflymoney.com/Token/
[Fix this]: http://www.dontflymoney.com/Token/Received
[(goodui)]: http://goodui.org/
[#52]: https://github.com/darakeon/dfm/issues/52
[#41]: https://github.com/darakeon/dfm/issues/41
[#47]: https://github.com/darakeon/dfm/issues/47
[#46]: https://github.com/darakeon/dfm/issues/46
[#32]: https://github.com/darakeon/dfm/issues/32
[#42]: https://github.com/darakeon/dfm/issues/42
[sitemap]: https://support.google.com/webmasters/answer/7451001
[#21]: https://github.com/darakeon/dfm/issues/21
[#44]: https://github.com/darakeon/dfm/issues/44
[#84]: https://github.com/darakeon/dfm/issues/84
[#83]: https://github.com/darakeon/dfm/issues/83
[accessibilitity]: https://chrome.google.com/webstore/detail/axe-coconut-web-accessibi/iobddmbdndbbbfjopjdgadphaoihpojp?hl=en
[#49]: https://github.com/darakeon/dfm/issues/49
[chart]: https://www.highcharts.com/demo/pie-legend
[#107]: https://github.com/darakeon/dfm/issues/107
[how to commit ci]: https://support.circleci.com/hc/en-us/articles/360018860473-How-to-push-a-commit-back-to-the-same-repository-as-part-of-the-CircleCI-job?utm_source=google&utm_medium=sem&utm_campaign=sem-google-dg--latam-en-dsa-maxConv-auth-nb&utm_term=g_b-_m__dsa_&utm_content=&gclid=EAIaIQobChMIvaiowOO59AIVChCRCh0rgA0SEAAYASAAEgLqdvD_BwE
[.NET Lib]:https://github.com/prometheus-net/prometheus-net
[django-prod]:https://docs.djangoproject.com/en/4.1/howto/deployment/checklist/
[django-nginx]:https://realpython.com/django-nginx-gunicorn/
