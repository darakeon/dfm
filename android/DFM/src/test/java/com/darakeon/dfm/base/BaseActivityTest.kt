package com.darakeon.dfm.base

import android.net.Uri
import android.view.ContextMenu
import android.view.View
import android.view.ViewGroup
import android.view.Window
import android.widget.Button
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.api.Api
import com.darakeon.dfm.auth.setValue
import com.darakeon.dfm.extensions.getPrivate
import com.darakeon.dfm.moves.MovesCreateActivity
import com.darakeon.dfm.settings.SettingsActivity
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.activity.getActivityName
import com.darakeon.dfm.utils.robolectric.RoboContextMenu
import com.darakeon.dfm.utils.robolectric.simulateNetwork
import kotlinx.android.synthetic.main.bottom_menu.action_close
import kotlinx.android.synthetic.main.bottom_menu.action_logout
import kotlinx.android.synthetic.main.bottom_menu.bottom_menu
import kotlinx.android.synthetic.main.bottom_menu.view.action_home
import kotlinx.android.synthetic.main.bottom_menu.view.action_move
import kotlinx.android.synthetic.main.bottom_menu.view.action_settings
import org.hamcrest.CoreMatchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.hamcrest.core.Is.`is`
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Test
import org.junit.runner.RunWith
import org.mockito.Mockito.mock
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog
import org.robolectric.shadows.ShadowToast.getTextOfLatestToast

@RunWith(RobolectricTestRunner::class)
class BaseActivityTest {

	@Test
	fun callApi() {
		val activity = ActivityMock.create()

		var called = false
		activity.testCallApi {
			called = true
		}
		assertTrue(called)
	}

	@Test
	fun callApiNull() {
		val activity = ActivityMock.get()

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
		val activity = ActivityMock.create()

		assertThat(activity.ticket, `is`(""))

		activity.ticket = "Hey, listen!"

		assertThat(activity.ticket, `is`("Hey, listen!"))
	}

	@Test
	fun clearAuth() {
		val activity = ActivityMock.create()

		activity.ticket = "Hey, listen!"

		activity.clearAuth()

		assertThat(activity.ticket, `is`(""))
	}

	@Test
	fun onCreateRecoverEnvironment() {
		val activity = ActivityMock.get()
		activity.setValue("Theme", R.style.Light.toString())

		val originalTheme = activity.theme
			.getPrivate("mThemeImpl", "mThemeResId") as Int

		activity.onCreate(null, null)

		val newTheme = activity.theme
			.getPrivate("mThemeImpl", "mThemeResId") as Int

		assertThat(newTheme, not(`is`(originalTheme)))
	}

	@Test
	fun onCreateAddApiAndAuth() {
		val activity = ActivityMock.get()

		var api = activity.getPrivate("api")
		var auth = activity.getPrivate("auth")

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
		val activity = ActivityMock.get()
		activity.testTitle = R.string.app_name

		activity.onCreate(null, null)

		assertThat(activity.title.toString(), `is`("Don't fly Money"))
	}

	@Test
	fun onCreateHandleScreenWithoutTitle() {
		val activity = ActivityMock.get()
		activity.testTitle = 0

		activity.onCreate(null, null)

		assertThat(activity.title.toString(), `is`("Don't fly Money"))
		assertTrue(activity.window.hasFeature(Window.FEATURE_NO_TITLE))
	}

	@Test
	fun onCreateHandleScreenWithContent() {
		val activity = ActivityMock.get()
		activity.testContentView = R.layout.welcome

		activity.onCreate(null, null)

		val main = activity.window.decorView as ViewGroup
		assertThat(main.childCount, not(`is`(0)))
	}

	@Test
	fun onCreateHandleScreenWithContextMenu() {
		val activity = ActivityMock.get()
		val view = View(activity)
		activity.testViewWithContext = view

		val shadow = shadowOf(view)
		assertNull(shadow.onCreateContextMenuListener)

		activity.onCreate(null, null)

		assertNotNull(shadow.onCreateContextMenuListener)
	}

	@Test
	fun onCreateSetMenuLongClicksLogout() {
		val activity = ActivityMock.get()
		activity.testContentView = R.layout.bottom_menu

		activity.onCreate(null, null)

		activity.ticket = "logged in"
		activity.simulateNetwork()
		activity.server.enqueue("empty")

		val logout = shadowOf(activity.action_logout)
		assertNotNull(logout.onLongClickListener)

		activity.action_logout.performLongClick()

		assertThat(activity.ticket, `is`(""))
	}

	@Test
	fun onCreateSetMenuLongClicksClose() {
		val activity = ActivityMock.get()
		activity.testContentView = R.layout.bottom_menu

		activity.onCreate(null, null)

		val close = shadowOf(activity.action_close)
		assertNotNull(close.onLongClickListener)

		activity.action_close.performLongClick()

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()
		assertTrue(intent.extras?.getBoolean("EXIT") ?: false)
	}

	@Test
	fun onCreateProcessQuery() {
		val activity = ActivityMock.get()
		activity.intent.data = Uri.parse("?X=Z")

		activity.onCreate(null, null)

		assertThat(activity.query.size, `is`(1))
		assertTrue(activity.query.keys.contains("X"))
		assertThat(activity.query["X"], `is`("Z"))
	}

