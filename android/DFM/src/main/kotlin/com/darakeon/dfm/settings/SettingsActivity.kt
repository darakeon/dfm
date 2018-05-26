package com.darakeon.dfm.settings

import android.app.AlertDialog
import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.auth.getAuth
import com.darakeon.dfm.base.SmartActivity
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.redirectWithExtras
import kotlinx.android.synthetic.main.settings.move_check
import kotlinx.android.synthetic.main.settings.use_categories
import org.json.JSONObject

class SettingsActivity : SmartActivity<SettingsStatic>(SettingsStatic) {

	override fun contentView(): Int = R.layout.settings


	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (rotated && succeeded) {
			use_categories.isChecked = static.useCategories
			move_check.isChecked = static.moveCheck
		} else {
			getCurrentSettings()
		}
	}

	private fun getCurrentSettings() {
		val request = InternalRequest(
			this, "Users/GetConfig", { d -> populateScreen(d) }
		)
		request.addParameter("ticket", getAuth())
		request.get()
	}

	fun saveSettings(@Suppress(ON_CLICK) view: View) {
		val request = InternalRequest(
			this, "Users/SaveConfig", { back() }
		)
		request.addParameter("ticket", getAuth())
		request.addParameter("UseCategories", use_categories.isChecked)
		request.addParameter("MoveCheck", move_check.isChecked)
		request.post()
	}


	private fun populateScreen(data: JSONObject) {
		static.useCategories = data.getBoolean("UseCategories")
		use_categories.isChecked = static.useCategories

		static.moveCheck = data.getBoolean("MoveCheck")
		move_check.isChecked = static.moveCheck
	}


	private fun back() {
		AlertDialog.Builder(this)
			.setTitle(R.string.title_activity_settings)
			.setMessage(R.string.settings_saved)
			.setPositiveButton(R.string.ok_button) { _, _ -> redirectWithExtras() }
			.show()
	}


}
