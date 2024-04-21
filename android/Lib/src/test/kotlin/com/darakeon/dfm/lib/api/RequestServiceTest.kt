package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.api.entities.Date
import com.darakeon.dfm.lib.api.entities.Theme
import com.darakeon.dfm.lib.api.entities.login.Login
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.api.entities.settings.Settings
import com.darakeon.dfm.lib.api.entities.signup.SignUp
import com.darakeon.dfm.lib.api.entities.status.Status
import com.darakeon.dfm.lib.api.entities.terms.Clause
import com.darakeon.dfm.lib.api.entities.tfa.TFA
import com.darakeon.dfm.lib.api.entities.toggleCheck.ToggleCheck
import com.darakeon.dfm.lib.api.entities.wipe.Wipe
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.api.Server
import com.darakeon.dfm.testutils.api.guid
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class RequestServiceTest: BaseTest() {
	private lateinit var server: Server<RequestService>

	private val service
		get() = server.service

	@Before
	fun setup() {
		server = Server(RequestService::class, Retrofit::build)
	}

	@After
	fun tearDown() {
		server.shutdown()
	}

	@Test
	fun listAccounts() {
		server.enqueue("account_list")

		val response = service.listAccounts().execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		val accountList = body.data!!.accountList
		assertNotNull(accountList)
		assertThat(accountList.size, `is`(2))

		assertThat(accountList[0].name, `is`("Account 1"))
		assertThat(accountList[0].url, `is`("account_1"))
		assertThat(accountList[0].total, `is`(1.0))

		assertThat(accountList[1].name, `is`("Account 2"))
		assertThat(accountList[1].url, `is`("account_2"))
		assertThat(accountList[1].total, `is`(2.0))
	}

	@Test
	fun getExtract() {
		server.enqueue("extract")

		val response = service.getExtract("account", 2020, 3).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		val extract = body.data!!
		assertThat(extract.title, `is`("account"))
		assertThat(extract.total, `is`(27.0))
		assertThat(extract.canCheck, `is`(true))

		val moveList = extract.moveList
		assertThat(moveList.size, `is`(1))

		assertThat(moveList[0].description, `is`("move"))
		assertThat(moveList[0].date, `is`(Date(2020, 3, 8)))
		assertThat(moveList[0].total, `is`(27.0))
		assertThat(moveList[0].checked, `is`(true))
		assertThat(moveList[0].guid, `is`(guid))
	}

	@Test
	fun check() {
		server.enqueue("empty")

		val response = service.check(guid, ToggleCheck(Nature.Out)).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		assertNull(body.data)
	}

	@Test
	fun uncheck() {
		server.enqueue("empty")

		val response = service.uncheck(guid, ToggleCheck(Nature.Out)).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		assertNull(body.data)
	}

	@Test
	fun delete() {
		server.enqueue("empty")

		val response = service.delete(guid).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		assertNull(body.data)
	}

	@Test
	fun signup() {
		server.enqueue("signup")

		val response = service.signup(
			SignUp(
				"dfm@dontflymoney.com",
				"password",
				true,
				"pt-BR",
				"UTC-03:00",
			)
		).execute()

		assertNotNull(response)

		val body = response.body()!!

		assertNull(body.data)
	}

	@Test
	fun login() {
		server.enqueue("login")

		val response = service.login(
			Login("dfm@dontflymoney.com", "password")
		).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		val login = body.data!!
		assertThat(login.ticket, `is`("ticket"))
	}

	@Test
	fun logout() {
		server.enqueue("empty")

		val response = service.logout().execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		assertNull(body.data)
	}

	@Test
	fun getMoveEdit() {
		server.enqueue("move_get")

		val response = service.getMove(guid).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		val data = body.data!!
		val move = data.move!!
		assertThat(move.guid, `is`(guid))
		assertThat(move.description, `is`("move"))
		assertThat(move.date, `is`(Date(2020, 3, 8)))
		assertThat(move.categoryName, `is`("category"))
		assertThat(move.outUrl, `is`("out"))
		assertThat(move.inUrl, `is`("in"))
		assertThat(move.value, `is`(1.0))
		assertThat(move.checked, `is`(true))

		val detailList = move.detailList
		assertThat(detailList.size, `is`(1))
		assertThat(detailList[0].description, `is`("detail"))
		assertThat(detailList[0].amount, `is`(1))
		assertThat(detailList[0].value, `is`(27.0))
	}


	@Test
	fun relations() {
		server.enqueue("relations")

		val response = service.listsForMoves().execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		val data = body.data!!
		assertThat(data.isUsingCategories, `is`(true))

		val accountList = data.accountList
		assertThat(accountList.size, `is`(2))
		assertThat(accountList[0].text, `is`("Reais"))
		assertThat(accountList[0].value, `is`("reais"))
		assertThat(accountList[0].currency, `is`("BRL"))
		assertThat(accountList[1].text, `is`("Euros"))
		assertThat(accountList[1].value, `is`("euros"))
		assertThat(accountList[1].currency, `is`("EUR"))

		val categoryList = data.categoryList
		assertThat(categoryList.size, `is`(1))
		assertThat(categoryList[0].text, `is`("Category"))
		assertThat(categoryList[0].value, `is`("category"))
	}

	@Test
	fun saveMoveNew() {
		server.enqueue("empty")

		val response = service.saveMove(Move()).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		assertNull(body.data)
	}

	@Test
	fun saveMoveEdit() {
		server.enqueue("empty")

		val response = service.saveMove(guid, Move()).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		assertNull(body.data)
	}

	@Test
	fun getSettings() {
		server.enqueue("config_get")

		val response = service.getSettings().execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		val settings = body.data!!
		assertThat(settings.moveCheck, `is`(true))
		assertThat(settings.useCategories, `is`(true))
	}

	@Test
	fun saveSettings() {
		server.enqueue("empty")

		val response = service.saveSettings(Settings()).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		assertNull(body.data)
	}

	@Test
	fun getSummary() {
		server.enqueue("summary")

		val response = service.getSummary("account", 2020).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		val summary = body.data!!
		assertThat(summary.title, `is`("account"))
		assertThat(summary.total, `is`(2.0))

		val monthList = summary.monthList
		assertThat(monthList.size, `is`(2))
		assertThat(monthList[0].name, `is`("March"))
		assertThat(monthList[0].number, `is`(3))
		assertThat(monthList[0].total, `is`(1.0))
		assertThat(monthList[1].name, `is`("December"))
		assertThat(monthList[1].number, `is`(12))
		assertThat(monthList[1].total, `is`(1.0))
	}

	@Test
	fun validateTFA() {
		server.enqueue("empty")

		val response = service.validateTFA(
			TFA("123456")
		).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		assertNull(body.data)
	}

	@Test
	fun wipe() {
		server.enqueue("empty")

		val response = service.wipe(Wipe("")).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		assertNull(body.data)
	}

	@Test
	fun getTerms() {
		server.enqueue("terms_get")

		val response = service.getTerms().execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		val summary = body.data!!
		assertThat(summary.date,  `is`(Date(2023, 11, 20)))

		val content = Clause(
			"main",
			listOf(
				Clause(
					"clause 1",
					listOf(
						Clause("clause 1a"),
						Clause("clause 1b"),
						Clause("clause 1c"),
					)
				),
				Clause("clause 2"),
				Clause(
					"clause 3",
					listOf(
						Clause(
							"clause 3a",
							listOf(
								Clause("clause 3aI"),
								Clause("clause 3aII"),
							)
						),
						Clause("clause 3b"),
					)
				),
			)
		)
		assertThat(summary.content, `is`(content))
	}

	@Test
	fun wakeUpSite() {
		server.enqueue("status")

		val response = service.wakeUpSite().execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.theme.enum, `is`(Theme.DarkMagic))

		assertNotNull(body.data)

		val data = body.data!!
		assertThat(data.status.enum, `is`(Status.DbError))
	}

	@Test
	fun error() {
		server.enqueue("error")

		val response = service.wakeUpSite().execute()
		assertNotNull(response)
		val body = response.body()!!

		assertNull(body.environment)
		assertNull(body.data)

		assertNotNull(body.error)
		assertThat(body.error?.code, `is`(2013))
		assertThat(body.error?.text, `is`("something went wrong"))
	}

	@Test
	fun countErrors() {
		server.enqueue("log_count")

		val response = service.countErrors().execute()
		assertNotNull(response)
		val body = response.body()!!

		val errors = body.data!!
		assertThat(errors.count, `is`(4))
		assertThat(errors.logs.size, `is`(0))
	}

	@Test
	fun listErrors() {
		server.enqueue("log_list")

		val response = service.listErrors().execute()
		assertNotNull(response)
		val body = response.body()!!

		val errors = body.data!!
		assertThat(errors.count, `is`(4))
		assertThat(errors.logs.size, `is`(4))

		val log0 = errors.logs[0]
		assertThat(log0.id, `is`("20200905021935886752"))
		assertThat(log0.exception.className, `is`("NHibernate.Exceptions.GenericADOException"))
		assertThat(log0.exception.message, `is`("message 1"))
		assertNull(log0.exception.stackTrace)
		assertThat(log0.exception.source, `is`("NHibernate"))
		assertNotNull(log0.exception.innerException)
		assertThat(log0.exception.innerException?.className, `is`("MySql.Data.MySqlClient.MySqlException"))
		assertThat(log0.exception.innerException?.message, `is`("message 2"))
		assertNull(log0.exception.innerException?.stackTrace)
		assertThat(log0.exception.innerException?.source, `is`("MySql.Data"))
		assertNull(log0.exception.innerException?.innerException)

		val log1 = errors.logs[1]
		assertThat(log1.id, `is`("20200905040939507282"))
		assertThat(log1.exception.className, `is`("System.ObjectDisposedException"))
		assertThat(log1.exception.message, `is`("message 3"))
		assertThat(log1.exception.stackTrace, `is`("stack 3"))
		assertThat(log1.exception.source, `is`("Microsoft.AspNetCore.Http.Features"))
		assertNull(log1.exception.innerException)

		val log2 = errors.logs[2]
		assertThat(log2.id, `is`("20200905041255989527"))
		assertThat(log2.exception.className, `is`("System.ObjectDisposedException"))
		assertThat(log2.exception.message, `is`("message 4"))
		assertThat(log2.exception.stackTrace, `is`("stack 4"))
		assertThat(log2.exception.source, `is`("Microsoft.AspNetCore.Http.Features"))
		assertNull(log2.exception.innerException)

		val log3 = errors.logs[3]
		assertThat(log3.id, `is`("20200905041338864402"))
		assertThat(log3.exception.className, `is`("NHibernate.Exceptions.GenericADOException"))
		assertThat(log3.exception.message, `is`("message 5"))
		assertThat(log3.exception.stackTrace, `is`("stack 5"))
		assertThat(log3.exception.source, `is`("NHibernate"))
		assertNotNull(log3.exception.innerException)
		assertThat(log3.exception.innerException?.className, `is`("MySql.Data.MySqlClient.MySqlException"))
		assertThat(log3.exception.innerException?.message, `is`("message 6"))
		assertThat(log3.exception.innerException?.stackTrace, `is`("stack 6"))
		assertThat(log3.exception.innerException?.source, `is`("MySql.Data"))
		assertNull(log3.exception.innerException?.innerException)
	}

	@Test
	fun archiveError() {
		server.enqueue("empty")

		val response = service.archiveErrors("id").execute()
		assertNotNull(response)
		val body = response.body()!!

		assertNull(body.data)
	}
}
