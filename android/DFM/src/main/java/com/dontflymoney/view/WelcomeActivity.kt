package com.dontflymoney.view

import com.dontflymoney.activityObjects.WelcomeStatic
import com.dontflymoney.api.Step
import com.dontflymoney.baseactivity.SmartActivity
import org.json.JSONObject

class WelcomeActivity : SmartActivity<WelcomeStatic>(WelcomeStatic) {

    override fun HandleSuccess(data: JSONObject, step: Step) {

    }

    override fun contentView(): Int {
        return R.layout.welcome
    }

}
