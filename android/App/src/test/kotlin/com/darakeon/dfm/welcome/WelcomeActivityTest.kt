package com.darakeon.dfm.welcome

import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.AccountComboItem
import com.darakeon.dfm.lib.api.entities.ComboItem
import com.darakeon.dfm.lib.auth.getValueTyped
import com.darakeon.dfm.lib.auth.setValue
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class WelcomeActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<WelcomeActivity>

	@Before
	fun setup() {
		mocker = ActivityMock(WelcomeActivity::class)
	}

	@Test
	fun structure() {
		val activity = mocker.create()
		activity.waitTasks(mocker.server)

		assertNotNull(activity.findViewById(R.id.pig))
		assertNotNull(activity.findViewById(R.id.action_logout))
		assertNotNull(activity.findViewById(R.id.action_close))
		assertNotNull(activity.findViewById(R.id.action_home))
		assertNotNull(activity.findViewById(R.id.action_move))
		assertNotNull(activity.findViewById(R.id.action_settings))
	}

	@Test
	fun redirectLoggedIn() {
		val activity = mocker.get()
		activity.setValue("Ticket", "ticket")
		activity.simulateNetwork()

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()
		assertNull(intent)
	}

	@Test
	fun redirectLoggedOut() {
		val activity = mocker.get()
		activity.setValue("Ticket", "")
		activity.simulateNetwork()

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()
		val name = intent.getCalledName()
		assertThat(name, `is`("LoginActivity"))
	}

	@Test
	fun populateCache() {
		val activity = mocker.get()
		activity.setValue("Ticket", "ticket")
		activity.simulateNetwork()
		mocker.server.enqueue("relations")

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val isUsingCategories: Boolean =
			activity.getValueTyped("isUsingCategories")
		assertThat(isUsingCategories, `is`(true))

		val accountList: Array<AccountComboItem> =
			activity.getValueTyped("accountCombo")

		assertThat(accountList.size, `is`(2))
		assertThat(accountList[0].text, `is`("Reais"))
		assertThat(accountList[0].value, `is`("reais"))
		assertThat(accountList[0].currency, `is`("BRL"))
		assertThat(accountList[1].text, `is`("Euros"))
		assertThat(accountList[1].value, `is`("euros"))
		assertThat(accountList[1].currency, `is`("EUR"))

		val categoryList: Array<ComboItem> =
			activity.getValueTyped("categoryCombo")

		assertThat(categoryList.size, `is`(1))
		assertThat(categoryList[0].text, `is`("Category"))
		assertThat(categoryList[0].value, `is`("category"))
	}
}
