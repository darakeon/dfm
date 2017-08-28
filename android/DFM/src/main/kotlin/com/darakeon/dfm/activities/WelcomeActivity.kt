package com.darakeon.dfm.activities

import android.os.Bundle
import android.widget.ImageView
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.License
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.objects.WelcomeStatic
import com.darakeon.dfm.api.Step
import org.json.JSONObject
import kotlin.reflect.KClass

class WelcomeActivity : SmartActivity<WelcomeStatic>(WelcomeStatic) {

    private val license: License get() = License(this)

    override fun HandleSuccess(data: JSONObject, step: Step) {

    }

    override fun contentView(): Int {
        return R.layout.welcome
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        if (intent.getBooleanExtra("EXIT", false)) {
            finish()
            return
        }

        license.Check()
    }

    fun GoToApp()
    {
        val nextActivity : KClass<*> =
            if (Authentication.IsLoggedIn())
                AccountsActivity::class
            else
                LoginActivity::class

        navigation.redirect(nextActivity.java)
    }

    fun KillThem()
    {
        val field = findViewById(R.id.pig) as ImageView
        field.setImageResource(R.drawable.unauthorized)
    }

    override fun onDestroy() {
        super.onDestroy()
        license.Destroy()
    }



}
