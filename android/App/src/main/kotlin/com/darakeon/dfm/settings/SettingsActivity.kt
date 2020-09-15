package com.darakeon.dfm.settings

import android.app.AlertDialog
import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.backWithExtras
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.MainInfo
import com.darakeon.dfm.lib.api.entities.settings.Settings
import kotlinx.android.synthetic.main.settings.move_check
import kotlinx.android.synthetic.main.settings.site
import kotlinx.android.synthetic.main.settings.use_categories
import kotlinx.android.synthetic.main.settings.version

class SettingsActivity : BaseActivity() {
	override val contentView = R.layout.settings
	override val title = R.string.title_activity_settings

	private var settings = Settings()
	private val settingsKey = "settings"

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (savedInstanceState == null) {
			callApi { api ->
				api.getConfig {
					this.populateScreen(it)
				}
			}
		} else {
			this.populateScreen(
				savedInstanceState.getFromJson(settingsKey, Settings())
			)
		}

		setControls()

		site.text = MainInfo.getSiteUrl(this)
		version.text = MainInfo.getAppVersion(this)
	}

	private fun populateScreen(data: Settings) {
		settings = data

		use_categories.isChecked = data.useCategories
		move_check.isChecked = data.moveCheck
	}

	private fun setControls() {
		use_categories.setOnCheckedChangeListener { _, isChecked ->
			settings.useCategories = isChecked
		}

		move_check.setOnCheckedChangeListener { _, isChecked ->
			settings.moveCheck = isChecked
		}
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)
		outState.putJson(settingsKey, settings)
	}

	fun saveSettings(@Suppress(ON_CLICK) view: View) {
		callApi {
			it.saveConfig(settings, this::back)
		}
	}

	private fun back() {
		AlertDialog.Builder(this)
			.setTitle(R.string.title_activity_settings)
			.setMessage(R.string.settings_saved)
			.setPositiveButton(R.string.ok_button) { _, _ -> backWithExtras() }
			.show()
	}
}
