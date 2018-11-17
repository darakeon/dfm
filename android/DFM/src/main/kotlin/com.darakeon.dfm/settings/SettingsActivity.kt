package com.darakeon.dfm.settings

import android.app.AlertDialog
import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.settings.Settings
import com.darakeon.dfm.api.old.InternalRequest
import com.darakeon.dfm.auth.auth
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.backWithExtras
import kotlinx.android.synthetic.main.settings.move_check
import kotlinx.android.synthetic.main.settings.use_categories

class SettingsActivity : BaseActivity<SettingsStatic>(SettingsStatic) {

	override val contentView = R.layout.settings


	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (rotated && static.succeeded) {
			this.populateScreen(static.settings)
		} else {
			api.getConfig(auth) {
				static.settings = it
				this.populateScreen(it)
			}
		}
	}

	private fun populateScreen(data: Settings) {
		use_categories.isChecked = data.useCategories
		move_check.isChecked = data.moveCheck

		use_categories.setOnCheckedChangeListener { _, isChecked ->
			data.useCategories = isChecked
		}

		move_check.setOnCheckedChangeListener { _, isChecked ->
			data.moveCheck = isChecked
		}
	}

	fun saveSettings(@Suppress(ON_CLICK) view: View) {
		api.saveConfig(auth, static.settings, this::back)
	}

	private fun back() {
		AlertDialog.Builder(this)
			.setTitle(R.string.title_activity_settings)
			.setMessage(R.string.settings_saved)
			.setPositiveButton(R.string.ok_button) { _, _ -> backWithExtras() }
			.show()
	}


}
