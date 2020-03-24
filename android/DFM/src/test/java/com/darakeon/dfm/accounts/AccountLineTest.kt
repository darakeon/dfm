package com.darakeon.dfm.accounts

import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.accounts.Account
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.activity.TestActivity
import com.darakeon.dfm.utils.activity.getActivityName
import com.darakeon.dfm.utils.getDecimal
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
class AccountLineTest {
	private lateinit var activity: TestActivity
	private lateinit var accountLine: AccountLine

	@Before
	fun setup() {
		activity = ActivityMock().create()
		accountLine = activity.layoutInflater
			.inflate(R.layout.accounts_line, null)
			as AccountLine
	}

	@Test
	fun setAccountPositive() {
		val account = Account("Account", 27.0, "account")

		accountLine.setAccount(account)

		assertThat(accountLine.name.text.toString(), `is`(account.name))
		assertThat(accountLine.value.text.toString(), `is`("27.00".getDecimal()))

		val color = activity.getColor(R.color.positive_dark)
		assertThat(accountLine.value.currentTextColor, `is`(color))
	}

	@Test
	fun setAccountNegative() {
		val account = Account("Account", -27.0, "account")

		accountLine.setAccount(account)

		assertThat(accountLine.name.text.toString(), `is`(account.name))
		assertThat(accountLine.value.text.toString(), `is`("27.00".getDecimal()))

		val color = activity.getColor(R.color.negative_dark)
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

		val activity = intent.getActivityName()
		assertThat(activity, `is`("ExtractActivity"))
	}
}
