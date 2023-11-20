package com.darakeon.dfm.settings

import android.app.AlertDialog
import android.app.Dialog
import android.os.Bundle
import android.os.PersistableBundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.SettingsBinding
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.settings.Settings
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.api.readBundle
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.api.ActivityMock
import com.darakeon.dfm.welcome.WelcomeActivity
import com.google.gson.Gson
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getShownDialogs

@RunWith(RobolectricTestRunner::class)
class SettingsActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<SettingsActivity>
	private lateinit var activity: SettingsActivity

	@Before
	fun setup() {
		mocker = ActivityMock(SettingsActivity::class)
		activity = mocker.get()
	}

	@After
	fun tearDown() {
		mocker.server.shutdown()
	}

	@Test
	fun structure() {
		val saved = Bundle()
		saved.putJson("settings", Settings())
		activity.onCreate(saved, null)

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
		activity.waitTasks(mocker.server)

		val binding = SettingsBinding.bind(
			shadowOf(activity).contentView
		)

		val settings = activity.getPrivate<Settings>("settings")
		assertThat(settings, `is`(Settings(true, true)))

		assertTrue(binding.useCategories.isChecked)
		assertTrue(binding.moveCheck.isChecked)
	}

	@Test
	fun onCreateFromSavedState() {
		val saved = Bundle()
		saved.putString("settings", readBundle("settings"))

		activity.onCreate(saved, null)

		val binding = SettingsBinding.bind(
			shadowOf(activity).contentView
		)

		val settings = activity.getPrivate<Settings>("settings")
		assertThat(settings, `is`(Settings(true, true)))

		assertTrue(binding.useCategories.isChecked)
		assertTrue(binding.moveCheck.isChecked)
	}

	@Test
	fun onCreateSetControls() {
		val saved = Bundle()
		saved.putJson("settings", Settings())

		activity.onCreate(saved, null)

		val binding = SettingsBinding.bind(
			shadowOf(activity).contentView
		)

		val settings = activity.getPrivate<Settings>("settings")

		assertFalse(settings.useCategories)
		binding.useCategories.performClick()
		assertTrue(settings.useCategories)
		binding.useCategories.performClick()
		assertFalse(settings.useCategories)

		assertFalse(settings.moveCheck)
		binding.moveCheck.performClick()
		assertTrue(settings.moveCheck)
		binding.moveCheck.performClick()
		assertFalse(settings.moveCheck)
	}

	@Test
	fun onCreateSetFooterData() {
		val saved = Bundle()
		saved.putJson("settings", Settings())

		activity.onCreate(saved, null)

		val binding = SettingsBinding.bind(
			shadowOf(activity).contentView
		)

		val siteValue = binding.site.text.toString()
		assertNotNull(siteValue)
		assertTrue(siteValue.startsWith("http"))

		val versionValue = binding.version.text.toString()
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

		val saved = Bundle()
		saved.putJson("settings", Settings())
		activity.intent.putExtra("__parent", WelcomeActivity::class.java)

		activity.onCreate(saved, null)

		mocker.server.enqueue("empty")
		activity.saveSettings(View(activity))
		activity.waitTasks(mocker.server)

		val alert = getShownDialogs()
			.filterIsInstance<AlertDialog>()
			.last { it.isShowing }

		val shadowAlert = shadowOf(alert)
		val successMessage = activity.getString(R.string.settings_saved)
		assertThat(shadowAlert.message.toString(), `is`(successMessage))

		alert.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		val shadowActivity = shadowOf(activity)
		val intent = shadowActivity.peekNextStartedActivity()

		assertThat(intent.getCalledName(), `is`("WelcomeActivity"))
	}

	@Test
	fun goToWipe() {
		val activity = mocker.create()

		val view = View(activity)
		activity.goToWipe(view)

		val called = shadowOf(activity)
			.peekNextStartedActivity()
			.getCalledName()

		assertThat(called, `is`("WipeActivity"))
	}
}
