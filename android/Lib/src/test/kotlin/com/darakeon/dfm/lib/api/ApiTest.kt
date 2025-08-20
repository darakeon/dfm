package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.BuildConfig
import com.darakeon.dfm.lib.api.entities.accounts.AccountList
import com.darakeon.dfm.lib.api.entities.errors.ErrorList
import com.darakeon.dfm.lib.api.entities.extract.Extract
import com.darakeon.dfm.lib.api.entities.login.Ticket
import com.darakeon.dfm.lib.api.entities.moves.Lists
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.api.entities.moves.MoveCreation
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.api.entities.settings.Settings
import com.darakeon.dfm.lib.api.entities.status.StatusResponse
import com.darakeon.dfm.lib.api.entities.summary.Summary
import com.darakeon.dfm.lib.api.entities.terms.Terms
import com.darakeon.dfm.lib.api.entities.wipe.Wipe
import com.darakeon.dfm.lib.utils.ActivityMock
import com.darakeon.dfm.lib.utils.ApiActivity
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.api.internetError
import com.darakeon.dfm.testutils.api.statusDbError
import com.darakeon.dfm.testutils.api.statusMaintenance
import com.darakeon.dfm.testutils.api.statusUnknown
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.google.gson.JsonSyntaxException
import org.junit.After
import org.junit.Assert.assertEquals
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import java.util.UUID

@RunWith(RobolectricTestRunner::class)
class ApiTest: BaseTest() {
	private lateinit var mocker: ActivityMock<ApiActivity>
	private lateinit var activity: ApiActivity
	private lateinit var api: Api<ApiActivity>

	private val server
		get() = mocker.server

	private val fakeGuid
		get() = UUID.randomUUID()

	@Before
	fun setup() {
		mocker = ActivityMock(ApiActivity::class)
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
		activity.waitTasks()

		assertNotNull(accountList)
	}

	@Test
	fun getExtract() {
		server.enqueue("extract")

		var extract: Extract? = null
		api.getExtract("lcsd", 1986, 2) {
			extract = it
		}
		activity.waitTasks()

		assertNotNull(extract)

		assertEquals(
			"/accounts/lcsd/extract?year=1986&month=3",
			server.lastPath()
		)
	}

