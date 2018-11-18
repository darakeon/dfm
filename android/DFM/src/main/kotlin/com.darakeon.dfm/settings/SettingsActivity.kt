package com.darakeon.dfm.settings

import android.app.AlertDialog
import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.settings.Settings
import com.darakeon.dfm.api.old.DELETE
import com.darakeon.dfm.auth.auth
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.backWithExtras
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import kotlinx.android.synthetic.main.settings.move_check
import kotlinx.android.synthetic.main.settings.use_categories

class SettingsActivity : BaseActivity<DELETE>(DELETE) {

	override val contentView = R.layout.settings

	private var settings = Settings()
	private val settingsKey = "settings"

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (savedInstanceState == null) {
			api.getConfig(auth) {
				this.populateScreen(it)
			}
		} else {
			this.populateScreen(
				savedInstanceState.getFromJson(settingsKey, Settings())
			)
		}

		setControls()
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
		api.saveConfig(auth, settings, this::back)
	}

	private fun back() {
		AlertDialog.Builder(this)
			.setTitle(R.string.title_activity_settings)
			.setMessage(R.string.settings_saved)
			.setPositiveButton(R.string.ok_button) { _, _ -> backWithExtras() }
			.show()
	}


}
