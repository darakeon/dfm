package com.darakeon.dfm.tfa

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.RemoveTfaBinding

class RemoveTFAActivity : BaseActivity<RemoveTfaBinding>() {
	override fun inflateBinding(): RemoveTfaBinding {
		return RemoveTfaBinding.inflate(layoutInflater)
	}

	fun remove(@Suppress("UNUSED_PARAMETER") view: View) {
		callApi {
			it.removeTFA(binding.password.text.toString()) {
				binding.password.text.clear()

				binding.successMessage.setText(
					R.string.remove_tfa_check_email
				)
			}
		}
	}
}
