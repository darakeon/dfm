package com.darakeon.dfm.api

import com.darakeon.dfm.api.entities.accounts.AccountList
import com.darakeon.dfm.api.entities.extract.Extract
import com.darakeon.dfm.api.entities.login.Login
import com.darakeon.dfm.api.entities.moves.Move
import com.darakeon.dfm.api.entities.moves.MoveCreation
import com.darakeon.dfm.api.entities.moves.Nature
import com.darakeon.dfm.api.entities.settings.Settings
import com.darakeon.dfm.api.entities.summary.Summary
import com.darakeon.dfm.extensions.getPrivate
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.activity.TestActivity
import com.darakeon.dfm.utils.log.LogRule
import com.darakeon.dfm.utils.robolectric.simulateNetwork
import com.darakeon.dfm.utils.waitTasksFinish
import org.junit.After
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog

@RunWith(RobolectricTestRunner::class)
class ApiTest {
	@get:Rule
	val log = LogRule()

	private lateinit var mocker: ActivityMock
	private lateinit var activity: TestActivity
	private lateinit var api: Api

	private val server
		get() = mocker.server

	@Before
	fun setup() {
		mocker = ActivityMock()
		activity = mocker.create()
		activity.simulateNetwork()

		api = activity.getPrivate("api")
	}

	@After
	fun tearDown() {
		server.shutdown()
	}

	@Test
	fun listAccounts() {
		server.enqueue("account_list")

		var accountList: AccountList? = null
		api.listAccounts {
			accountList = it
		}
 		assertNotNull(accountList)
	}

	@Test
	fun getExtract() {
		server.enqueue("extract")

		var extract: Extract? = null
		api.getExtract("", 0, 0) {
			extract = it
		}
		assertNotNull(extract)
	}

	@Test
	fun check() {
		server.enqueue("empty")

		var called = false
		api.check(0, Nature.Transfer) {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun uncheck() {
		server.enqueue("empty")

		var called = false
		api.uncheck(0, Nature.Transfer) {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun delete() {
		server.enqueue("empty")

		var called = false
		api.delete(0) {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun login() {
		server.enqueue("login")

		var login: Login? = null
		api.login("", "") {
			login = it
		}
		assertNotNull(login)
	}

	@Test
	fun logout() {
		server.enqueue("empty")

		var called = false
		api.logout{
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun getMove() {
		server.enqueue("move_get")

		var moveCreation: MoveCreation? = null
		api.getMove(0) {
			moveCreation = it
		}

		waitTasksFinish()
		assertNotNull(moveCreation)
	}

	@Test
	fun saveMove() {
		server.enqueue("empty")

		var called = false
		api.saveMove(Move()) {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun getConfig() {
		server.enqueue("config_get")

		var settings: Settings? = null
		api.getConfig{
			settings = it
		}
		assertNotNull(settings)
	}

	@Test
	fun saveConfig() {
		server.enqueue("empty")

		var called = false
		api.saveConfig(Settings()) {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun getSummary() {
		server.enqueue("summary")

		var summary: Summary? = null
		api.getSummary("", 0) {
			summary = it
		}
		assertNotNull(summary)
	}

	@Test
	fun validateTFA() {
		server.enqueue("empty")

		var called = false
		api.validateTFA("") {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun wakeUpSite() {
		server.enqueue("empty")

		var called = false
		api.wakeUpSite{
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun error() {
		server.enqueue("error")

		var called = false
		api.wakeUpSite{
			called = true
		}
		assertFalse(called)
	}

	@Test
	fun cancel() {
		val ui = api.getPrivate<UIHandler>(
			"requestHandler",
			"uiHandler"
		)

		ui.startUIWait()

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		api.cancel()

		assertFalse(alert.isShowing)
	}
}
