package com.dontflymoney.view

import android.app.DatePickerDialog
import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.EditText
import android.widget.LinearLayout
import android.widget.ScrollView

import com.dontflymoney.api.InternalRequest
import com.dontflymoney.api.Step
import com.dontflymoney.baseactivity.SmartActivity
import com.dontflymoney.dialogs.DialogAccountIn
import com.dontflymoney.dialogs.DialogAccountOut
import com.dontflymoney.entities.Detail
import com.dontflymoney.entities.Move
import com.dontflymoney.entities.Nature
import com.dontflymoney.listeners.DialogCategory
import com.dontflymoney.listeners.DialogNature
import com.dontflymoney.listeners.IDatePickerActivity
import com.dontflymoney.listeners.PickDate
import com.dontflymoney.viewhelper.DetailBox
import com.dontflymoney.watchers.DescriptionWatcher
import com.dontflymoney.watchers.ValueWatcher

import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject

import java.util.Calendar

class MovesCreateActivity : SmartActivity(), IDatePickerActivity {
    internal var dialog: DatePickerDialog
    internal var window: ScrollView


    override fun contentView(): Int {
        return R.layout.moves_create
    }


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        window = findViewById(R.id.window) as ScrollView

        if (rotated && SmartActivity.succeded) {
            try {
                populateCategoryAndNature()
                setControls()
                populateOldData(false)
            } catch (e: JSONException) {
                message.alertError(R.string.error_activity_json, e)
            }

        } else {
            move = Move()
            populateScreen()
        }
    }

    @Throws(NumberFormatException::class, JSONException::class)
    private fun populateOldData(populateAll: Boolean) {
        if (move != null) {
            if (useCategories) {
                setDataFromList(categoryList, move!!.Category, R.id.category)
            }

            form.setValue(R.id.date, move!!.DateString())

            for (n in 0..natureList.length() - 1) {
                val nature = natureList.getJSONObject(n)
                val comparValue = nature.getInt("Value")

                if (comparValue == move!!.Nature.GetNumber()) {
                    val text = nature.getString("Text")
                    val value = nature.getString("Value")
                    setNature(text, value)
                    break
                }
            }

            setDataFromList(accountList, move!!.AccountOut, R.id.account_out)
            setDataFromList(accountList, move!!.AccountIn, R.id.account_in)

            if (move!!.isDetailed) {
                for (d in move!!.Details.indices) {
                    val detail = move!!.Details[d]

                    addViewDetail(move, detail.Description, detail.Amount, detail.Value)
                }

                useDetailed()
            }

            if (!populateAll)
                return

            val descriptionView = findViewById(R.id.description) as EditText
            descriptionView.setText(move!!.Description)

            if (move!!.Value != 0.0) {
                val valueView = findViewById(R.id.value) as EditText
                valueView.setText(String.format("%1$,.2f", move!!.Value))
            }

        }
    }

    @Throws(JSONException::class)
    private fun setDataFromList(list: JSONArray, dataSaved: String, resourceId: Int) {
        for (n in 0..list.length() - 1) {
            val `object` = list.getJSONObject(n)
            val value = `object`.getString("Value")

            if (value == dataSaved) {
                val text = `object`.getString("Text")
                form.setValue(resourceId, text)
                break
            }
        }
    }

    private fun populateScreen() {
        request = InternalRequest(this, "Moves/Create")
        request.AddParameter("ticket", Authentication.Get())
        request.AddParameter("accountUrl", intent.getStringExtra("accountUrl"))
        request.AddParameter("id", intent.getIntExtra("id", 0))
        request.Get(Step.Populate)

        setCurrentDate()
        setControls()
    }

    @Throws(JSONException::class)
    override fun HandleSuccess(data: JSONObject, step: Step) {
        when (step) {
            Step.Populate -> {
                populateScreen(data)
            }
            Step.Recording -> {
                backToExtract()
            }
            else -> {
                message.alertError(R.string.this_is_not_happening)
            }
        }
    }

    @Throws(JSONException::class)
    private fun populateScreen(data: JSONObject) {
        useCategories = data.getBoolean("UseCategories")

        if (useCategories)
            categoryList = data.getJSONArray("CategoryList")

        natureList = data.getJSONArray("NatureList")
        accountList = data.getJSONArray("AccountList")

        populateCategoryAndNature()

        if (data.has("Move") && !data.isNull("Move")) {
            val moveToEdit = data.getJSONObject("Move")
            move!!.SetData(moveToEdit, intent.getStringExtra("accountUrl"))
            populateOldData(true)
        }
    }

    @Throws(JSONException::class)
    private fun populateCategoryAndNature() {
        if (useCategories) {
            findViewById(R.id.category_box).visibility = View.VISIBLE
        } else {
            findViewById(R.id.category_box).visibility = View.GONE
        }

        if (move!!.Nature == null) {
            val firstNature = natureList.getJSONObject(0)
            form.setValue(R.id.nature, firstNature.getString("Text"))
            move!!.SetNature(firstNature.getInt("Value"))
        }
    }


    private fun setCurrentDate() {
        val today = Calendar.getInstance()
        val day = intent.getIntExtra("day", today.get(Calendar.DAY_OF_MONTH))
        val month = intent.getIntExtra("month", today.get(Calendar.MONTH))
        val year = intent.getIntExtra("year", today.get(Calendar.YEAR))
        move!!.Date.set(year, month, day)
    }

    private fun setControls() {
        form.setValue(R.id.date, move!!.DateString())

        setDescriptionListener()
        setValueListener()
    }

    private fun setDescriptionListener() {
        val textMessage = findViewById(R.id.description) as EditText
        textMessage.addTextChangedListener(DescriptionWatcher(move))
    }

    private fun setValueListener() {
        val textMessage = findViewById(R.id.value) as EditText
        textMessage.addTextChangedListener(ValueWatcher(move))
    }


    fun showDatePicker(view: View) {
        dialog = DatePickerDialog(this, PickDate(this), move!!.year, move!!.month, move!!.day
        )

        dialog.show()
    }

    override fun setResult(year: Int, month: Int, day: Int) {
        move!!.Date.set(year, month, day)
        form.setValue(R.id.date, move!!.DateString())
    }

    override fun getDialog(): DatePickerDialog {
        return dialog
    }


    @Throws(JSONException::class)
    fun changeCategory(view: View) {
        form.showChangeList(categoryList, R.string.category, DialogCategory(categoryList, form, message, move))
    }

    @Throws(JSONException::class)
    fun changeNature(view: View) {
        form.showChangeList(natureList, R.string.nature, DialogNature(natureList, this))
    }


    fun setNature(text: String, value: String) {
        form.setValue(R.id.nature, text)
        move!!.SetNature(value)

        val accountOutVisibility = if (move!!.Nature != Nature.In) View.VISIBLE else View.GONE
        findViewById(R.id.account_out_block).visibility = accountOutVisibility

        val accountInVisibility = if (move!!.Nature != Nature.Out) View.VISIBLE else View.GONE
        findViewById(R.id.account_in_block).visibility = accountInVisibility

        if (move!!.Nature == Nature.Out) {
            move!!.AccountIn = null
            form.setValue(R.id.account_in, getString(R.string.pick))
        }

        if (move!!.Nature == Nature.In) {
            move!!.AccountOut = null
            form.setValue(R.id.account_out, getString(R.string.pick))
        }
    }


    @Throws(JSONException::class)
    fun changeAccountOut(view: View) {
        form.showChangeList(accountList, R.string.account, DialogAccountOut(accountList, form, message, move))
    }

    @Throws(JSONException::class)
    fun changeAccountIn(view: View) {
        form.showChangeList(accountList, R.string.account, DialogAccountIn(accountList, form, message, move))
    }

    @JvmOverloads fun useDetailed(view: View? = null) {
        move!!.isDetailed = true

        findViewById(R.id.simple_value).visibility = View.GONE
        findViewById(R.id.detailed_value).visibility = View.VISIBLE

        scrollToTheEnd()
    }

    fun useSimple(view: View) {
        move!!.isDetailed = false

        findViewById(R.id.simple_value).visibility = View.VISIBLE
        findViewById(R.id.detailed_value).visibility = View.GONE

        scrollToTheEnd()
    }

    fun addDetail(view: View) {
        val description = form.getValue(R.id.detail_description)
        val amountStr = form.getValue(R.id.detail_amount)
        val valueStr = form.getValue(R.id.detail_value)

        if (description.isEmpty() || amountStr.isEmpty() || valueStr.isEmpty()) {
            message.alertError(R.string.fill_all)
            return
        }

        val amountDefault = resources.getInteger(R.integer.amount_default)

        form.setValue(R.id.detail_description, "")
        form.setValue(R.id.detail_amount, amountDefault)
        form.setValue(R.id.detail_value, "")

        val amount = Integer.parseInt(amountStr)
        val value = java.lang.Double.parseDouble(valueStr)

        move!!.Add(description, amount, value)

        addViewDetail(move, description, amount, value)

        scrollToTheEnd()
    }

    private fun addViewDetail(move: Move, description: String, amount: Int, value: Double) {
        val row = DetailBox(this, move, description, amount, value)
        val list = findViewById(R.id.details) as LinearLayout
        list.addView(row)
    }


    fun save(view: View) {
        request = InternalRequest(this, "Moves/Create")

        request.AddParameter("ticket", Authentication.Get())
        move!!.setParameters(request)

        request.Post(Step.Recording)
    }

    private fun backToExtract() {
        val intent = Intent(this, ExtractActivity::class.java)
        intent.putExtra("accountUrl", getIntent().getStringExtra("accountUrl"))
        intent.putExtra("month", move!!.month)
        intent.putExtra("year", move!!.year)

        startActivity(intent)
    }


    private fun scrollToTheEnd() {
        window.postDelayed({ window.fullScroll(ScrollView.FOCUS_DOWN) }, 100)
    }

    companion object {

        internal var move: Move? = null
        internal var useCategories: Boolean = false
        internal var categoryList: JSONArray
        internal var natureList: JSONArray
        internal var accountList: JSONArray
    }


}

