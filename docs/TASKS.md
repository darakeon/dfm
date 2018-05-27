<img src="../site/DFM.MVC/Assets/Images/pig.svg" width="85" align="left"/>

# Task list

This is the task list for the project. Done, Doing, To-do, all here, all planned. =)

- [go to published version](#prod)
- [go to version in development](#dev)
- [dev version state](../../4.1.2.2/docs/TASKS.md#dev)

Legend:
- :dragon:: a huge change in system
- :whale:: big changes, like a new feature
- :sheep:: little changes, as a change in an existing feature
- :ant:: the developer is just fixing some sh*t it did

## 8.0.0.0 :dragon: <sup>`2`</sup>
- [ ] Link unsubscribe
- [ ] Run robot each hour and remove it from request (GO?)

## 7.0.0.0 :dragon: <sup>`2`</sup>
- [ ] Create service to translate site ("put in your language")
- [ ] Use .NET resources

## 6.0.0.0 :dragon: <sup>`1`</sup>
- [ ] OCR to add values to system

## 5.5.0.0 :whale: <sup>`5`</sup>
- [ ] E-mail send mock
- [ ] "Guess" the category
- [ ] report with more than one account
- [ ] year report by category [(idea)](http://www.chartjs.org/)
- [ ] month report by category [(idea)](http://www.chartjs.org/)

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
- [ ] Delete not accessed accounts
- [ ] Last login date
- [ ] Data export
- [ ] Data import

## 5.2.0.0 :whale: <sup>`4`</sup>
- [ ] Enable reopen account
- [ ] Config max request length
- [ ] Show future moves and foreseen balance
- [ ] Add config for first day of account

## 5.1.0.0 :whale: <sup>`1`</sup>
- [ ] Create Market - group of moves, each one of one category, to when you are shopping

## 5.0.0.0 :dragon: <sup>`1`</sup>
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

## 4.3.1.0 :sheep: <sup>`4`</sup>
- [ ] If session drop, call history.go(-2) after re-login
- [ ] Update SpecFlow version
- [ ] Remove direct access of Month and Year to Summary (refactor Summary)
- [ ] Remove GetOrCreate

## 4.3.0.1 :ant: <sup>`3`</sup>
- [ ] Make web use fakeId, not id
- [ ] Handle DFMCoreException at Robot
- [ ] Change fields to use the ones firefox and chrome customize

## 4.3.0.0 :whale: <sup>`4`</sup>
- [ ] Put info that you can disabled mobile login if your cellphone is stolen
- [ ] Adjust tab order at move screen (mobile)
- [ ] Add account creation (mobile)
- [ ] Add category creation (mobile)

## 4.2.0.0 :whale: <sup>`4`</sup>
- [ ] Allow to hide values at start screen (mobile)
- [ ] Change date to field with datepicker button (mobile)
- [ ] Change category to autocomplete (mobile)
- [ ] Move datetime spinner to the side of the button (remove modal)

## 4.1.4.0 :sheep: <sup>`6`</sup>
- [ ] Allow check all the moves before a date
- [ ] Enable to check moves on closed accounts
- [ ] Check move without reload screen - web
- [ ] Check move without reload screen - mobile
- [ ] Edit must uncheck move (warn before and after edit)
- [ ] Split move check to account in / out

## 4.1.3.2 :ant: <sup>`2`</sup>
- [ ] Make android automated tests
- [ ] Make selenium automated tests - web

## 4.1.3.1 :ant: <sup>`4`</sup>
- [ ] Transform service receiving / returns into DTO
- [ ] IDs Int64
- [ ] Put lock at NH get session
- [ ] Fix transaction call to avoid code duplication (Action/Func<>)

## 4.1.3.0 :sheep: <sup>`4`</sup>
- [ ] This site uses cookies to store your preferences for site-specific language and display options.
- [ ] Fix detailed sum cents at e-mail
- [ ] Tests category delete with children deleted
- [ ] Tests for user permission

## <a name="dev"></a>4.1.2.3 :ant: <sup>`6`</sup>
- [ ] Make/find app to verify DFM status
- [ ] AntiForgery
- [ ] Add IsAdm to User
- [ ] Avoid weird layout on changing theme
- [ ] Kill all java click listeners
- [ ] Make slide menu, not bottom anymore

## <a name="dev"></a>4.1.2.2 :ant: <sup>`4`</sup>
- [ ] Encrypt sharedPreferences (and logoff all mobile)
- [ ] Remove ticket from url (mobile)
- [ ] `2018-05-26 > ...` Kill state objects (mobile)
- [ ] `2018-05-26 > ...` Replace volley by retrofit

## <a name="prod"></a>4.1.2.1 :ant: <sup>`3`</sup>
- [x] `2018-03-07 > 2018-03-07` Fix category repeated block at mobile/moves
- [x] `2018-03-07 > 2018-03-07` Fix nature issue on create at mobile/moves
- [x] `2018-03-06 > 2018-03-06` Fix add detail at mobile/moves

## 4.1.2.0 :sheep: <sup>`6`</sup>
- [x] `2018-03-02 > 2018-03-02` Remove license permission (not used anymore)
- [x] `2018-02-25 > 2018-02-25` Update Volley
- [x] `2018-02-26 > 2018-02-26` Remove findViewById
- [x] `2018-01-27 > 2018-02-26` Implement 2FA
- [x] `2018-01-19 > 2018-01-22` Create option of starting with wizard at sign up
- [x] `2018-01-18 > 2018-01-18` Add warning on move with category editing when categories are disabled

## 4.1.1.3 :ant: <sup>`3`</sup>
- [x] `2018-01-15 > 2018-01-15` Fix mobile to edit move with category when categories use are disabled
- [x] `2018-01-16 > 2018-01-16` Put list of feature on screen
- [x] `2018-01-14 > 2018-01-14` Fix button name on "You are offline"

## 4.1.1.2 :ant: <sup>`6`</sup>
- [x] `2018-01-07 > 2018-01-07` Fix date combo start value (mobile)
- [x] `2018-01-07 > 2018-01-07` Offer send e-mail on error (mobile)
- [x] `2018-01-07 > 2018-01-07` Fix move add from home (mobile)
- [x] `2018-01-05 > 2018-01-07` Fix month future year report
- [x] `2018-01-05 > 2018-01-05` Fix account list on transfer
- [x] `2018-01-05 > 2018-01-05` Fix task list duplication and already done

## 4.1.1.1 :ant: <sup>`2`</sup>
- [x] `2017-12-31 > 2017-12-31` Fix facebook image
- [x] `2017-12-31 > 2017-12-31` Fix android pig redirection

## 4.1.1.0 :sheep: <sup>`4`</sup>
- [x] `2017-12-29 > 2017-12-30` Mobile home link goes to app if it exists
- [x] `2017-10-22 > 2017-10-22` Fix show error at mobile
- [x] `2017-10-22 > 2017-10-22` Refactor android dialog handling
- [x] `2017-10-19 > 2017-10-19` Refactor android post/get handling

## 4.1.0.1 :ant: <sup>`2`</sup>
- [x] `2017-10-18 > 2017-10-18` Fix error when enter cents on mobile
- [x] `2017-10-08 > 2017-10-18` Refactor android activity helper classes

## 4.1.0.0 :whale: <sup>`3`</sup>
- [x] `2017-10-07 > 2017-10-08` Put titles on glyphicons
- [x] `2017-10-07 > 2017-10-07` Fix places where the understanding depends on colors
- [x] `2017-10-02 > 2017-10-03` Fix account stati icons - should not be all the same

## 4.0.2.0 :sheep: <sup>`5`</sup>
- [x] `2017-10-02 > 2017-10-02` Add package for JS libraries
- [x] `2017-09-24 > 2017-10-07` Upgrade to Android Studio 3.0
- [x] `2017-09-24 > 2017-10-07` Upgrade to VS 2017
- [x] `2017-09-24 > 2017-09-24` Add link to github Issues
- [x] `2017-09-24 > 2017-09-24` Add to github README the libraries used

## 4.0.1.3 :ant: <sup>`1`</sup>
- [x] `2017-09-27 > 2017-09-27` Fix THE NEW communication bug

## 4.0.1.2 :ant: <sup>`1`</sup>
- [x] `2017-09-24 > 2017-09-24` Fix mobile site communication

## 4.0.1.1 :ant: <sup>`7`</sup>
- [x] `2017-09-23 > 2017-09-23` Change password should drop logins
- [x] `2017-09-23 > 2017-09-23` Verify create / edit / schedule transfer move (browser)
- [x] `2017-09-19 > 2017-09-20` Contract acceptance warning at android
- [x] `2017-09-10 > 2017-09-10` Remove license check from android
- [x] `2017-09-16 > 2017-09-16` Fix delete empty account with detailed schedule
- [x] `2017-09-16 > 2017-09-16` Tests for delete empty account with detailed schedule
- [x] `2017-09-10 > 2017-09-10` Warning that you need to create categories on site to make moves

## 4.0.1.0 :sheep: <sup>`8`</sup>
- [x] `2017-09-09 > 2017-09-09` Make tasks file as MD
- [x] `2017-09-09 > 2017-09-09` Update twitter with recent versions
- [x] `2017-09-09 > 2017-09-09` Translate tasks file to english
- [x] `2017-09-09 > 2017-09-09` Make SVG pig
- [x] `2017-09-06 > 2017-09-06` Organize files used to manage project
- [x] `2017-09-06 > 2017-09-08` Create license file
- [x] `2017-09-06 > 2017-09-06` Make collaborate docs
- [x] `2017-09-06 > 2017-09-06` Config gitcop

## 4.0.0.0 :dragon: <sup>`4`</sup>
- [x] `2017-09-05 > 2017-09-05` Change password encryption
- [x] `2017-09-04 > 2017-09-05` Redirect known 404 to HELP
- [x] `2017-09-04 > 2017-09-04` Add Open Source to Contract
- [x] `2017-07-01 > 2017-09-04` Refactor commits to GIT

## 3.0.0.7 :ant: <sup>`1`</sup>
- [x] `2017-06-04 > 2017-08-26` Fix let's encrypt

## 3.0.0.6 :ant: <sup>`1`</sup>
- [x] `2017-05-21 > 2017-05-21` Fix schedule list natures - THE RETURN

## 3.0.0.5 :ant: <sup>`6`</sup>
- [x] `2017-05-01 > 2017-05-01` Fix e-mail urls when the e-mail is sent by API
- [x] `2017-05-01 > 2017-05-01` Fiz uncheck/check url at report
- [x] `2017-04-18 > 2017-04-18` Fix move single value edit at mobile
- [x] `2017-04-18 > 2017-04-18` Fix schedule list with categories disabled
- [x] `2017-04-18 > 2017-04-18` Fix schedule list natures
- [x] `2017-04-18 > 2017-04-18` Fix balance box size at mobile browser

## 3.0.0.4 :ant: <sup>`1`</sup>
- [x] `2017-04-18 > 2017-04-18` Fix schedule list (shows all the schedules of the system)

## 3.0.0.3 :ant: <sup>`1`</sup>
- [x] `2017-04-11 > 2017-04-11` Fix icon missing in the apps list

## 3.0.0.2 :ant: <sup>`7`</sup>
- [x] `2017-04-11 > 2017-04-11` Fix logged out 404
- [x] `2017-04-11 > 2017-04-11` New known 404 redirects
- [x] `2017-04-11 > 2017-04-11` Fix edit move the second time at mobile
- [x] `2017-04-10 > 2017-04-11` Reduce move form size at mobile
- [x] `2017-04-10 > 2017-04-10` Make red, green and blue fonts more visible mobile
- [x] `2017-04-10 > 2017-04-10` Fix of manifest warnings
- [x] `2017-04-10 > 2017-04-10` Shorten list app name

## 3.0.0.1 :ant: <sup>`11`</sup>
- [x] `2017-04-08 > 2017-04-08` Put date and category at same line on create/edit at mobile
- [x] `2017-04-08 > 2017-04-08` "Cancel" should not reload the page when cannot verify license
- [x] `2017-04-08 > 2017-04-08` Fix extract landscape layout
- [x] `2017-04-08 > 2017-04-08` Solve OutOfMemoryError when load welcome image
- [x] `2017-04-08 > 2017-04-08` Check android read state permission
- [x] `2017-04-06 > 2017-04-06` Fix mobile logo
- [x] `2017-04-06 > 2017-04-06` Highlight signup
- [x] `2017-04-06 > 2017-04-06` Fix move details (table) at e-mail
- [x] `2017-04-05 > 2017-04-06` Fix e-mail colors
- [x] `2017-04-05 > 2017-04-05` Fix empty account in / out at e-mail
- [x] `2017-04-05 > 2017-04-05` Shorten e-mail title

## 3.0.0.0 :dragon: <sup>`22`</sup>
- [x] `2017-04-01 > 2017-04-02` Pig video
- [x] `2017-03-29 > 2017-03-29` Change top menu by bottom menu (mobile)
- [x] `2017-03-29 > 2017-03-31` Remove blue from dark layout
- [x] `2017-03-29 > 2017-03-29` Fix icon list / menu mobile
- [x] `2017-03-28 > 2017-03-29` Fix move duplication (mobile)
- [x] `2017-03-27 > 2017-03-28` Android dark theme
- [x] `2017-03-27 > 2017-03-27` Fix cache on create/edit move (account/category) - and other crazy caches
- [x] `2017-03-27 > 2017-03-27` Fix move with schedule edition
- [x] `2017-03-26 > 2017-03-27` Change e-mail layouts - light / dark
- [x] `2017-03-25 > 2017-03-26` Theme config
- [x] `2017-03-24 > 2017-03-25` Add cookies warning at logon (remember me)
- [x] `2017-03-24 > 2017-03-24` Make new cover for youtube / facebook / twitter (?)
- [x] `2017-03-21 > 2017-03-24` Implement mandatory Term sign
- [x] `2017-03-20 > 2017-03-22` Write Terms of Use
- [x] `2017-03-19 > 2017-03-19` Allow negative account limit
- [x] `2017-03-19 > 2017-03-19` Change datepicker
- [x] `2017-03-01 > 2017-01-19` Layout Bootstrap
- [x] `2017-03-03 > 2017-03-05` Fix remember at login
- [x] `2017-02-24 > 2017-02-25` Fix android packages
- [x] `2017-02-23 > 2017-02-23` Add android start image
- [x] `2017-02-23 > 2017-02-23` Implement Admin/Robot GetList tests 
- [x] `2017-02-09 > 2017-02-22` Android Kotlin

## 2.1.4.4 :ant: <sup>`1`</sup>
- [x] `2017-02-14 > 2017-02-15` Fix account / categories list when one more is created

## 2.1.4.3 :ant: <sup>`1`</sup>
- [x] `2017-02-09 > 2017-02-09` Fix NullReference

## 2.1.4.2 :ant: <sup>`1`</sup>
- [x] `2017-02-08 > 2017-02-08` Favicon and robots do not use session

## 2.1.4.1 :ant: <sup>`2`</sup>
- [x] `2017-02-08 > 2017-02-08` Fix android licensing
- [x] `2017-02-08 > 2017-02-08` Fix browser cookie

## 2.1.4.0 :sheep: <sup>`5`</sup>
- [x] `2017-02-07 > 2017-02-07` Making log at elmah in case of general fail / elmah test page
- [x] `2017-01-30 > 2017-01-30` SharedPreferences to store login / language
- [x] `2017-01-27 > 2017-01-28` Use Nuget packages
- [x] `2017-01-27 > 2017-01-27` Change apache by Volley
- [x] `2017-01-27 > 2017-01-27` Change licensing

## 2.1.3.4 :ant: <sup>`1`</sup>
- [x] `2017-01-27 > 2017-01-27` Fix duplication os details

## 2.1.3.3 :ant: <sup>`1`</sup>
- [x] `2017-01-15 > 2017-01-22` Adjust android layouts to use new format

## 2.1.3.2 :ant: <sup>`3`</sup>
- [x] `2017-01-17 > 2017-01-17` Adjust of number at move creation
- [x] `2017-01-17 > 2017-01-17` Adjust site (again)
- [x] `2017-01-17 > 2017-01-17` Adjust multilanguage

## 2.1.3.1 :ant: <sup>`1`</sup>
- [x] `2017-01-17 > 2017-01-17` Fix site address (published with error)

## 2.1.3.0 :sheep: <sup>`1`</sup>
- [x] `2017-01-15 > 2017-01-15` Hide column of check move (web/mobile)

## 2.1.2.0 :sheep: <sup>`2`</sup>
- [x] `2017-01-15 > 2017-01-15` Fix problem with out/in move
- [x] `2017-01-14 > 2017-01-15` Adjust NH to new forms

## 2.1.1.0 :sheep: <sup>`3`</sup>
- [x] `2017-01-14 > 2017-01-14` Remove front validation (date problem)
- [x] `2017-01-13 > 2017-01-14` Make e-mail change
- [x] `2017-01-13 > 2017-01-14` Make password change

## 2.1.0.0 :whale: <sup>`7`</sup>
- [x] `2017-01-13 > 2017-01-13` Fix e-mail urls
- [x] `2017-01-13 > 2017-01-13` Config to user enable move check - mobile
- [x] `2017-01-12 > 2017-01-13` Config to user enable move check - site
- [x] `2016-07-28 > 2017-01-12` Make check move - mobile
- [x] `2016-07-18 > 2016-07-18` Make check move - site
- [x] `2016-05-23 > 2016-05-24` Put account combos to enable change any account (out/in/transfer) - mobile
- [x] `2016-05-23 > 2016-05-23` Put account combos to enable change any account (out/in/transfer) - site

## 2.0.3.1 :ant: <sup>`9`</sup>
- [x] `2016-05-15 > 2016-05-15` Fix duplication when request reports (mobile)
- [x] `2016-05-15 > 2016-05-15` Show installment at mobile
- [x] `2016-05-15 > 2016-05-15` Leave Amazon sandbox
- [x] `2016-05-09 > 2016-05-15` Fix e-mail send by Amazon
- [x] `2015-09-19 > 2016-05-09` Config change tests
- [x] `2015-09-15 > 2015-09-17` Fix app at Lollipop
- [x] `2015-09-10 > 2015-09-10` Fix problem on open move creation (mobile)
- [x] `2015-09-10 > 2015-09-10` Fix delete account when it had moves
- [x] `2015-09-10 > 2015-09-10` Use colors at number on schedule screen

## 2.0.3.0 :sheep: <sup>`5`</sup>
- [x] `2015-09-07 > 2015-09-07` Let save the program at SD card
- [x] `2015-09-06 > 2015-09-07` Add move edit (mobile)
- [x] `2015-09-06 > 2015-09-06` Do not change orientation when its saving or deleting a move
- [x] `2015-09-06 > 2015-09-06` Do not let sleep when its waiting a request response
- [x] `2015-09-05 > 2015-09-05` Add close button (mobile)

## 2.0.2.0 :sheep: <sup>`13`</sup>
- [x] `2015-09-02 > 2015-09-03` Put project version at .NET
- [x] `2015-09-02 > 2015-09-02` Fix logo url
- [x] `2015-08-31 > 2015-08-31` Fix tablet error (no 3G)
- [x] `2015-08-30 > 2015-08-30` Migrate to android studio
- [x] `2015-02-14 > 2015-09-02` Put CANCEL button at Settings and Move (mobile)
- [x] `2015-02-14 > 2015-02-14` Put in and out at year report
- [x] `2015-02-14 > 2015-02-14` Put URLReferrer at 404 report
- [x] `2015-02-14 > 2015-02-14` Remove parent > child cascade from NH
- [x] `2015-02-14 > 2015-02-14` Fix e-mail and robots urls
- [x] `2015-01-11 > 2015-01-11` Review empty table layout
- [x] `2015-01-11 > 2015-01-11` Increase android version accordly with DFM downloads
- [x] `2015-01-11 > 2015-01-11` Fix menu click (right and middle mouse click)
- [x] `2015-01-11 > 2015-01-11` Handle logoff in the middle of ajax category creation

## 2.0.1.2 :ant: <sup>`2`</sup>
- [x] `2015-01-11 > 2015-01-11` Fix closed account list
- [x] `2015-01-08 > 2015-01-08` Fix move / schedule creation screen - value problem

## 2.0.1.1 :ant: <sup>`5`</sup>
- [x] `2015-01-01 > 2015-01-03` Fix test errors
- [x] `2015-01-03 > 2015-01-03` Change YellowLimit and RedLimit to Cents
- [x] `2015-01-01 > 2015-01-03` Fix error on mobile detailed move
- [x] `2014-12-30 > 2014-12-31` Fix move error with decimal value
- [x] `2014-12-30 > 2014-12-30` Fix login redirect bug

## 2.0.1.0 :sheep: <sup>`9`</sup>
- [x] `2014-12-21 > 2014-12-25` Put move delete (mobile)
- [x] `2014-12-07 > 2014-12-07` Extract: make year combo change if month combo restart
- [x] `2014-12-07 > 2014-12-07` Find TO-DOs at project
- [x] `2014-12-07 > 2014-12-07` Refactor at controllers and areas, inverse plural
- [x] `2014-12-07 > 2014-12-07` Change Extensions Sum to Total
- [x] `2014-12-06 > 2014-12-07` At Move, put Value as recorded field and use Total to show
- [x] `2014-12-01 > 2014-12-01` Put integer values at DB (double > int)
- [x] `2014-11-29 > 2014-11-29` Create unit test of deleting all the moves of a schedule
- [x] `2014-11-29 > 2014-11-29` Create unit test of schedule half past / half future

## 2.0.0.3 :ant: <sup>`12`</sup>
- [x] `2014-10-20 > 2014-10-21` Fix apk for kitkat
- [x] `2014-07-25 > 2014-07-25` Delete schedule of deleted account
- [x] `2014-07-25 > 2014-07-25` Lock schedule of closed account
- [x] `2014-07-06 > 2014-07-06` Security disable the older securities
- [x] `2014-07-06 > 2014-07-06` Deactivate user if it use wrong password 5 times
- [x] `2014-07-06 > 2014-07-06` Check if password reset disables user
- [x] `2014-07-06 > 2014-07-06` Handle error at e-mail send (Security)
- [x] `2014-07-06 > 2014-07-06` Fix error when requesting report that doesn't have Month/Year
- [x] `2014-07-05 > 2014-07-06` Move generic itens do Ak
- [x] `2014-06-29 > 2014-06-29` Make move without category fix its summaries
- [x] `2014-06-29 > 2014-06-29` Fix tests of e-mail error
- [x] `2014-06-26 > 2014-06-26` Fix detailed schedule

## 2.0.0.2 :ant: <sup>`10`</sup>
- [x] `2014-06-20 > 2014-06-20` Put version at footer
- [x] `2014-06-20 > 2014-06-20` Put logon link at password reset
- [x] `2014-06-18 > 2014-06-19` Stop request at activity destroy
- [x] `2014-06-11 > 2014-06-17` Update project: MVC 5
- [x] `2014-06-09 > 2014-06-09` Update project: .NET 4.5
- [x] `2014-06-09 > 2014-06-09` Update project: VS 2013
- [x] `2014-06-09 > 2014-06-09` Fix transform for rewrite
- [x] `2014-06-05 > 2014-06-16` Review rotate logic
- [x] `2014-06-04 > 2014-06-05` Handle differentely EConnReset
- [x] `2014-06-03 > 2014-06-04` Stop user google license is denied

## 2.0.0.1 :ant: <sup>`1`</sup>
- [x] `2014-06-01 > 2014-06-02` Adjust move data loosing when orientation changes

## 2.0.0.0 :dragon: <sup>`71`</sup>
- [x] `2014-05-31 > 2014-05-31` Force HTTPS access
- [x] `2014-05-31 > 2014-05-31` Remove BETA
- [x] `2014-05-28 > 2014-05-28` Put play link at site
- [x] `2014-05-25 > 2014-05-31` Create product at google play
- [x] `2014-05-25 > 2014-05-28` HTTPS!!!
- [x] `2014-05-19 > 2014-05-19` Orientation change at move must set fields functions
- [x] `2014-05-19 > 2014-05-19` Orientation change at reports must keep time filter
- [x] `2014-05-19 > 2014-05-19` Scroll at move details
- [x] `2014-05-19 > 2014-05-19` Fix mobile extract month and year do extract
- [x] `2014-05-19 > 2014-05-19` Fix transfer move edit
- [x] `2014-05-18 > 2014-05-19` Review intro text
- [x] `2014-05-12 > 2014-05-16` Remove Release from versioning, change db name, user and password
- [x] `2014-05-11 > 2014-05-11` Remove post data if it's passwords or values
- [x] `2014-05-10 > 2014-05-11` Fix server slowness
- [x] `2014-05-04 > 2014-05-04` Delete PageLog
- [x] `2014-05-04 > 2014-05-04` Fix language saving
- [x] `2014-05-04 > 2014-05-04` Fix table for too big texts
- [x] `2014-05-04 > 2014-05-04` Show just 2 digits as cents (mobile)
- [x] `2014-05-04 > 2014-05-04` Change language before screen render
- [x] `2014-05-04 > 2014-05-04` Record language locally
- [x] `2014-05-04 > 2014-05-04` Fix logoff (M)
- [x] `2014-04-18 > 2014-04-18` Make activity do not call the pig if it's just orientation change
- [x] `2014-04-18 > 2014-04-18` Fix nature translation
- [x] `2014-04-18 > 2014-04-18` Make zero show up as blue
- [x] `2014-04-18 > 2014-04-18` Fix e-mail layout links
- [x] `2014-04-14 > 2014-04-14` Fix when turn device
- [x] `2014-04-14 > 2014-04-14` List logins to logoff
- [x] `2014-04-14 > 2014-04-14` Put user default language as browser language
- [x] `2014-04-14 > 2014-04-14` Deny open dfm at iframe / X-frame-options
- [x] `2014-04-14 > 2014-04-14` List schedule / allow stop schedule
- [x] `2014-04-14 > 2014-04-14` Schedule limite label / Detailed label
- [x] `2014-04-14 > 2014-04-14` Show move date at landscape
- [x] `2014-04-08 > 2014-04-14` Change mobile language to user language
- [x] `2014-04-07 > 2014-04-07` Create config screen to user (Web)
- [x] `2014-04-07 > 2014-04-07` Redirect, after create move, to extract of that move
- [x] `2014-03-31 > 2014-03-31` Order Moves by ID too
- [x] `2014-03-31 > 2014-03-31` Fix amount default at move screen
- [x] `2014-03-23 > 2014-03-24` Put current balance at Extract
- [x] `2014-03-23 > 2014-03-23` Put categories / account combo list on alphabetical order
- [x] `2014-03-23 > 2014-03-23` Change Accounts button by Home image, Logoff button by Logoff image
- [x] `2014-03-23 > 2014-03-23` Fix session schema NH
- [x] `2014-03-23 > 2014-03-23` Fix tests from not changing summaries when there is no category
- [x] `2014-03-16 > 2014-03-23` Handle FailOnEmailSend at Robot / Move > record and warn that it doesn't sent the e-mail
- [x] `2014-03-05 > 2014-03-05` Create warning box
- [x] `2014-03-05 > 2014-03-05` Make config screen for user (Mobile)
- [x] `2014-03-05 > 2014-03-05` Hide category when user do not want categories
- [x] `2014-03-04 > 2014-03-05` Make possible to not choose category
- [x] `2014-03-04 > 2014-03-04` Separate User additional data (Config)
- [x] `2014-03-04 > 2014-03-04` Fix error at turning device at move creation screen (Mobile)
- [x] `2014-03-04 > 2014-03-04` Put amount default 1 (Mobile)
- [x] `2014-03-03 > 2014-03-03` Put [ok] at error alert (Mobile)
- [x] `2014-03-03 > 2014-03-03` Change [search by name] by [search by url] at account
- [x] `2014-02-25 > 2014-03-03` Make move recording (Mobile)
- [x] `2014-01-25 > 2014-02-23` Create screen to save move (Mobile)
- [x] `2014-01-25 > 2014-01-25` Create accounts summary (Mobile)
- [x] `2014-01-25 > 2014-01-25` Put scroll and loading
- [x] `2014-01-23 > 2014-01-23` Make assync site call
- [x] `2014-01-06 > 2014-01-06` Move strings to resource
- [x] `2014-01-01 > 2014-01-06` Create month report (Mobile)
- [x] `2013-12-30 > 2014-01-01` Put account list (Mobile)
- [x] `2013-12-28 > 2013-12-31` Remake app
- [x] `2013-11-08 > 2013-11-08` Fix move insertion date and time limit
- [x] `2013-10-04 > 2013-10-16` Make login by ticket at website
- [x] `2013-10-04 > 2013-10-04` Create App Mobile
- [x] `2013-09-04 > 2013-10-04` Decide about objects passing to external
- [x] `2013-10-03 > 2013-10-03` Fix bug when adding details to an already created move
- [x] `2013-10-03 > 2013-10-03` Fix detailed scheduling
- [x] `2013-10-02 > 2013-10-02` Hide Move and Schedule from closed accounts
- [x] `2013-10-02 > 2013-10-02` Change closed account to be same as opened, but without edit possibility
- [x] `2013-09-28 > 2013-10-02` Review layout [(goodui)](http://goodui.org/)
- [x] `2013-09-02 > 2013-09-02` Create Json area

## 1.1.0.0 :whale: <sup>`5`</sup>
- [x] `2013-09-28 > 2013-09-28` Stop datepicker from filling today at edit
- [x] `2013-09-28 > 2013-09-28` Fix datepicker at chrome
- [x] `2013-09-27 > 2013-09-27` Put Enter / Logoff as buttons at top menu (external)
- [x] `2013-09-27 > 2013-09-27` Put Categories / Accounts / Logoff as buttons at top menu (internal)
- [x] `2013-09-27 > 2013-09-27` Remove loggedin user top menu

## 1.0.4.0 :sheep: <sup>`2`</sup>
- [x] `2013-09-26 > 2013-09-27` Fix detailed move creation
- [x] `2013-09-26 > 2013-09-26` Put dot at thousand

## 1.0.3.1 :ant: <sup>`2`</sup>
- [x] `2013-08-30 > 2013-08-30` [Fix this](http://www.dontflymoney.com/Token/Received)
- [x] `2013-08-24 > 2013-08-30` Move Controllers logic to Models

## 1.0.3.0 :sheep: <sup>`18`</sup>
- [x] `2013-08-18 > 2013-08-22` Add Broken to summary, and use RobotService to fix
- [x] `2013-08-09 > 2013-08-22` Solve update Summaries problem when something change the value
- [x] `2013-08-06 > 2013-08-09` Use just one cookie to get everything from server
- [x] `2013-08-05 > 2013-08-06` Add link to login screen at User Verification screen
- [x] `2013-08-02 > 2013-08-02` Split Controller Move in Move / Schedule / Detail
- [x] `2013-07-22 > 2013-08-01` Review Schedule to implement IMove / kill FutureMove
- [x] `2013-07-17 > 2013-07-30` Fix tests
- [x] `2013-07-16 > 2013-07-16` Replace Select by Get
- [x] `2013-07-15 > 2013-07-16` Create SessionOld to use at GetByIdOld
- [x] `2013-07-15 > 2013-07-15` [put Index](http://www.dontflymoney.com/Token/)
- [x] `2013-07-15 > 2013-07-15` Dic [(1)](http://dontflymoney.com/?dgcr=1) / [(2)](http://dontflymoney.com/?author=1)
- [x] `2013-03-24 > 2013-03-24` Suggest account url
- [x] `2013-03-24 > 2013-03-24` Fix NullReference when access non-existent [account url](http://www.dontflymoney.com/Accounts/19/Report/ShowMoves)
- [x] `2013-03-18 > 2013-03-24` Use Identity (Generic) to store logged user
- [x] `2013-03-18 > 2013-03-18` Remove Authentication setter return
- [x] `2013-03-17 > 2013-03-17` Make ticket logout
- [x] `2013-03-16 > 2013-03-17` Replace SelectByEmail of User by SelectByTicket
- [x] `2013-03-11 > 2013-03-14` Make ValidateAndGet get Ticket String

## 1.0.2.3 :ant: <sup>`17`</sup>
- [x] `2013-03-07 > 2013-03-07` Protect hiddens edition, where its necessary
- [x] `2013-03-07 > 2013-03-07` Verify Move deletion with Schedule (ScheduleTimesCantBeZero)
- [x] `2013-03-07 > 2013-03-07` Do not send e-mail in case of error when edit move
- [x] `2013-03-07 > 2013-03-07` Protect object edit (user) at core
- [x] `2013-03-07 > 2013-03-07` Put numbers at enums (safer to DB)
- [x] `2013-03-03 > 2013-03-07` Use account url at MVC
- [x] `2013-03-03 > 2013-03-03` Add url choice to account
- [x] `2013-02-24 > 2013-03-03` Create Category / Account name edit (including mvc)
- [x] `2013-02-21 > 2013-02-22` Replace SelectById  of Account / Category by SelectByName
- [x] `2013-02-21 > 2013-02-22` Change places where any entity except move and detail are called by its ID (including MVC)
- [x] `2013-02-21 > 2013-02-21` Send Accounts / Category as name to save move
- [x] `2013-02-21 > 2013-02-21` Fix transfer e-mail
- [x] `2013-02-20 > 2013-02-21` User protection at Core (verify active too)
- [x] `2013-02-20 > 2013-02-20` Isolate authentication project
- [x] `2012-10-17 > 2013-02-20` Fix MVC to send category and schedule separated from move
- [x] `2012-10-17 > 2012-10-17` Put visit log (IP, Date/Time, Address)
- [x] `2012-10-17 > 2012-10-17` Fix e-mail config at web.config

## 1.0.2.2 :ant: <sup>`22`</sup>
- [x] `2012-08-20 > 2012-08-21` Multilanguage tests
- [x] `2012-08-20 > 2012-08-20` Test close account and  disable category (MVC)
- [x] `2012-08-17 > 2012-08-17` Add SMTP to web.config
- [x] `2012-08-16 > 2012-08-16` Test enter with invalid user (MVC)
- [x] `2012-08-16 > 2012-08-16` Put robot.txt for google
- [x] `2012-08-16 > 2012-08-16` Remake service method order
- [x] `2012-08-09 > 2012-08-09` Fix DB config at webconfig release
- [x] `2012-08-09 > 2012-08-09` Put Post data on error e-mail
- [x] `2012-07-18 > 2012-07-18` Send parent to superservices, instead of all services
- [x] `2012-07-18 > 2012-07-18` Create Session class to Begin/Commit/Rollback
- [x] `2012-07-18 > 2012-07-18` Change this[] to return new object if none is found
- [x] `2012-06-17 > 2012-07-04` Create move edit tests
- [x] `2012-06-14 > 2012-06-14` Add Assert to check whether month/year do not change in case of error (moneyservice)
- [x] `2012-06-14 > 2012-06-14` Verify at tests if summaries are not changed in case of error
- [x] `2012-06-14 > 2012-06-14` Create too big names test
- [x] `2012-06-14 > 2012-06-14` Create move/schedule creation at closed account test
- [x] `2012-06-14 > 2012-06-14` Create move/schedule creation at disabled category test
- [x] `2012-06-14 > 2012-06-14` Put category/schedule as move save parameter
- [x] `2012-06-03 > 2012-06-03` Remove Transaction return from Begin (use session transaction)
- [x] `2012-04-07 > 2012-04-07` Save User should receive e-mail and password
- [x] `2012-04-06 > 2012-06-13` Implement BDD
- [x] `2012-03-18 > 2012-04-06` Write BDD

## 1.0.2.1 :ant: <sup>`2`</sup>
- [x] `2012-03-21 > 2012-03-22` Send error details on report e-mail
- [x] `2012-03-20 > 2012-03-20` Refactor Resharper

## 1.0.2.0 :sheep: <sup>`13`</sup>
- [x] `2012-03-17 > 2012-03-17` Publish, test if logs are working, and remove error from report Year 18967
- [x] `2012-03-16 > 2012-03-17` Test Elmah with forced error (Report > SeeYear)
- [x] `2012-03-14 > 2012-03-16` Remove delegates from Getters to send e-mail
- [x] `2012-03-14 > 2012-03-14` Separate Multilanguage from MVC
- [x] `2012-03-14 > 2012-03-14` Remove delegate DeleteSummary / another tries duplicated
- [x] `2012-02-02 > 2012-03-14` When the schedule is boundless, when the futuremove becomes a move, it should create another future move
- [x] `2012-01-30 > 2012-01-31` Layout with shadows and 3D
- [x] `2012-01-30 > 2012-01-31` Option to put (current/total) on scheduled move descriptions
- [x] `2012-01-28 > 2012-01-28` Highlight chosen account (account area)
- [x] `2012-01-04 > 2012-01-27` Change Scheduler to use FutureMove
- [x] `2011-12-28 > 2012-01-03` Create superServices and migrate use to them
- [x] `2011-12-05 > 2011-12-19` Separate BusinessLogic from NHibernate
- [x] `2011-12-02 > 2011-12-05` Reorganizing layers

## 1.0.1.0 :sheep: <sup>`5`</sup>
- [x] `2011-11-25 > 2011-11-30` Tests and Publish
- [x] `2011-10-25 > 2011-10-25` Error handling
- [x] `2011-10-25 > 2011-10-25` Protection for Elmah log
- [x] `2011-10-24 > 2011-10-25` Try to pass Log4Net / Elmah to MySql: FAIL
- [x] `2011-10-19 > 2011-10-19` Log4Net / Elmah

## 1.0.0.4 :ant: <sup>`6`</sup>
- [x] `2011-11-11 > 2011-11-11` Enable ZERO for red and yellow signs
- [x] `2011-11-09 > 2011-11-09` Verify if password recover disable user
- [x] `2011-11-09 > 2011-11-22` Send e-mail on move creation (user will be able to disable in the future)
- [x] `2011-11-09 > 2011-11-09` Replace links by icons at categories
- [x] `2011-11-08 > 2011-11-08` Alphabetical order at accounts (menu, opened, closed) and categories
- [x] `2011-10-23 > 2011-10-23` Fix error on transfer move: was not getting accounts

## 1.0.0.3 :ant: <sup>`4`</sup>
- [x] `2011-10-13 > 2011-10-14` Fix error with move creation (foreign key fail)
- [x] `2011-10-11 > 2011-10-13` Use icons instead of words at accounts list
- [x] `2011-10-10 > 2011-10-10` Validate user by e-mail - hashcode (WARN ABOUT NO LINK TO FINANCIAL INSTITUTIONS)
- [x] `2011-10-02 > 2011-10-09` Password recover - hashcode

## 1.0.0.2 :ant: <sup>`5`</sup>
- [x] `2011-10-01 > 2011-10-01` Put BETA at site logo
- [x] `2011-10-01 > 2011-10-01` Make error message darker (inverse color?)
- [x] `2011-10-01 > 2011-10-01` User register date
- [x] `2011-09-28 > 2011-10-01` Password confirm
- [x] `2011-09-25 > 2011-09-26` Date format conflict

## 1.0.0.1 :ant: <sup>`2`</sup>
- [x] `2011-09-24 > 2011-09-24` Move, when change In>Out or Out>In, was showing error about the other account
- [x] `2011-09-24 > 2011-09-24` Fix NH access at publish site

## 1.0.0.0 :dragon: <sup>`52`</sup>
- [x] `2011-09-22 > 2011-09-22` Publish
- [x] `2011-09-22 > 2011-09-22` Verify IE
- [x] `2011-09-21 > 2011-09-22` Limit text field sizes
- [x] `2011-09-21 > 2011-09-21` Encrypt password
- [x] `2011-09-18 > 2011-09-19` Implement account limits (and warning about limits) - yellow / red sign
- [x] `2011-09-18 > 2011-09-18` Remove account nature. FORGET IT: Change account creation / edit to add details
- [x] `2011-09-15 > 2011-09-15` Robot for fix taxes
- [x] `2011-09-15 > 2011-09-15` Bank: taxes + date / Debt: grow percent + date  / Credit: taxes + data, limit  / Charge: daily spending, max charge
- [x] `2011-09-15 > 2011-09-15` Refactor: put error texts in enum
- [x] `2011-09-15 > 2011-09-15` Remove Login field
- [x] `2011-09-15 > 2011-09-15` Protect edit / delete that do not belongs to logged in user
- [x] `2011-09-13 > 2011-09-15` Break Summary Value by In / Out
- [x] `2011-09-12 > 2011-09-12` Implement frequency Daily at Scheduler (test functions that use Enum values)
- [x] `2011-09-11 > 2011-09-12` if the first schedule is future, insert Move and save new when run, to change Summary
- [x] `2011-09-10 > 2011-09-10` Arrancar IsValid de Summary
- [x] `2011-09-07 > 2011-09-11` Create schedule robot (control by request) - get last and copy
- [x] `2011-09-05 > 2011-09-06` Interface of boundless schedule
- [x] `2011-09-04 > 2011-09-05` Refactor: extensions of entities, so just the props stay with entities
- [x] `2011-09-02 > 2011-09-02` Refactor: kill SelectOne and Select with lambda
- [x] `2011-08-28 > 2011-09-02` Fix moves (create Schedule)
- [x] `2011-08-27 > 2011-08-27` Change color to yellow / put icon
- [x] `2011-08-23 > 2011-08-27` Review layout (verify top menu, if it fits)
- [x] `2011-08-23 > 2011-08-23` Submenu with reports
- [x] `2011-08-22 > 2011-08-22` Report link of closed account goes to last reports
- [x] `2011-08-22 > 2011-08-22` Empty account - delete / Account with Moves - close
- [x] `2011-08-22 > 2011-08-22` Put enter button, when already logged in
- [x] `2011-08-22 > 2011-08-22` Make EntityData static
- [x] `2011-08-19 > 2011-08-21` Unify layout page
- [x] `2011-08-18 > 2011-08-22` Navigation Month / Year
- [x] `2011-08-18 > 2011-08-18` Remove quantity from single value
- [x] `2011-08-16 > 2011-08-17` Layout
- [x] `2011-08-15 > 2011-08-16` WCF started
- [x] `2011-08-14 > 2011-08-14` Implement "keep logged in"
- [x] `2011-08-10 > 2011-08-14` Move schedule
- [x] `2011-08-09 > 2011-08-10` REFACTORING
- [x] `2011-08-03 > 2011-08-08` Select with lambda expression at BaseData
- [x] `2011-08-01 > 2011-08-03` Month balance (adapt move edit / creation)
- [x] `2011-08-01 > 2011-08-01` Category edit
- [x] `2011-08-01 > 2011-08-01` Category disable
- [x] `2011-07-31 > 2011-08-01` Categories list
- [x] `2011-07-25 > 2011-07-31` Translation
- [x] `2011-07-24 > 2011-07-25` Multilanguage
- [x] `2011-07-24 > 2011-07-24` Listing disabled accounts
- [x] `2011-07-24 > 2011-07-24` Account disable
- [x] `2011-07-23 > 2011-07-24` Account edit
- [x] `2011-07-23 > 2011-07-23` Details delete
- [x] `2011-07-23 > 2011-07-23` Implement quantity at details
- [x] `2011-07-23 > 2011-07-23` Move delete
- [x] `2011-07-18 > 2011-07-23` Remove Transfer entity
- [x] `2011-06-29 > 2011-07-18` Move edit
- [x] `2011-06-20 > 2011-06-29` Remake routing to have route by account
- [x] `2011-06-01 > 2011-06-20` Move details
