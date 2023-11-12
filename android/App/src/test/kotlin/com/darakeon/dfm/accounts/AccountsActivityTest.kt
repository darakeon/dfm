package com.darakeon.dfm.accounts

import android.os.Bundle
import android.os.PersistableBundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.AccountsBinding
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.accounts.Account
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.api.readBundle
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.api.ActivityMock
import com.google.gson.Gson
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
class AccountsActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<AccountsActivity>
	private lateinit var activity: AccountsActivity

	@Before
	fun setup() {
		mocker = ActivityMock(AccountsActivity::class)
		activity = mocker.get()
	}

	@Test
	fun structure() {
		activity.onCreate(Bundle(), null)

		assertNotNull(activity.findViewById(R.id.empty_list))
		assertNotNull(activity.findViewById(R.id.main_table))
	}

	@Test
	fun onCreateFromApi() {
		activity.simulateNetwork()
		mocker.server.enqueue("account_list")

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val accountList = activity
			.getPrivate<List<Account>>("accountList")

		assertThat(accountList.size, `is`(2))
		assertThat(accountList[0].name, `is`("Account 1"))
		assertThat(accountList[0].total, `is`(1.0))
		assertThat(accountList[0].url, `is`("account_1"))
		assertThat(accountList[1].name, `is`("Account 2"))
		assertThat(accountList[1].total, `is`(2.0))
		assertThat(accountList[1].url, `is`("account_2"))
	}

	@Test
	fun onCreateWithSavedState() {
		val saved = Bundle()
		saved.putString("accountList", readBundle("accounts"))

		activity.onCreate(saved, null)

		val accountList = activity
			.getPrivate<List<Account>>("accountList")

		assertThat(accountList.size, `is`(2))
		assertThat(accountList[0].name, `is`("Account 1"))
		assertThat(accountList[0].total, `is`(1.0))
		assertThat(accountList[0].url, `is`("account_1"))
		assertThat(accountList[1].name, `is`("Account 2"))
		assertThat(accountList[1].total, `is`(2.0))
		assertThat(accountList[1].url, `is`("account_2"))
	}

	@Test
	fun onCreateFillAccountsEmpty() {
		val saved = Bundle()
		saved.putJson("accountList", emptyArray<Account>())

		activity.onCreate(saved, null)

		val binding = AccountsBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.emptyList.visibility, `is`(View.VISIBLE))
		assertThat(binding.mainTable.visibility, `is`(View.GONE))
		assertNull(binding.mainTable.adapter)
	}

	@Test
	fun onCreateFillAccountsFilled() {
		val saved = Bundle()
		saved.putString("accountList", readBundle("accounts"))

		activity.onCreate(saved, null)

		val binding = AccountsBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.emptyList.visibility, `is`(View.GONE))
		assertThat(binding.mainTable.visibility, `is`(View.VISIBLE))
		assertThat(binding.mainTable.adapter.count, `is`(2))
	}

	@Test
	fun onSaveInstance() {
		val originalAccounts = Gson().fromJson(
			readBundle("accounts"),
			Array<Account>::class.java
		)

		val originalState = Bundle()
		originalState.putJson("accountList", originalAccounts)

		activity.onCreate(originalState, null)

		val newState = Bundle()
		activity.onSaveInstanceState(newState, PersistableBundle())

		val newAccounts: Array<Account> = newState
			.getFromJson("accountList", emptyArray())
		assertThat(newAccounts, `is`(originalAccounts))
	}
}
