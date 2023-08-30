package com.darakeon.dfm.tfa

import android.view.View
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.TfaBinding
import com.darakeon.dfm.lib.extensions.redirect

class TFAActivity : BaseActivity<TfaBinding>() {
	override fun inflateBinding(): TfaBinding {
		return TfaBinding.inflate(layoutInflater)
	}

	fun verify(@Suppress("UNUSED_PARAMETER") view: View) {
		callApi {
			it.validateTFA(binding.code.text.toString()) {
				redirect<AccountsActivity>()
			}
		}
	}
}