	@Test
	fun check() {
		server.enqueue("empty")

		var called = false
		api.check(fakeGuid, Nature.Transfer) {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun uncheck() {
		server.enqueue("empty")

		var called = false
		api.uncheck(fakeGuid, Nature.Transfer) {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun delete() {
		server.enqueue("empty")

		var called = false
		api.delete(fakeGuid) {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun signup() {
		server.enqueue("signup")

		var called = false
		api.signup(
			"dfm@dontflymoney.com",
			"password",
			true,
			"pt-BR",
			"UTC-03:00",
		) {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun login() {
		server.enqueue("login")

		var login: Ticket? = null
		api.login("", "") {
			login = it
		}
		activity.waitTasks()

		assertNotNull(login)
	}

	@Test
	fun logout() {
		server.enqueue("empty")

		var called = false
		api.logout {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun getMove() {
		server.enqueue("move_get")

		var moveCreation: MoveCreation? = null
		api.getMove(fakeGuid) {
			moveCreation = it
		}
		activity.waitTasks()

		assertNotNull(moveCreation)
	}

	@Test
	fun relations() {
		server.enqueue("relations")

		var lists: Lists? = null
		api.listsForMoves {
			lists = it
		}
		activity.waitTasks()

		assertNotNull(lists)
	}

	@Test
	fun saveMoveNew() {
		server.enqueue("empty")

		var called = false
		api.saveMove(Move()) {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun saveMoveEdit() {
		server.enqueue("empty")

		val move = Move(fakeGuid)

		var called = false
		api.saveMove(move) {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun getSettings() {
		server.enqueue("config_get")

		var settings: Settings? = null
		api.getSettings {
			settings = it
		}
		activity.waitTasks()

		assertNotNull(settings)
	}

	@Test
	fun saveSettings() {
		server.enqueue("empty")

		var called = false
		api.saveSettings(Settings()) {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun getSummary() {
		server.enqueue("summary")

		var summary: Summary? = null
		api.getSummary("lcsd", 1986) {
			summary = it
		}
		activity.waitTasks()

		assertNotNull(summary)

		assertEquals(
			"/accounts/lcsd/summary?year=1986",
			server.lastPath()
		)
	}

	@Test
	fun validateTFA() {
		server.enqueue("empty")

		var called = false
		api.validateTFA("") {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun removeTFA() {
		server.enqueue("empty")

		var called = false
		api.removeTFA("") {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun wipe() {
		server.enqueue("empty")

		var called = false
		api.wipe(Wipe("")) {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun getTerms() {
		server.enqueue("terms_get")

		var terms: Terms? = null
		api.getTerms {
			terms = it
		}
		activity.waitTasks()

		assertNotNull(terms)
	}

	@Test
	fun wakeUpSite() {
		server.enqueue("status")

		var status: StatusResponse? = null
		api.wakeUpSite {
			status = it
		}
		activity.waitTasks()

		assertNotNull(status)
	}

	@Test
	fun countErrors() {
		server.enqueue("log_count")

		var errors: ErrorList? = null
		api.countErrors {
			errors = it
		}
		activity.waitTasks()

		assertNotNull(errors)
	}

	@Test
	fun listErrors() {
		server.enqueue("log_list")

		var errors: ErrorList? = null
		api.listErrors {
			errors = it
		}
		activity.waitTasks()

		assertNotNull(errors)
	}

	@Test
	fun archiveError() {
		server.enqueue("log_list")

		var called = false
		api.archiveErrors("id") {
			called = true
		}
		activity.waitTasks()

		assertTrue(called)
	}

	@Test
	fun error() {
		server.enqueue("error")

		var status: StatusResponse? = null
		api.wakeUpSite {
			status = it
		}
		assertNull(status)
	}

	@Test
	fun cancel() {
		api.wakeUpSite { }
		api.cancel()
		assertTrue(activity.waitEnded)
	}

	@Test
	fun receiveXMLDebug() {
		if (!BuildConfig.DEBUG)
			return

		server.enqueue("xml")

		api.wakeUpSite { }
		activity.waitTasks()

		assertNull(activity.errorText)
		assertNotNull(activity.error)
		assertEquals(activity.error!!::class, JsonSyntaxException::class)
	}

	@Test
	fun receiveXMLRelease() {
		if (BuildConfig.DEBUG)
			return

		server.enqueue("xml")

		api.wakeUpSite { }
		activity.waitTasks()

		assertEquals(internetError, activity.errorText)
		assertNotNull(activity.error)
		assertEquals(activity.error!!::class, JsonSyntaxException::class)
	}

	@Test
	fun statusNoneRoot() {
		server.enqueue("status_none")

		var status: StatusResponse? = null
		api.wakeUpSite {
			status = it
		}

		assertNull(status)
		assertEquals(statusUnknown, activity.errorText)
	}

	@Test
	fun statusNoneAfter404() {
		server.enqueue(404)
		server.enqueue("status_none")

		var called = false
		api.getTerms {
			called = true
		}

		activity.waitTasks(mocker.server)

		assertFalse(called)
		assertEquals("/", server.lastPath())
		assertEquals(statusUnknown, activity.errorText)
	}

	@Test
	fun statusDbErrorRoot() {
		server.enqueue("status_db_error")

		var status: StatusResponse? = null
		api.wakeUpSite {
			status = it
		}

		activity.waitTasks(mocker.server)

		assertNull(status)
		assertEquals(statusDbError, activity.errorText)
	}

	@Test
	fun statusDbErrorAfter404() {
		server.enqueue(404)
		server.enqueue("status_db_error")

		var called = false
		api.getTerms {
			called = true
		}

		activity.waitTasks(mocker.server)

		assertFalse(called)
		assertEquals("/", server.lastPath())
		assertEquals(statusDbError, activity.errorText)
	}

	@Test
	fun statusMaintenanceRoot() {
		server.enqueue("status_maintenance")

		var status: StatusResponse? = null
		api.wakeUpSite {
			status = it
		}

		activity.waitTasks(mocker.server)

		assertNull(status)
		assertEquals(statusMaintenance, activity.errorText)
	}

	@Test
	fun statusMaintenanceAfter404() {
		server.enqueue(404)
		server.enqueue("status_maintenance")

		var called = false
		api.getTerms {
			called = true
		}

		activity.waitTasks(mocker.server)

		assertFalse(called)
		assertEquals("/", server.lastPath())
		assertEquals(statusMaintenance, activity.errorText)
	}
}
