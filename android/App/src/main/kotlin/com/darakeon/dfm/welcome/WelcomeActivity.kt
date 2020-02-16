package com.darakeon.dfm.welcome

import android.os.Bundle
import androidx.compose.unaryPlus
import androidx.ui.core.Text
import androidx.ui.core.setContent
import androidx.ui.foundation.DrawImage
import androidx.ui.graphics.Color
import androidx.ui.layout.Container
import androidx.ui.res.imageResource
import androidx.ui.res.stringResource
import androidx.ui.text.TextStyle
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.lib.extensions.applyGlyphicon
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.login.LoginActivity
import kotlinx.android.synthetic.main.welcome.action_close
import kotlinx.android.synthetic.main.welcome.action_home
import kotlinx.android.synthetic.main.welcome.action_logout
import kotlinx.android.synthetic.main.welcome.action_move
import kotlinx.android.synthetic.main.welcome.action_settings

class WelcomeActivity : BaseActivity() {
	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (isLoggedIn) {

			setContent {
				val image = +imageResource(R.drawable.pigs)
				var descriptor = +stringResource(R.string.starting_app)
				Container {
					DrawImage(image = image)
					Text(descriptor, style = TextStyle(Color.Transparent))
				}
			}

			action_home.applyGlyphicon()
			action_settings.applyGlyphicon()
			action_move.applyGlyphicon()
			action_logout.applyGlyphicon()
			action_close.applyGlyphicon()
		} else {
			redirect<LoginActivity>()
		}
	}
}
