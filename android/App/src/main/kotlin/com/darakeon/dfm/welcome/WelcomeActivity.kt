package com.darakeon.dfm.welcome

import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.lib.extensions.applyGlyphicon
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.login.LoginActivity
import com.darakeon.dfm.moves.MovesService
import kotlinx.android.synthetic.main.welcome.action_close
import kotlinx.android.synthetic.main.welcome.action_home
import kotlinx.android.synthetic.main.welcome.action_logout
import kotlinx.android.synthetic.main.welcome.action_move
import kotlinx.android.synthetic.main.welcome.action_settings

class WelcomeActivity : BaseActivity() {
	override val contentView = R.layout.welcome

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (isLoggedIn) {
			action_home.applyGlyphicon()
			action_settings.applyGlyphicon()
			action_move.applyGlyphicon()
			action_logout.applyGlyphicon()
			action_close.applyGlyphicon()

			MovesService.start(this)

			populateCache()
		} else {
			redirect<LoginActivity>()
		}
	}
}
