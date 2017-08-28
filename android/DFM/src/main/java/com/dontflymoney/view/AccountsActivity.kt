package com.dontflymoney.view

import android.os.Bundle
import android.view.View
import android.widget.ListView
import android.widget.TextView

import com.dontflymoney.adapters.AccountAdapter
import com.dontflymoney.api.InternalRequest
import com.dontflymoney.api.Step
import com.dontflymoney.baseactivity.SmartActivity

import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject

class AccountsActivity : SmartActivity() {
    internal var main: ListView
    internal var empty: TextView


    override fun contentView(): Int {
        return R.layout.accounts
    }


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setCurrentInfo()

        if (rotated && SmartActivity.succeded) {
            try {
                fillAccounts()
            } catch (e: JSONException) {
                message.alertError(R.string.error_activity_json, e)
            }

        } else {
            getAccounts()
        }
    }

    private fun setCurrentInfo() {
        main = findViewById(R.id.main_table) as ListView
        empty = findViewById(R.id.empty_list) as TextView
    }

    private fun getAccounts() {
        request = InternalRequest(this, "Accounts/List")
        request.AddParameter("ticket", Authentication.Get())
        request.Post()
    }

    @Throws(JSONException::class)
    override fun HandleSuccess(data: JSONObject, step: Step) {
        accountList = data.getJSONArray("AccountList")
        fillAccounts()
    }

    @Throws(JSONException::class)
    private fun fillAccounts() {
        if (accountList.length() == 0) {
            main.visibility = View.GONE
            empty.visibility = View.VISIBLE
        } else {
            main.visibility = View.VISIBLE
            empty.visibility = View.GONE

            val accountAdapter = AccountAdapter(this, accountList)
            main.adapter = accountAdapter
        }
    }

    companion object {

        internal var accountList: JSONArray
    }


}
