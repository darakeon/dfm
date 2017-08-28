package com.dontflymoney.view

import android.os.Bundle
import android.view.View
import android.widget.ListView
import android.widget.TextView
import com.dontflymoney.activityObjects.AccountsStatic
import com.dontflymoney.adapters.AccountAdapter
import com.dontflymoney.api.InternalRequest
import com.dontflymoney.api.Step
import com.dontflymoney.baseactivity.SmartActivity
import org.json.JSONException
import org.json.JSONObject

class AccountsActivity : SmartActivity<AccountsStatic>(AccountsStatic) {
    internal val main: ListView get() = findViewById(R.id.main_table) as ListView
    internal val empty: TextView get() = findViewById(R.id.empty_list) as TextView


    override fun contentView(): Int {
        return R.layout.accounts
    }


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        if (rotated && succeeded) {
            try {
                fillAccounts()
            } catch (e: JSONException) {
                message.alertError(R.string.error_activity_json, e)
            }

        } else {
            getAccounts()
        }
    }

    private fun getAccounts() {
        val request = InternalRequest(this, "Accounts/List")
        request.AddParameter("ticket", Authentication.Get())
        request.Post()
    }

    @Throws(JSONException::class)
    override fun HandleSuccess(data: JSONObject, step: Step) {
        static.accountList = data.getJSONArray("AccountList")
        fillAccounts()
    }

    @Throws(JSONException::class)
    private fun fillAccounts() {
        if (static.accountList.length() == 0) {
            main.visibility = View.GONE
            empty.visibility = View.VISIBLE
        } else {
            main.visibility = View.VISIBLE
            empty.visibility = View.GONE

            val accountAdapter = AccountAdapter(this, static.accountList)
            main.adapter = accountAdapter
        }
    }


}
