package com.darakeon.dfm.welcome

import android.Manifest
import android.content.pm.PackageManager
import android.os.Build
import android.os.Bundle
import android.widget.Button
import androidx.core.content.ContextCompat
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.BottomMenuBinding
import com.darakeon.dfm.databinding.WelcomeBinding
import com.darakeon.dfm.lib.extensions.applyGlyphicon
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.login.LoginActivity
import com.darakeon.dfm.moves.MovesService
import com.google.firebase.messaging.FirebaseMessaging


class WelcomeActivity : BaseActivity<WelcomeBinding>() {
	override fun inflateBinding(): WelcomeBinding {
		return WelcomeBinding.inflate(layoutInflater)
	}
	override fun getMenuBinding(): BottomMenuBinding {
		return BottomMenuBinding.inflate(layoutInflater)
	}
	override fun getLogoutButton(): Button? {
		return binding.actionLogout
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

			FirebaseMessaging.getInstance().token.addOnCompleteListener { task ->
				if (task.isSuccessful) {
					val token = task.result
					//println(token)
				}
			}

			populateCache()
		} else {
			redirect<LoginActivity>()
		}
	}

	private val requestPermissionLauncher = registerForActivityResult(
		ActivityResultContracts.RequestPermission(),
	) { isGranted: Boolean ->
		if (isGranted) {
			// FCM SDK (and your app) can post notifications.
		} else {
			// TODO: Inform user that that your app will not show notifications.
		}
	}

	private fun askNotificationPermission() {
		// This is only necessary for API level >= 33 (TIRAMISU)
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
			if (ContextCompat.checkSelfPermission(this, Manifest.permission.POST_NOTIFICATIONS) ==
				PackageManager.PERMISSION_GRANTED
			) {
				// FCM SDK (and your app) can post notifications.
			} else if (shouldShowRequestPermissionRationale(Manifest.permission.POST_NOTIFICATIONS)) {
				// TODO: display an educational UI explaining to the user the features that will be enabled
				//       by them granting the POST_NOTIFICATION permission. This UI should provide the user
				//       "OK" and "No thanks" buttons. If the user selects "OK," directly request the permission.
				//       If the user selects "No thanks," allow the user to continue without notifications.
			} else {
				// Directly ask for the permission
				requestPermissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
			}
		}
	}
}
