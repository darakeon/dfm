package com.darakeon.dfm.accounts

import android.os.Bundle
import android.os.PersistableBundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.accounts.Account
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.getPrivate
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.api.readBundle
import com.darakeon.dfm.utils.robolectric.simulateNetwork
import com.google.gson.Gson
import kotlinx.android.synthetic.main.accounts.empty_list
import kotlinx.android.synthetic.main.accounts.main_table
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class AccountsActivityTest {
	private lateinit var mocker: ActivityMock
	private lateinit var activity: AccountsActivity

	@Before
	fun setup() {
		mocker = ActivityMock()
		activity = mocker.get<AccountsActivity>()
	}

	@Test
	fun structure() {
		activity.onCreate(null, null)
		assertNotNull(activity.findViewById(R.id.empty_list))
		assertNotNull(activity.findViewById(R.id.main_table))
	}

	@Test
	fun onCreateFromApi() {
		activity.simulateNetwork()
		mocker.server.enqueue("account_list")

		activity.onCreate(null, null)

		val accountList = activity
			.getPrivate<Array<Account>>("accountList")

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
			.getPrivate<Array<Account>>("accountList")

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

		assertThat(activity.empty_list.visibility, `is`(View.VISIBLE))
		assertThat(activity.main_table.visibility, `is`(View.GONE))
		assertNull(activity.main_table.adapter)
	}

	@Test
	fun onCreateFillAccountsFilled() {
		val saved = Bundle()
		saved.putString("accountList", readBundle("accounts"))

		activity.onCreate(saved, null)

		assertThat(activity.empty_list.visibility, `is`(View.GONE))
		assertThat(activity.main_table.visibility, `is`(View.VISIBLE))
		assertThat(activity.main_table.adapter.count, `is`(2))
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
