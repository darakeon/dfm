<img src="../site/DFM.MVC/Assets/Images/pig.svg" width="85" align="left"/>

# Task list

This is the task list for the project. Done, Doing, To-do, all here, all planned. =)

- [go to published version](#prod)
- [go to version in development](#dev)
- [dev version state](../../4.1.3.7/docs/TASKS.md#dev)

Legend:
- :dragon:: a huge change in system
- :whale:: big changes, like a new feature
- :sheep:: little changes, as a change in an existing feature
- :ant:: the developer is just fixing some sh*t it did

## 8.0.0.0 :dragon: <sup>`3`</sup>
- [ ] Make/find app to verify DFM status
- [ ] Link unsubscribe
- [ ] Run robot each hour and remove it from request (Rust)

## 7.0.0.0 :dragon: <sup>`2`</sup>
- [ ] Create service to translate site ("put in your language")
- [ ] Use .NET resources

## 6.0.0.0 :dragon: <sup>`1`</sup>
- [ ] OCR to add values to system

## 5.6.0.0 :whale: <sup>`1`</sup>
- [ ] Add spendable value - how much one can spend by day [#28](https://github.com/darakeon/dfm/issues/28)

## 5.5.1.0 :sheep: <sup>`2`</sup>
- [ ] Add priority account (listed above others) [#50](https://github.com/darakeon/dfm/issues/50)
- [ ] Add priority category (listed above others) [#50](https://github.com/darakeon/dfm/issues/50)

## 5.5.0.0 :whale: <sup>`5`</sup>
- [ ] E-mail send mock
- [ ] "Guess" the category
- [ ] report with more than one account
- [ ] year report by category [(chartjs)](http://www.chartjs.org/)
- [ ] month report by category [(chartjs)](http://www.chartjs.org/)

## 5.4.1.0 :sheep: <sup>`3`</sup>
- [ ] Fix error handling to have all at header
- [ ] Put name (nickname?) on sign up
- [ ] Remove account name oneness (keep for Url)

## 5.4.0.0 :whale: <sup>`5`</sup>
- [ ] Feature to join Categories
- [ ] Link account and category (combo)
- [ ] Report with date filters
- [ ] Report with values: select to see sum
- [ ] Send e-mail if account is above yellow and red (add config to allow)

## 5.3.0.0 :whale: <sup>`4`</sup>
- [ ] Delete not accessed accounts [#49](https://github.com/darakeon/dfm/issues/49)
- [ ] Last login date
- [ ] Data export (csv, json)
- [ ] Data import (csv, json)

## 5.2.0.0 :whale: <sup>`4`</sup>
- [ ] Enable reopen account
- [ ] Config max request length
- [ ] Show future moves and foreseen balance [#21](https://github.com/darakeon/dfm/issues/21)
- [ ] Add config for first day of account

## 5.1.0.0 :whale: <sup>`1`</sup>
- [ ] Create Market - group of moves, each one of one category, to when you are shopping

## 5.0.0.0 :dragon: <sup>`4`</sup>
- [ ] Add contributing rules to repo [#44](https://github.com/darakeon/dfm/issues/44)
- [ ] Add code of conduct to repo [#44](https://github.com/darakeon/dfm/issues/44)
- [ ] Add issue template to repo [#43](https://github.com/darakeon/dfm/issues/43)
- [ ] Store data locally, to use when there is no internet (Mobile)

## 4.5.0.0 :whale: <sup>`2`</sup>
- [ ] Add Negative / Positive / Sum to Year summary
- [ ] Add Balance Negative / Positive / Sum to Year summary

## 4.4.0.0 :whale: <sup>`5`</sup>
- [ ] Add Template to Mobile
- [ ] Create Template (Move)
- [ ] Enable Copy Move (web+mobile)
- [ ] Search by category
- [ ] Search by move

## 4.3.1.0 :sheep: <sup>`2`</sup>
- [ ] If session drop, call history.go(-2) after re-login
- [ ] Remove GetOrCreate

## 4.3.0.1 :ant: <sup>`3`</sup>
- [ ] Make DTO use guid, not db id
- [ ] Handle DFMCoreException at Robot
- [ ] Change fields to use the ones firefox and chrome customize

## 4.3.0.0 :whale: <sup>`4`</sup>
- [ ] Put info that you can disabled mobile login if your cellphone is stolen
- [ ] Adjust tab order at move screen (mobile)
- [ ] Add account creation (mobile)
- [ ] Add category creation (mobile)

## 4.2.0.0 :whale: <sup>`5`</sup>
- [ ] Allow to hide values at start screen (mobile)
- [ ] Change date to field with datepicker button (mobile)
- [ ] Change category to autocomplete (mobile)
- [ ] Move datetime spinner to the side of the button (remove modal)
- [ ] Make it clear that clicking on account, you go to account area [#42](https://github.com/darakeon/dfm/issues/42)

## 4.1.4.0 :sheep: <sup>`6`</sup>
- [ ] Allow check all the moves before a date
- [ ] Enable to check moves on closed accounts
- [ ] Check move without reload screen - web
- [ ] Check move without reload screen - mobile
- [ ] Edit must uncheck move (warn before and after edit)
- [ ] Split move check to account in / out

## 4.1.3.8 :ant: <sup>`2`</sup>
- [ ] Fix logic of repeated account name (sql) [#32](https://github.com/darakeon/dfm/issues/32)
- [ ] Change account begin date when add a older move [#46](https://github.com/darakeon/dfm/issues/46)

## <a name="dev"></a>4.1.3.7 :ant: <sup>`2`</sup>
- [ ] Make android automated tests
- [ ] Make selenium automated tests - web (maybe puppeteer)

## <a name="prod"></a>4.1.3.6 :ant: <sup>`9`</sup>
- [x] `190924>190924` Fix account name repetition on area title
- [x] `190924>190924` Review contract, about account deletion and cookies
- [x] `190922>190922` Add test for empty ticket
- [x] `190920>190922` Transform SAFE service receiving / returns into DTO - check android
- [x] `190920>190920` Transform ROBOT service receiving / returns into DTO - check android
- [x] `190920>190920` Transform REPORT service receiving / returns into DTO - check android
- [x] `190918>190925` Transform MONEY service receiving / returns into DTO - check android
- [x] `190913>190918` Remove Month and Year (summaries goes to account)
- [x] `190908>190912` Transform ADMIN service receiving / returns into DTO - check android

## 4.1.3.5 :ant: <sup>`4`</sup>
- [x] `190912>190912` Fix bug on signup page

## 4.1.3.4 :ant: <sup>`3`</sup>
- [x] `190911>190911` Add contract text to DB
- [x] `190911>190911` Fix login problem on android
- [x] `190911>190911` Fix logins page bug

## 4.1.3.3 :ant: <sup>`4`</sup>
- [x] `190807>190808` IDs Int64
- [x] `190807>190807` Fix transaction call to avoid code duplication (Action/Func<>)
- [x] `190807>190807` Put warning of no content on year summary [#47](https://github.com/darakeon/dfm/issues/47)
- [x] `190906>190907` Fix register language half pt, half en [#41](https://github.com/darakeon/dfm/issues/41)

## 4.1.3.2 :ant: <sup>`4`</sup>
- [x] `190904>190905` Map repo issues to task list [#52](https://github.com/darakeon/dfm/issues/52)
- [x] `190903>190904` Cookies warning
- [x] `190901>190901` Fix detailed sum cents at e-mail
- [x] `190827>190831` Fix transient detail insertion

## 4.1.3.1 :ant: <sup>`1`</sup>
- [x] `190824>190826` Create automatic version change

## 4.1.3.0 :sheep: <sup>`4`</sup>
- [x] `190823>190825` Update SpecFlow version
- [x] `181213>190825` Tests for user permission
- [x] `181210>181211` Add press-and-hold to logout and exit
- [x] `181210>181210` Add IsAdm to User

## 4.1.2.9 :ant: <sup>`1`</sup>
- [x] `190703>190703` Adapt to new server

## 4.1.2.8 :ant: <sup>`5`</sup>
- [x] `190304>190304` Fix report when url year / month is out of range
- [x] `190304>190304` Fix duplicated month entities
- [x] `190304>190304` Add 404 from e-mail on web.config
- [x] `190304>190304` Add http method on 404 reporting e-mail
- [x] `190304>190304` Fix account not found on accounts area

## 4.1.2.7 :ant: <sup>`1`</sup>
- [x] `190222>190222` Update dependencies in .NET project

## 4.1.2.6 :ant: <sup>`1`</sup>
- [x] `181209>181209` Check and fix errors that arrived on e-mail

## 4.1.2.5 :ant: <sup>`5`</sup>
- [x] `181202>181202` Add action to Move Notification
- [x] `181201>181203` AntiForgery
- [x] `181127>181127` Avoid weird layout on changing theme
- [x] `181126>181127` Fix Image in Welcome
- [x] `181125>181125` Fix requests for pre-launch report

## 4.1.2.4 :ant: <sup>`1`</sup>
- [x] `181125>181125` Fix empty decryption

## 4.1.2.3 :ant: <sup>`1`</sup>
- [x] `181124>181124` Fix android max version number

## 4.1.2.2 :ant: <sup>`5`</sup>
- [x] `181121>181123` Encrypt sharedPreferences
- [x] `181121>181121` Given two security changes in this version, logoff all mobile
- [x] `181120>181121` Remove ticket from url (mobile)
- [x] `180526>181118` Kill state objects (mobile)
- [x] `180526>181118` Replace volley by retrofit

## 4.1.2.1 :ant: <sup>`3`</sup>
- [x] `180307>180307` Fix category repeated block at mobile/moves
- [x] `180307>180307` Fix nature issue on create at mobile/moves
- [x] `180306>180306` Fix add detail at mobile/moves

## 4.1.2.0 :sheep: <sup>`6`</sup>
- [x] `180302>180302` Remove license permission (not used anymore)
- [x] `180225>180225` Update Volley
- [x] `180226>180226` Remove findViewById
- [x] `180127>180226` Implement 2FA
- [x] `180119>180122` Create option of starting with wizard at sign up
- [x] `180118>180118` Add warning on move with category editing when categories are disabled

## 4.1.1.3 :ant: <sup>`3`</sup>
- [x] `180115>180115` Fix mobile to edit move with category when categories use are disabled
- [x] `180116>180116` Put list of feature on screen
- [x] `180114>180114` Fix button name on "You are offline"

## 4.1.1.2 :ant: <sup>`6`</sup>
- [x] `180107>180107` Fix date combo start value (mobile)
- [x] `180107>180107` Offer send e-mail on error (mobile)
- [x] `180107>180107` Fix move add from home (mobile)
- [x] `180105>180107` Fix month future year report
- [x] `180105>180105` Fix account list on transfer
- [x] `180105>180105` Fix task list duplication and already done

## 4.1.1.1 :ant: <sup>`2`</sup>
- [x] `171231>171231` Fix facebook image
- [x] `171231>171231` Fix android pig redirection

## 4.1.1.0 :sheep: <sup>`4`</sup>
- [x] `171229>171230` Mobile home link goes to app if it exists
- [x] `171022>171022` Fix show error at mobile
- [x] `171022>171022` Refactor android dialog handling
- [x] `171019>171019` Refactor android post/get handling

## 4.1.0.1 :ant: <sup>`2`</sup>
- [x] `171018>171018` Fix error when enter cents on mobile
- [x] `171008>171018` Refactor android activity helper classes

## 4.1.0.0 :whale: <sup>`3`</sup>
- [x] `171007>171008` Put titles on glyphicons
- [x] `171007>171007` Fix places where the understanding depends on colors
- [x] `171002>171003` Fix account stati icons - should not be all the same

## 4.0.2.0 :sheep: <sup>`5`</sup>
- [x] `171002>171002` Add package for JS libraries
- [x] `170924>171007` Upgrade to Android Studio 3.0
- [x] `170924>171007` Upgrade to VS 2017
- [x] `170924>170924` Add link to github Issues
- [x] `170924>170924` Add to github README the libraries used

## 4.0.1.3 :ant: <sup>`1`</sup>
- [x] `170927>170927` Fix THE NEW communication bug

## 4.0.1.2 :ant: <sup>`1`</sup>
- [x] `170924>170924` Fix mobile site communication

## 4.0.1.1 :ant: <sup>`7`</sup>
- [x] `170923>170923` Change password should drop logins
- [x] `170923>170923` Verify create / edit / schedule transfer move (browser)
- [x] `170919>170920` Contract acceptance warning at android
- [x] `170910>170910` Remove license check from android
- [x] `170916>170916` Fix delete empty account with detailed schedule
- [x] `170916>170916` Tests for delete empty account with detailed schedule
- [x] `170910>170910` Warning that you need to create categories on site to make moves

## 4.0.1.0 :sheep: <sup>`8`</sup>
- [x] `170909>170909` Make tasks file as MD
- [x] `170909>170909` Update twitter with recent versions
- [x] `170909>170909` Translate tasks file to english
- [x] `170909>170909` Make SVG pig
- [x] `170906>170906` Organize files used to manage project
- [x] `170906>170908` Create license file
- [x] `170906>170906` Make collaborate docs
- [x] `170906>170906` Config gitcop

## 4.0.0.0 :dragon: <sup>`4`</sup>
- [x] `170905>170905` Change password encryption
- [x] `170904>170905` Redirect known 404 to HELP
- [x] `170904>170904` Add Open Source to Contract
- [x] `170701>170904` Refactor commits to GIT

## 3.0.0.7 :ant: <sup>`1`</sup>
- [x] `170604>170826` Fix let's encrypt

## 3.0.0.6 :ant: <sup>`1`</sup>
- [x] `170521>170521` Fix schedule list natures - THE RETURN

## 3.0.0.5 :ant: <sup>`6`</sup>
- [x] `170501>170501` Fix e-mail urls when the e-mail is sent by API
- [x] `170501>170501` Fiz uncheck/check url at report
- [x] `170418>170418` Fix move single value edit at mobile
- [x] `170418>170418` Fix schedule list with categories disabled
- [x] `170418>170418` Fix schedule list natures
- [x] `170418>170418` Fix balance box size at mobile browser

## 3.0.0.4 :ant: <sup>`1`</sup>
- [x] `170418>170418` Fix schedule list (shows all the schedules of the system)

## 3.0.0.3 :ant: <sup>`1`</sup>
- [x] `170411>170411` Fix icon missing in the apps list

## 3.0.0.2 :ant: <sup>`7`</sup>
- [x] `170411>170411` Fix logged out 404
- [x] `170411>170411` New known 404 redirects
- [x] `170411>170411` Fix edit move the second time at mobile
- [x] `170410>170411` Reduce move form size at mobile
- [x] `170410>170410` Make red, green and blue fonts more visible mobile
- [x] `170410>170410` Fix of manifest warnings
- [x] `170410>170410` Shorten list app name

## 3.0.0.1 :ant: <sup>`11`</sup>
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

## 3.0.0.0 :dragon: <sup>`22`</sup>
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

## 2.1.4.4 :ant: <sup>`1`</sup>
- [x] `170214>170215` Fix account / categories list when one more is created

## 2.1.4.3 :ant: <sup>`1`</sup>
- [x] `170209>170209` Fix NullReference

## 2.1.4.2 :ant: <sup>`1`</sup>
- [x] `170208>170208` Favicon and robots do not use session

## 2.1.4.1 :ant: <sup>`2`</sup>
- [x] `170208>170208` Fix android licensing
- [x] `170208>170208` Fix browser cookie

## 2.1.4.0 :sheep: <sup>`5`</sup>
- [x] `170207>170207` Making log at elmah in case of general fail / elmah test page
- [x] `170130>170130` SharedPreferences to store login / language
- [x] `170127>170128` Use Nuget packages
- [x] `170127>170127` Change apache by Volley
- [x] `170127>170127` Change licensing

## 2.1.3.4 :ant: <sup>`1`</sup>
- [x] `170127>170127` Fix duplication os details

## 2.1.3.3 :ant: <sup>`1`</sup>
- [x] `170115>170122` Adjust android layouts to use new format

## 2.1.3.2 :ant: <sup>`3`</sup>
- [x] `170117>170117` Adjust of number at move creation
- [x] `170117>170117` Adjust site (again)
- [x] `170117>170117` Adjust multilanguage

## 2.1.3.1 :ant: <sup>`1`</sup>
- [x] `170117>170117` Fix site address (published with error)

## 2.1.3.0 :sheep: <sup>`1`</sup>
- [x] `170115>170115` Hide column of check move (web/mobile)

## 2.1.2.0 :sheep: <sup>`2`</sup>
- [x] `170115>170115` Fix problem with out/in move
- [x] `170114>170115` Adjust NH to new forms

## 2.1.1.0 :sheep: <sup>`3`</sup>
- [x] `170114>170114` Remove front validation (date problem)
- [x] `170113>170114` Make e-mail change
- [x] `170113>170114` Make password change

## 2.1.0.0 :whale: <sup>`7`</sup>
- [x] `170113>170113` Fix e-mail urls
- [x] `170113>170113` Config to user enable move check - mobile
- [x] `170112>170113` Config to user enable move check - site
- [x] `160728>170112` Make check move - mobile
- [x] `160718>160718` Make check move - site
- [x] `160523>160524` Put account combos to enable change any account (out/in/transfer) - mobile
- [x] `160523>160523` Put account combos to enable change any account (out/in/transfer) - site

## 2.0.3.1 :ant: <sup>`9`</sup>
- [x] `160515>160515` Fix duplication when request reports (mobile)
- [x] `160515>160515` Show installment at mobile
- [x] `160515>160515` Leave Amazon sandbox
- [x] `160509>160515` Fix e-mail send by Amazon
- [x] `150919>160509` Config change tests
- [x] `150915>150917` Fix app at Lollipop
- [x] `150910>150910` Fix problem on open move creation (mobile)
- [x] `150910>150910` Fix delete account when it had moves
- [x] `150910>150910` Use colors at number on schedule screen

## 2.0.3.0 :sheep: <sup>`5`</sup>
- [x] `150907>150907` Let save the program at SD card
- [x] `150906>150907` Add move edit (mobile)
- [x] `150906>150906` Do not change orientation when its saving or deleting a move
- [x] `150906>150906` Do not let sleep when its waiting a request response
- [x] `150905>150905` Add close button (mobile)

## 2.0.2.0 :sheep: <sup>`13`</sup>
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

## 2.0.1.2 :ant: <sup>`2`</sup>
- [x] `150111>150111` Fix closed account list
- [x] `150108>150108` Fix move / schedule creation screen - value problem

## 2.0.1.1 :ant: <sup>`5`</sup>
- [x] `150101>150103` Fix test errors
- [x] `150103>150103` Change YellowLimit and RedLimit to Cents
- [x] `150101>150103` Fix error on mobile detailed move
- [x] `141230>141231` Fix move error with decimal value
- [x] `141230>141230` Fix login redirect bug

## 2.0.1.0 :sheep: <sup>`9`</sup>
- [x] `141221>141225` Put move delete (mobile)
- [x] `141207>141207` Extract: make year combo change if month combo restart
- [x] `141207>141207` Find TO-DOs at project
- [x] `141207>141207` Refactor at controllers and areas, inverse plural
- [x] `141207>141207` Change Extensions Sum to Total
- [x] `141206>141207` At Move, put Value as recorded field and use Total to show
- [x] `141201>141201` Put integer values at DB (double > int)
- [x] `141129>141129` Create unit test of deleting all the moves of a schedule
- [x] `141129>141129` Create unit test of schedule half past / half future

## 2.0.0.3 :ant: <sup>`12`</sup>
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

## 2.0.0.2 :ant: <sup>`10`</sup>
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

## 2.0.0.1 :ant: <sup>`1`</sup>
- [x] `140601>140602` Adjust move data loosing when orientation changes

## 2.0.0.0 :dragon: <sup>`71`</sup>
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
- [x] `140303>140303` Put [ok] at error alert (Mobile)
- [x] `140303>140303` Change [search by name] by [search by url] at account
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
- [x] `130928>131002` Review layout [(goodui)](http://goodui.org/)
- [x] `130902>130902` Create Json area

## 1.1.0.0 :whale: <sup>`5`</sup>
- [x] `130928>130928` Stop datepicker from filling today at edit
- [x] `130928>130928` Fix datepicker at chrome
- [x] `130927>130927` Put Enter / Logoff as buttons at top menu (external)
- [x] `130927>130927` Put Categories / Accounts / Logoff as buttons at top menu (internal)
- [x] `130927>130927` Remove loggedin user top menu

## 1.0.4.0 :sheep: <sup>`2`</sup>
- [x] `130926>130927` Fix detailed move creation
- [x] `130926>130926` Put dot at thousand

## 1.0.3.1 :ant: <sup>`2`</sup>
- [x] `130830>130830` [Fix this](http://www.dontflymoney.com/Token/Received)
- [x] `130824>130830` Move Controllers logic to Models

## 1.0.3.0 :sheep: <sup>`18`</sup>
- [x] `130818>130822` Add Broken to summary, and use RobotService to fix
- [x] `130809>130822` Solve update Summaries problem when something change the value
- [x] `130806>130809` Use just one cookie to get everything from server
- [x] `130805>130806` Add link to login screen at User Verification screen
- [x] `130802>130802` Split Controller Move in Move / Schedule / Detail
- [x] `130722>130801` Review Schedule to implement IMove / kill FutureMove
- [x] `130717>130730` Fix tests
- [x] `130716>130716` Replace Select by Get
- [x] `130715>130716` Create SessionOld to use at GetByIdOld
- [x] `130715>130715` [put Index](http://www.dontflymoney.com/Token/)
- [x] `130715>130715` Dic [(1)](http://dontflymoney.com/?dgcr=1) / [(2)](http://dontflymoney.com/?author=1)
- [x] `130324>130324` Suggest account url
- [x] `130324>130324` Fix NullReference when access non-existent [account url](http://www.dontflymoney.com/Accounts/19/Report/ShowMoves)
- [x] `130318>130324` Use Identity (Generic) to store logged user
- [x] `130318>130318` Remove Authentication setter return
- [x] `130317>130317` Make ticket logout
- [x] `130316>130317` Replace SelectByEmail of User by SelectByTicket
- [x] `130311>130314` Make ValidateAndGet get Ticket String

## 1.0.2.3 :ant: <sup>`17`</sup>
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

## 1.0.2.2 :ant: <sup>`22`</sup>
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

## 1.0.2.1 :ant: <sup>`2`</sup>
- [x] `120321>120322` Send error details on report e-mail
- [x] `120320>120320` Refactor Resharper

## 1.0.2.0 :sheep: <sup>`13`</sup>
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

## 1.0.1.0 :sheep: <sup>`5`</sup>
- [x] `111125>111130` Tests and Publish
- [x] `111025>111025` Error handling
- [x] `111025>111025` Protection for Elmah log
- [x] `111024>111025` Try to pass Log4Net / Elmah to MySql: FAIL
- [x] `111019>111019` Log4Net / Elmah

## 1.0.0.4 :ant: <sup>`6`</sup>
- [x] `111111>111111` Enable ZERO for red and yellow signs
- [x] `111109>111109` Verify if password recover disable user
- [x] `111109>111122` Send e-mail on move creation (user will be able to disable in the future)
- [x] `111109>111109` Replace links by icons at categories
- [x] `111108>111108` Alphabetical order at accounts (menu, opened, closed) and categories
- [x] `111023>111023` Fix error on transfer move: was not getting accounts

## 1.0.0.3 :ant: <sup>`4`</sup>
- [x] `111013>111014` Fix error with move creation (foreign key fail)
- [x] `111011>111013` Use icons instead of words at accounts list
- [x] `111010>111010` Validate user by e-mail - hashcode (WARN ABOUT NO LINK TO FINANCIAL INSTITUTIONS)
- [x] `111002>111009` Password recover - hashcode

## 1.0.0.2 :ant: <sup>`5`</sup>
- [x] `111001>111001` Put BETA at site logo
- [x] `111001>111001` Make error message darker (inverse color?)
- [x] `111001>111001` User register date
- [x] `110928>111001` Password confirm
- [x] `110925>110926` Date format conflict

## 1.0.0.1 :ant: <sup>`2`</sup>
- [x] `110924>110924` Move, when change In>Out or Out>In, was showing error about the other account
- [x] `110924>110924` Fix NH access at publish site

## 1.0.0.0 :dragon: <sup>`52`</sup>
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
