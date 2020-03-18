package com.darakeon.dfm.api

import com.darakeon.dfm.api.entities.Date
import com.darakeon.dfm.api.entities.moves.Move
import com.darakeon.dfm.api.entities.moves.Nature
import com.darakeon.dfm.api.entities.settings.Settings
import com.darakeon.dfm.utils.api.Server
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Assert.assertNotNull
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class RequestServiceTest {
	private lateinit var server: Server

	private val service
		get() = server.service

	@Before
	fun setup() {
		server = Server()
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
		assertThat(environment.mobileTheme, `is`("Dark"))

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

		val response = service.getExtract("account", 202003).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		val extract = body.data!!
		assertThat(extract.title, `is`("March"))
		assertThat(extract.total, `is`(27.0))
		assertThat(extract.canCheck, `is`(true))

		val moveList = extract.moveList
		assertThat(moveList.size, `is`(1))

		assertThat(moveList[0].description, `is`("move"))
		assertThat(moveList[0].date, `is`(Date(2020, 3, 8)))
		assertThat(moveList[0].total, `is`(27.0))
		assertThat(moveList[0].checked, `is`(true))
		assertThat(moveList[0].id, `is`(1))
	}

	@Test
	fun check() {
		server.enqueue("empty")

		val response = service.check(1, Nature.Out).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		assertNotNull(body.data)
	}

	@Test
	fun uncheck() {
		server.enqueue("empty")

		val response = service.uncheck(1, Nature.Out).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		assertNotNull(body.data)
	}

	@Test
	fun delete() {
		server.enqueue("empty")

		val response = service.delete(1).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		assertNotNull(body.data)
	}

	@Test
	fun login() {
		server.enqueue("login")

		val response = service.login("dfm@dontflymoney.com", "password").execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

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
		assertThat(environment.mobileTheme, `is`("Dark"))

		assertNotNull(body.data)
	}

	@Test
	fun getMove() {
		server.enqueue("move_get")

		val response = service.getMove(1).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		val data = body.data!!
		assertThat(data.isUsingCategories, `is`(true))

		val natureList = data.natureList
		assertThat(natureList.size, `is`(3))
		assertThat(natureList[0].text, `is`("Out"))
		assertThat(natureList[0].value, `is`("0"))
		assertThat(natureList[1].text, `is`("In"))
		assertThat(natureList[1].value, `is`("1"))
		assertThat(natureList[2].text, `is`("Transfer"))
		assertThat(natureList[2].value, `is`("2"))

		val accountList = data.accountList
		assertThat(accountList.size, `is`(1))
		assertThat(accountList[0].text, `is`("Account"))
		assertThat(accountList[0].value, `is`("account"))

		val categoryList = data.categoryList!!
		assertThat(categoryList.size, `is`(1))
		assertThat(categoryList[0].text, `is`("Category"))
		assertThat(categoryList[0].value, `is`("category"))

		val move = data.move!!
		assertThat(move.id, `is`(1))
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
	fun saveMove() {
		server.enqueue("empty")

		val response = service.saveMove(1, Move()).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		assertNotNull(body.data)
	}

	@Test
	fun getConfig() {
		server.enqueue("config_get")

		val response = service.getConfig().execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		val settings = body.data!!
		assertThat(settings.moveCheck, `is`(true))
		assertThat(settings.useCategories, `is`(true))
	}

	@Test
	fun saveConfig() {
		server.enqueue("empty")

		val response = service.saveConfig(Settings()).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		assertNotNull(body.data)
	}

	@Test
	fun getSummary() {
		server.enqueue("summary")

		val response = service.getSummary("account", 2020).execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		val summary = body.data!!
		assertThat(summary.title, `is`("account"))
		assertThat(summary.total, `is`(1.0))

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

		val response = service.validateTFA("123456").execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		assertNotNull(body.data)
	}

	@Test
	fun wakeUpSite() {
		server.enqueue("empty")

		val response = service.wakeUpSite().execute()
		assertNotNull(response)
		val body = response.body()!!

		val environment = body.environment!!
		assertThat(environment.language, `is`("pt-BR"))
		assertThat(environment.mobileTheme, `is`("Dark"))

		assertNotNull(body.data)
	}
}
