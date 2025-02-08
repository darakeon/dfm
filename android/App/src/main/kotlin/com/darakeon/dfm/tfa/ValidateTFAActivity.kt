package com.darakeon.dfm.tfa

import android.view.View
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.ValidateTfaBinding
import com.darakeon.dfm.lib.extensions.redirect

class ValidateTFAActivity : BaseActivity<ValidateTfaBinding>() {
	override fun inflateBinding(): ValidateTfaBinding {
		return ValidateTfaBinding.inflate(layoutInflater)
	}

	fun verify(@Suppress("UNUSED_PARAMETER") view: View) {
		callApi {
			it.validateTFA(binding.code.text.toString()) {
				redirect<AccountsActivity>()
			}
		}
	}

	fun goToRemove(@Suppress("UNUSED_PARAMETER") view: View) {
		redirect<RemoveTFAActivity>()
	}
}
