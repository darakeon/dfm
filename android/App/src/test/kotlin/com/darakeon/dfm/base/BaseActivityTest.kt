package com.darakeon.dfm.base

import android.net.Uri
import android.view.View
import android.view.View.GONE
import android.view.View.VISIBLE
import android.view.ViewGroup
import android.view.Window
import android.widget.Button
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.databinding.AccountsBinding
import com.darakeon.dfm.databinding.BottomMenuBinding
import com.darakeon.dfm.databinding.MovesBinding
import com.darakeon.dfm.databinding.SettingsBinding
import com.darakeon.dfm.databinding.WelcomeBinding
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.auth.Authentication
import com.darakeon.dfm.lib.auth.setValue
import com.darakeon.dfm.moves.MovesActivity
import com.darakeon.dfm.settings.SettingsActivity
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.activity.TestActivity
import com.darakeon.dfm.utils.activity.TestBaseActivity
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog
import org.robolectric.shadows.ShadowToast.getTextOfLatestToast

@RunWith(RobolectricTestRunner::class)
class BaseActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<TestActivity>

	@Before
	fun setup() {
		mocker = ActivityMock(TestActivity::class)
	}

	@After
	fun tearDown() {
		mocker.server.shutdown()
	}

	@Test
	fun callApi() {
		val activity = mocker.create()

		var called = false
		activity.testCallApi {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun callApiNull() {
		val activity = mocker.get()

		var called = false
		activity.testCallApi { called = true }

		assertFalse(called)

		val alert = getLatestAlertDialog()
		assertNotNull(alert)

		val shadow = shadowOf(alert)

		assertThat(
			shadow.title.toString(),
			`is`("Ops!")
		)

		assertThat(
			shadow.message.toString(),
			`is`("Error on calling the site, contact us")
		)
	}

	@Test
	fun ticket() {
		val activity = mocker.create()

		assertThat(activity.ticket, `is`(""))

		activity.ticket = "Hey, listen!"

		assertThat(activity.ticket, `is`("Hey, listen!"))
	}

	@Test
	fun clearAuth() {
		val activity = mocker.create()

		activity.ticket = "Hey, listen!"

		activity.clearAuth()

		assertThat(activity.ticket, `is`(""))
	}

	@Test
	fun onCreateRecoverEnvironment() {
		val activity = mocker.get()
		activity.setValue("Theme", R.style.Light.toString())

		val originalTheme = activity.theme
			.getPrivate<Int>("mThemeImpl", "mThemeResId")

		activity.onCreate(null, null)

		val newTheme = activity.theme
			.getPrivate<Int>("mThemeImpl", "mThemeResId")

		assertThat(newTheme, not(`is`(originalTheme)))
	}

	@Test
	fun onCreateTFAForgottenWarningVisible() {
		val activity = mocker.get()
		activity.onCreate(null, null)

		activity.toggleTfaForgottenWarning(true)

		val warning = activity.findViewById<View>(R.id.tfa_forgotten_warning)
		assertThat(warning.visibility, `is`(VISIBLE))
	}

	@Test
	fun onCreateTFAForgottenWarningGone() {
		val activity = mocker.get()
		activity.onCreate(null, null)

		activity.toggleTfaForgottenWarning(false)

		val warning = activity.findViewById<View>(R.id.tfa_forgotten_warning)
		assertThat(warning.visibility, `is`(GONE))
	}

	@Test
	fun onCreateAddApiAndAuth() {
		val activity = mocker.get()

		var api = activity.getPrivate<Api<TestBaseActivity>>("api")
		var auth = activity.getPrivate<Authentication>("auth")

		assertNull(api)
		assertNull(auth)

		activity.onCreate(null, null)

		api = activity.getPrivate("api")
		auth = activity.getPrivate("auth")

		assertNotNull(api)
		assertNotNull(auth)
	}

	@Test
	fun onCreateHandleScreenWithTitle() {
		val activity = mocker.get()
		activity.testTitle = R.string.app_name

		activity.onCreate(null, null)

		assertThat(activity.title.toString(), `is`("Don't fly Money"))
	}

	@Test
	fun onCreateHandleScreenWithoutTitle() {
		val activity = mocker.get()
		activity.testTitle = 0

		activity.onCreate(null, null)

		assertThat(activity.title.toString(), `is`("Don't fly Money"))
		assertTrue(activity.window.hasFeature(Window.FEATURE_NO_TITLE))
	}

	@Test
	fun onCreateHandleScreenWithContent() {
		val activity = mocker.get()

		activity.onCreate(null, null)

		val main = activity.window.decorView as ViewGroup
		assertThat(main.childCount, not(`is`(0)))
	}

	@Test
	fun onCreateSetMenuLongClicksLogout() {
		val activity = mocker.get()

		activity.isBottomMenuTest = true

		activity.onCreate(null, null)

		val shadow = shadowOf(activity)
		val binding = BottomMenuBinding.bind(shadow.contentView)

		activity.ticket = "logged in"
		activity.simulateNetwork()
		mocker.server.enqueue("empty")

		val logout = shadowOf(binding.actionLogout)
		assertNotNull(logout.onLongClickListener)

		binding.actionLogout.performLongClick()
		activity.waitTasks(mocker.server)

		assertThat(activity.ticket, `is`(""))
	}

	@Test
	fun onCreateSetMenuLongClicksLogoutWelcome() {
		val activity = mocker.get()

		activity.onCreate(null, null)

		val shadow = shadowOf(activity)
		val binding = WelcomeBinding.bind(shadow.contentView)

		activity.ticket = "logged in"
		activity.simulateNetwork()
		mocker.server.enqueue("empty")

		val logout = shadowOf(binding.actionLogout)
		assertNotNull(logout.onLongClickListener)

		binding.actionLogout.performLongClick()
		activity.waitTasks(mocker.server)

		assertThat(activity.ticket, `is`(""))
	}

	@Test
	fun onCreateProcessQuery() {
		val activity = mocker.get()
		activity.intent.data = Uri.parse("?X=Z")

		activity.onCreate(null, null)

		assertThat(activity.query.size, `is`(1))
		assertTrue(activity.query.keys.contains("X"))
		assertThat(activity.query["X"], `is`("Z"))
	}

	@Test
	fun onCreateCustomizeBottomMenuGenericActivity() {
		val activity = mocker.get()
		activity.isBottomMenuTest = true

		activity.onCreate(null, null)

		val shadow = shadowOf(activity)
		val binding = BottomMenuBinding.bind(shadow.contentView)

		val menu = binding.bottomMenu
		for (b in 0 until menu.childCount) {
			val button = menu.getChildAt(b) as Button
			assertNotNull(button.typeface)
		}

		assertTrue(binding.actionHome.isEnabled)
		assertTrue(binding.actionSettings.isEnabled)
		assertTrue(binding.actionMove.isEnabled)
	}

	@Test
	fun onCreateCustomizeBottomMenuAccountsActivity() {
		val activity = ActivityMock(AccountsActivity::class).create()

		val shadow = shadowOf(activity)
		val binding = AccountsBinding.bind(shadow.contentView)

		val menu = binding.bottomMenu
		assertFalse(menu.actionHome.isEnabled)
		assertTrue(menu.actionSettings.isEnabled)
		assertTrue(menu.actionMove.isEnabled)
	}

	@Test
	fun onCreateCustomizeBottomMenuSettingsActivity() {
		val activity = ActivityMock(SettingsActivity::class).create()

		val shadow = shadowOf(activity)
		val binding = SettingsBinding.bind(shadow.contentView)

		val menu = binding.bottomMenu
		assertTrue(menu.actionHome.isEnabled)
		assertFalse(menu.actionSettings.isEnabled)
		assertTrue(menu.actionMove.isEnabled)
	}

	@Test
	fun onCreateCustomizeBottomMenuMovesActivity() {
		val activity = ActivityMock(MovesActivity::class).create()

		val shadow = shadowOf(activity)
		val binding = MovesBinding.bind(shadow.contentView)

		val menu = binding.bottomMenu
		assertTrue(menu.actionHome.isEnabled)
		assertTrue(menu.actionSettings.isEnabled)
		assertFalse(menu.actionMove.isEnabled)
	}

	@Test
	fun onDestroy() {
		val activity = mocker.create()

		val api = activity.getPrivate<Api<TestBaseActivity>>("api")
		api.wakeUpSite{}

		activity.testDestroy()

		assertTrue(api.cancelled)
	}

	@Test
	fun back() {
		val activity = mocker.create()

		val shadow = shadowOf(activity)
		shadow.clearNextStartedActivities()

		assertFalse(activity.isFinishing)

		val view = View(activity)
		activity.back(view)

		assertTrue(activity.isFinishing)
	}

	@Test
	fun refresh() {
		val activity = mocker.create()

		val shadow = shadowOf(activity)
		shadow.clearNextStartedActivities()

		val view = View(activity)
		activity.refresh(view)

		val called = shadow
			.peekNextStartedActivity()
			.getCalledName()

		assertThat(called, `is`("TestActivity"))
	}

	@Test
	fun showLongClickWarning() {
		val activity = mocker.create()

		val view = View(activity)

		activity.showLongClickWarning(view)

		val text = getTextOfLatestToast()
		assertThat(text, `is`("hold for a while to execute the action"))
	}

	@Test
	fun goToAccounts() {
		val activity = mocker.create()

		val view = View(activity)
		activity.goToAccounts(view)
		activity.waitTasks(mocker.server)

		val called = shadowOf(activity)
			.peekNextStartedActivity()
			.getCalledName()

		assertThat(called, `is`("AccountsActivity"))
	}

	@Test
	fun goToSettings() {
		val activity = mocker.create()

		val view = View(activity)
		activity.goToSettings(view)
		activity.waitTasks(mocker.server)

		val called = shadowOf(activity)
			.peekNextStartedActivity()
			.getCalledName()

		assertThat(called, `is`("SettingsActivity"))
	}

	@Test
	fun createMove() {
		val activity = mocker.create()

		val view = View(activity)
		activity.createMove(view)
		activity.waitTasks(mocker.server)

		val called = shadowOf(activity)
			.peekNextStartedActivity()
			.getCalledName()

		assertThat(called, `is`("MovesActivity"))
	}

	@Test
	fun getExtraOrUrlWithAtExtra() {
		val activity = mocker.create()
		activity.intent.putExtra("link", "zelda")

		val value = activity.testGetExtraOrUrl("link")

		assertThat(value, `is`("zelda"))
	}

	@Test
	fun getExtraOrUrlWithAtUrl() {
		val activity = mocker.create()
		activity.query["link"] = "zelda"

		val value = activity.testGetExtraOrUrl("link")

		assertThat(value, `is`("zelda"))
	}

	@Test
	fun getExtraOrUrlWithAtExtraAndUrl() {
		val activity = mocker.create()
		activity.intent.putExtra("link", "zelda")
		activity.query["link"] = "wrong"

		val value = activity.testGetExtraOrUrl("link")

		assertThat(value, `is`("zelda"))
	}

	@Test
	fun getExtraOrUrlNull() {
		val activity = mocker.create()

		val value = activity.testGetExtraOrUrl("link")

		assertNull(value)
	}
}
