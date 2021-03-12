package com.darakeon.dfm.accounts

import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.accounts.Account
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.getDecimal
import com.darakeon.dfm.utils.api.ActivityMock
import kotlinx.android.synthetic.main.accounts_line.view.name
import kotlinx.android.synthetic.main.accounts_line.view.value
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class AccountLineTest: BaseTest() {
	private lateinit var activity: AccountsActivity
	private lateinit var accountLine: AccountLine

	@Before
	fun setup() {
		activity = ActivityMock(AccountsActivity::class).get()
		activity.onCreate(Bundle(), null)

		accountLine = activity.layoutInflater
			.inflate(R.layout.accounts_line, null)
			as AccountLine
	}

	@Test
	fun setAccountPositive() {
		val account = Account("Account", 27.0, "account")

		accountLine.setAccount(account)

		assertThat(accountLine.name.text.toString(), `is`(account.name))
		assertThat(accountLine.value.text.toString(), `is`("+27.00".getDecimal()))

		val color = activity.getColor(android.R.color.holo_blue_dark)
		assertThat(accountLine.value.currentTextColor, `is`(color))
	}

	@Test
	fun setAccountNegative() {
		val account = Account("Account", -27.0, "account")

		accountLine.setAccount(account)

		assertThat(accountLine.name.text.toString(), `is`(account.name))
		assertThat(accountLine.value.text.toString(), `is`("-27.00".getDecimal()))

		val color = activity.getColor(android.R.color.holo_red_dark)
		assertThat(accountLine.value.currentTextColor, `is`(color))
	}

	@Test
	fun setAccountClick() {
		val account = Account("Account", -27.0, "account")

		accountLine.setAccount(account)
		accountLine.performClick()

		val intent = shadowOf(activity)
			.peekNextStartedActivity()

		val extras = intent?.extras ?: Bundle()
		assertThat(extras.getString("accountUrl"), `is`(account.url))

		val activity = intent.getCalledName()
		assertThat(activity, `is`("ExtractActivity"))
	}
}
