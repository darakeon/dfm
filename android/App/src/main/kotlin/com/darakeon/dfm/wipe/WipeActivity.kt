package com.darakeon.dfm.wipe

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.BottomMenuBinding
import com.darakeon.dfm.databinding.WipeBinding
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.lib.api.entities.wipe.Wipe

class WipeActivity : BaseActivity<WipeBinding>() {
	override fun inflateBinding(): WipeBinding {
		return WipeBinding.inflate(layoutInflater)
	}
	override fun getMenuBinding(): BottomMenuBinding {
		return binding.bottomMenu
	}

	override val title = R.string.wipe_title

	fun wipe(@Suppress(ON_CLICK) view: View) {
		callApi {
			it.wipe(
				Wipe(binding.password.text.toString())
			) {
				this.logout()
			}
		}
	}
}
