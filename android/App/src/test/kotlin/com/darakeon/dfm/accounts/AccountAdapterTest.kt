package com.darakeon.dfm.accounts

import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.AccountsLineBinding
import com.darakeon.dfm.lib.api.entities.accounts.Account
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class AccountAdapterTest: BaseTest() {
	@Test
	fun populateView() {
		val list = listOf(Account("name", 0.0, "url"))
		val activity = ActivityMock(AccountsActivity::class).create()
		val adapter = AccountAdapter(activity, list)
		val line = activity.layoutInflater
			.inflate(R.layout.accounts_line, null)
			as AccountLine

		adapter.populateView(line, 0)

		val binding = AccountsLineBinding.bind(line)

		assertThat(binding.name.text.toString(), `is`("name"))
	}
}
