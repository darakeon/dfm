package com.darakeon.dfm.settings

import android.app.AlertDialog
import android.app.Dialog
import android.os.Bundle
import android.os.PersistableBundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.settings.Settings
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.getPrivate
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.activity.getActivityName
import com.darakeon.dfm.utils.api.readBundle
import com.darakeon.dfm.utils.log.LogRule
import com.darakeon.dfm.utils.robolectric.simulateNetwork
import com.darakeon.dfm.utils.waitTasksFinish
import com.darakeon.dfm.welcome.WelcomeActivity
import com.google.gson.Gson
import kotlinx.android.synthetic.main.settings.move_check
import kotlinx.android.synthetic.main.settings.site
import kotlinx.android.synthetic.main.settings.use_categories
import kotlinx.android.synthetic.main.settings.version
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getShownDialogs

@RunWith(RobolectricTestRunner::class)
class SettingsActivityTest {
	@get:Rule
	val log = LogRule()

	private lateinit var mocker: ActivityMock
	private lateinit var activity: SettingsActivity

	@Before
	fun setup() {
		mocker = ActivityMock()
		activity = mocker.get<SettingsActivity>()
	}

	@Test
	fun structure() {
		activity.onCreate(null, null)

		assertNotNull(activity.findViewById(R.id.use_categories))
		assertNotNull(activity.findViewById(R.id.move_check))
		assertNotNull(activity.findViewById(R.id.site))
		assertNotNull(activity.findViewById(R.id.version))
	}

	@Test
	fun onCreateFromApi() {
		activity.simulateNetwork()
		mocker.server.enqueue("config_get")

		activity.onCreate(null, null)

		waitTasksFinish()
		val settings = activity.getPrivate<Settings>("settings")
		assertThat(settings, `is`(Settings(true, true)))

		assertTrue(activity.use_categories.isChecked)
		assertTrue(activity.move_check.isChecked)
	}

	@Test
	fun onCreateFromSavedState() {
		val saved = Bundle()
		saved.putString("settings", readBundle("settings"))

		activity.onCreate(saved, null)

		val settings = activity.getPrivate<Settings>("settings")
		assertThat(settings, `is`(Settings(true, true)))

		assertTrue(activity.use_categories.isChecked)
		assertTrue(activity.move_check.isChecked)
	}

	@Test
	fun onCreateSetControls() {
		val saved = Bundle()
		saved.putJson("settings", Settings())

		activity.onCreate(saved, null)

		val settings = activity.getPrivate<Settings>("settings")

		assertFalse(settings.useCategories)
		activity.use_categories.performClick()
		assertTrue(settings.useCategories)
		activity.use_categories.performClick()
		assertFalse(settings.useCategories)

		assertFalse(settings.moveCheck)
		activity.move_check.performClick()
		assertTrue(settings.moveCheck)
		activity.move_check.performClick()
		assertFalse(settings.moveCheck)
	}

	@Test
	fun onCreateSetFooterData() {
		val saved = Bundle()
		saved.putJson("settings", Settings())

		activity.onCreate(saved, null)

		val siteValue = activity.site.text.toString()
		assertNotNull(siteValue)
		assertTrue(siteValue.startsWith("http"))

		val versionValue = activity.version.text.toString()
		val versionRegex = Regex("\\d{1,2}\\.\\d{1,2}\\.\\d{1,2}\\.\\d{1,2}")
		assertTrue(versionRegex.matches(versionValue))
	}

	@Test
	fun onSaveInstance() {
		val originalSettings = Gson().fromJson(
			readBundle("settings"),
			Settings::class.java
		)

		val originalState = Bundle()
		originalState.putJson("settings", originalSettings)

		activity.onCreate(originalState, null)

		val newState = Bundle()
		activity.onSaveInstanceState(newState, PersistableBundle())

		val newSettings = newState.getFromJson("settings", Settings())
		assertThat(newSettings, `is`(originalSettings))
	}

	@Test
	fun saveSettings() {
		activity.simulateNetwork()
		mocker.server.enqueue("empty")

		val saved = Bundle()
		saved.putJson("settings", Settings())
		activity.intent.putExtra("__parent", WelcomeActivity::class.java)

		activity.onCreate(saved, null)

		activity.saveSettings(View(activity))

		val alert = getShownDialogs()
			.filterIsInstance<AlertDialog>()
			.last { it.isShowing }

		val shadowAlert = shadowOf(alert)
		val successMessage = activity.getString(R.string.settings_saved)
		assertThat(shadowAlert.message.toString(), `is`(successMessage))

		alert.getButton(Dialog.BUTTON_POSITIVE).performClick()

		val shadowActivity = shadowOf(activity)
		val intent = shadowActivity.peekNextStartedActivity()

		assertThat(intent.getActivityName(), `is`("WelcomeActivity"))
	}
}
