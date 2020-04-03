package com.darakeon.dfm.accounts

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.accounts.Account
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.log.LogRule
import kotlinx.android.synthetic.main.accounts_line.view.name
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class AccountAdapterTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun populateView() {
		val list = listOf(Account("name", 0.0, "url"))
		val activity = ActivityMock().create<AccountsActivity>()
		val adapter = AccountAdapter(activity, list)
		val line = activity.layoutInflater
			.inflate(R.layout.accounts_line, null)
			as AccountLine

		adapter.populateView(line, 0)

		assertThat(line.name.text.toString(), `is`("name"))
	}
}
