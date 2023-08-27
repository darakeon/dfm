package com.darakeon.dfm.welcome

import android.os.Bundle
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.WelcomeBinding
import com.darakeon.dfm.lib.extensions.applyGlyphicon
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.login.LoginActivity
import com.darakeon.dfm.moves.MovesService

class WelcomeActivity : BaseActivity<WelcomeBinding>() {
	override fun inflateBinding(): WelcomeBinding {
		return WelcomeBinding.inflate(layoutInflater)
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (isLoggedIn) {
			binding.actionHome.applyGlyphicon()
			binding.actionSettings.applyGlyphicon()
			binding.actionMove.applyGlyphicon()
			binding.actionLogout.applyGlyphicon()
			binding.actionClose.applyGlyphicon()

			MovesService.start(this)

			populateCache()
		} else {
			redirect<LoginActivity>()
		}
	}
}
