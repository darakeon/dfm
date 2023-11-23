package com.darakeon.dfm.settings

import android.app.AlertDialog
import android.os.Bundle
import android.view.View
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.BottomMenuBinding
import com.darakeon.dfm.databinding.SettingsBinding
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.backWithExtras
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.goToTerms
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.MainInfo
import com.darakeon.dfm.lib.api.entities.settings.Settings
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.wipe.WipeActivity

class SettingsActivity : BaseActivity<SettingsBinding>() {
	override fun inflateBinding(): SettingsBinding {
		return SettingsBinding.inflate(layoutInflater)
	}
	override fun getMenuBinding(): BottomMenuBinding {
		return binding.bottomMenu
	}

	override val title = R.string.title_activity_settings

	private var settings = Settings()
	private val settingsKey = "settings"

	override val refresh: SwipeRefreshLayout
		get() = binding.main

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (savedInstanceState == null) {
			callApi { api ->
				api.getSettings {
					this.populateScreen(it)
				}
			}
		} else {
			this.populateScreen(
				savedInstanceState.getFromJson(settingsKey, Settings())
			)
		}

		setControls()

		binding.site.text = MainInfo.getSiteUrl(this)
		binding.version.text = MainInfo.getAppVersion(this)
	}

	private fun populateScreen(data: Settings) {
		settings = data

		binding.useCategories.isChecked = data.useCategories
		binding.moveCheck.isChecked = data.moveCheck
	}

	private fun setControls() {
		binding.useCategories.setOnCheckedChangeListener { _, isChecked ->
			settings.useCategories = isChecked
		}

		binding.moveCheck.setOnCheckedChangeListener { _, isChecked ->
			settings.moveCheck = isChecked
		}
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)
		outState.putJson(settingsKey, settings)
	}

	fun saveSettings(@Suppress(ON_CLICK) view: View) {
		callApi {
			it.saveSettings(settings, this::back)
		}
	}

	fun goToWipe(@Suppress(ON_CLICK) view: View) {
		redirect<WipeActivity>()
	}

	fun goToTerms(@Suppress(ON_CLICK) view: View) {
		goToTerms()
	}

	private fun back() {
		val alert = AlertDialog.Builder(this)
			.setTitle(R.string.title_activity_settings)
			.setMessage(R.string.settings_saved)
			.setPositiveButton(R.string.ok_button) {
				_, _ -> backWithExtras()
			}

		runOnUiThread { alert.show() }
	}
}
