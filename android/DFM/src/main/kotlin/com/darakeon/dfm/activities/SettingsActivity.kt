package com.darakeon.dfm.activities

import android.app.AlertDialog
import android.os.Bundle
import android.view.View
import android.widget.CheckBox
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.objects.SettingsStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.api.Step
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.onClick
import org.json.JSONObject

class SettingsActivity : SmartActivity<SettingsStatic>(SettingsStatic) {
	internal var useCategories: Boolean = false
	internal val useCategoriesField: CheckBox get() = findViewById(R.id.use_categories)

	internal var moveCheck: Boolean = false
	internal val moveCheckField: CheckBox get() = findViewById(R.id.move_check)


	override fun contentView(): Int {
		return R.layout.settings
	}


	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (rotated && succeeded) {
			useCategoriesField.isChecked = useCategories
			moveCheckField.isChecked = moveCheck
		} else {
			getCurrentSettings()
		}
	}

	private fun getCurrentSettings() {
		val request = InternalRequest(this, "Users/GetConfig")
		request.AddParameter("ticket", Authentication.Get())
		request.Get(Step.Populate)
	}

	fun saveSettings(@Suppress(onClick) view: View) {
		val request = InternalRequest(this, "Users/SaveConfig")
		request.AddParameter("ticket", Authentication.Get())
		request.AddParameter("UseCategories", useCategoriesField.isChecked)
		request.AddParameter("MoveCheck", moveCheckField.isChecked)
		request.Post(Step.Recording)
	}


	override fun HandleSuccess(data: JSONObject, step: Step) {
		when (step) {
			Step.Populate -> {
				populateScreen(data)
			}
			Step.Recording -> {
				back()
			}
			else -> {
				message.alertError(R.string.this_is_not_happening)
			}
		}

	}


	private fun populateScreen(data: JSONObject) {
		useCategories = data.getBoolean("UseCategories")
		useCategoriesField.isChecked = useCategories

		moveCheck = data.getBoolean("MoveCheck")
		moveCheckField.isChecked = moveCheck
	}


	private fun back() {
		AlertDialog.Builder(this)
				.setTitle(R.string.title_activity_settings)
				.setMessage(R.string.settings_saved)
				.setPositiveButton(R.string.ok_button) { _, _ -> navigation.redirectWithExtras() }
				.show()
	}


}
