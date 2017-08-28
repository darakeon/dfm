package com.dontflymoney.adapters

import android.annotation.SuppressLint
import android.content.Context
import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter

import com.dontflymoney.layout.AccountLine
import com.dontflymoney.view.R

import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject

import java.util.ArrayList

class AccountAdapter @Throws(JSONException::class)
constructor(context: Context, accountJsonList: JSONArray) : BaseAdapter() {
    private val accountList: MutableList<Account>

    init {
        accountList = ArrayList<Account>()

        for (a in 0..accountJsonList.length() - 1) {
            val account = Account(accountJsonList.getJSONObject(a))
            accountList.add(account)
        }

        inflater = context.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
    }

    inner class Account @Throws(JSONException::class)
    internal constructor(jsonObject: JSONObject) {
        var Name: String
        var Total: Double = 0.toDouble()
        var Url: String

        init {
            Name = jsonObject.getString("Name")
            Total = jsonObject.getDouble("Total")
            Url = jsonObject.getString("Url")
        }
    }

    override fun getCount(): Int {
        return accountList.size
    }

    override fun getItem(position: Int): Any {
        return position
    }

    override fun getItemId(position: Int): Long {
        return position.toLong()
    }

    override fun getView(position: Int, view: View, viewGroup: ViewGroup): View {
        @SuppressLint("ViewHolder", "InflateParams")
        val line = inflater!!.inflate(R.layout.accounts_line, null) as AccountLine

        try {
            val color = if (position % 2 == 0) Color.TRANSPARENT else Color.LTGRAY
            line.setAccount(accountList[position], color)
        } catch (e: JSONException) {
            e.printStackTrace()
        }

        return line
    }

    companion object {
        private var inflater: LayoutInflater? = null
    }

}