	@Test
	fun onCreateCustomizeBottomMenuGenericActivity() {
		val activity = ActivityMock.get()
		activity.testContentView = R.layout.bottom_menu

		activity.onCreate(null, null)

		val menu = activity.bottom_menu
		for (b in 0 until menu.childCount) {
			val button = menu.getChildAt(b) as Button
			assertNotNull(button.typeface)
		}

		assertTrue(menu.action_home.isEnabled)
		assertTrue(menu.action_settings.isEnabled)
		assertTrue(menu.action_move.isEnabled)
	}

	@Test
	fun onCreateCustomizeBottomMenuAccountsActivity() {
		val activity = ActivityMock.create<AccountsActivity>()

		val menu = activity.bottom_menu
		assertFalse(menu.action_home.isEnabled)
		assertTrue(menu.action_settings.isEnabled)
		assertTrue(menu.action_move.isEnabled)
	}

	@Test
	fun onCreateCustomizeBottomMenuSettingsActivity() {
		val activity = ActivityMock.create<SettingsActivity>()

		val menu = activity.bottom_menu
		assertTrue(menu.action_home.isEnabled)
		assertFalse(menu.action_settings.isEnabled)
		assertTrue(menu.action_move.isEnabled)
	}

	@Test
	fun onCreateCustomizeBottomMenuMovesCreateActivity() {
		val activity = ActivityMock.create<MovesCreateActivity>()

		val menu = activity.bottom_menu
		assertTrue(menu.action_home.isEnabled)
		assertTrue(menu.action_settings.isEnabled)
		assertFalse(menu.action_move.isEnabled)
	}

	@Test
	fun onCreateContextMenu() {
		val activity = ActivityMock.create()
		activity.hasContextMenu = true

		val menu = RoboContextMenu()
		val view = mock(View::class.java)
		val info = mock(ContextMenu.ContextMenuInfo::class.java)

		activity.onCreateContextMenu(menu, view, info)

		assertTrue(activity.inflatedContextMenu)
		assertThat(menu.size(), `is`(4))
	}

	@Test
	fun onCreateContextMenuZero() {
		val activity = ActivityMock.create()

		val menu = RoboContextMenu()
		val view = mock(View::class.java)
		val info = mock(ContextMenu.ContextMenuInfo::class.java)

		activity.onCreateContextMenu(menu, view, info)

		assertFalse(activity.inflatedContextMenu)
		assertThat(menu.size(), `is`(0))
	}

	@Test
	fun onDestroy() {
		val activity = ActivityMock.create()

		val api = activity.getPrivate("api") as Api

		activity.testDestroy()

		assertTrue(api.cancelled)
	}

	@Test
	fun back() {
		val activity = ActivityMock.create()

		val shadow = shadowOf(activity)
		shadow.clearNextStartedActivities()

		assertFalse(activity.isFinishing)

		val view = View(activity)
		activity.back(view)

		assertTrue(activity.isFinishing)
	}

	@Test
	fun refresh() {
		val activity = ActivityMock.create()

		val shadow = shadowOf(activity)
		shadow.clearNextStartedActivities()

		val view = View(activity)
		activity.refresh(view)

		val called = shadow
			.peekNextStartedActivity()
			.getActivityName()

		assertThat(called, `is`("TestActivity"))
	}

	@Test
	fun showLongClickWarning() {
		val activity = ActivityMock.create()

		val view = View(activity)

		activity.showLongClickWarning(view)

		val text = getTextOfLatestToast()
		assertThat(text, `is`("hold for a while to execute the action"))
	}

	@Test
	fun goToAccounts() {
		val activity = ActivityMock.create()

		val view = View(activity)
		activity.goToAccounts(view)

		val called = shadowOf(activity)
			.peekNextStartedActivity()
			.getActivityName()

		assertThat(called, `is`("AccountsActivity"))
	}

	@Test
	fun goToSettings() {
		val activity = ActivityMock.create()

		val view = View(activity)
		activity.goToSettings(view)

		val called = shadowOf(activity)
			.peekNextStartedActivity()
			.getActivityName()

		assertThat(called, `is`("SettingsActivity"))
	}

	@Test
	fun createMove() {
		val activity = ActivityMock.create()

		val view = View(activity)
		activity.createMove(view)

		val called = shadowOf(activity)
			.peekNextStartedActivity()
			.getActivityName()

		assertThat(called, `is`("MovesCreateActivity"))
	}

	@Test
	fun getExtraOrUrlWithAtExtra() {
		val activity = ActivityMock.create()
		activity.intent.putExtra("link", "zelda")

		val value = activity.testGetExtraOrUrl("link")

		assertThat(value, `is`("zelda"))
	}

	@Test
	fun getExtraOrUrlWithAtUrl() {
		val activity = ActivityMock.create()
		activity.query["link"] = "zelda"

		val value = activity.testGetExtraOrUrl("link")

		assertThat(value, `is`("zelda"))
	}

	@Test
	fun getExtraOrUrlWithAtExtraAndUrl() {
		val activity = ActivityMock.create()
		activity.intent.putExtra("link", "zelda")
		activity.query["link"] = "wrong"

		val value = activity.testGetExtraOrUrl("link")

		assertThat(value, `is`("zelda"))
	}

	@Test
	fun getExtraOrUrlNull() {
		val activity = ActivityMock.create()

		val value = activity.testGetExtraOrUrl("link")

		assertNull(value)
	}
}
