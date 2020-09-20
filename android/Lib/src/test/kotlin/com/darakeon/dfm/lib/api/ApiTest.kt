package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.api.entities.accounts.AccountList
import com.darakeon.dfm.lib.api.entities.extract.Extract
import com.darakeon.dfm.lib.api.entities.login.Login
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.api.entities.moves.MoveCreation
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.api.entities.settings.Settings
import com.darakeon.dfm.lib.api.entities.status.ErrorList
import com.darakeon.dfm.lib.api.entities.summary.Summary
import com.darakeon.dfm.lib.utils.ActivityMock
import com.darakeon.dfm.lib.utils.ApiActivity
import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasksFinish
import org.junit.After
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import java.util.UUID

@RunWith(RobolectricTestRunner::class)
class ApiTest {
	@get:Rule
	val log = LogRule()

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

		api = activity.api!!
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
		api.check(fakeGuid, Nature.Transfer) {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun uncheck() {
		server.enqueue("empty")

		var called = false
		api.uncheck(fakeGuid, Nature.Transfer) {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun delete() {
		server.enqueue("empty")

		var called = false
		api.delete(fakeGuid) {
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
		api.logout {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun getMoveNew() {
		server.enqueue("move_get")

		var moveCreation: MoveCreation? = null
		api.getMove(null) {
			moveCreation = it
		}

		waitTasksFinish()
		assertNotNull(moveCreation)
	}

	@Test
	fun getMoveEdit() {
		server.enqueue("move_get")

		var moveCreation: MoveCreation? = null
		api.getMove(fakeGuid) {
			moveCreation = it
		}

		waitTasksFinish()
		assertNotNull(moveCreation)
	}

	@Test
	fun saveMoveNew() {
		server.enqueue("empty")

		var called = false
		api.saveMove(Move()) {
			called = true
		}
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
		assertTrue(called)
	}

	@Test
	fun getConfig() {
		server.enqueue("config_get")

		var settings: Settings? = null
		api.getConfig {
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
		api.wakeUpSite {
			called = true
		}

		assertTrue(called)
	}

	@Test
	fun countErrors() {
		server.enqueue("log_count")

		var errors: ErrorList? = null
		api.countErrors {
			errors = it
		}

		assertNotNull(errors)
	}

	@Test
	fun listErrors() {
		server.enqueue("log_list")

		var errors: ErrorList? = null
		api.listErrors {
			errors = it
		}

		assertNotNull(errors)
	}

	@Test
	fun archiveError() {
		server.enqueue("log_list")

		var called = false
		api.archiveError("id") {
			called = true
		}

		assertTrue(called)
	}

	@Test
	fun error() {
		server.enqueue("error")

		var called = false
		api.wakeUpSite {
			called = true
		}
		assertFalse(called)
	}

	@Test
	fun cancel() {
		api.cancel()
		assertTrue(activity.waitEnded)
	}
}
