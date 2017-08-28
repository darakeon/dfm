package com.dontflymoney.view

import android.annotation.SuppressLint
import android.app.DatePickerDialog
import android.content.Intent
import android.os.Bundle
import android.view.ContextMenu
import android.view.MenuItem
import android.view.View
import android.widget.DatePicker
import android.widget.ListView
import android.widget.TextView

import com.dontflymoney.adapters.MoveAdapter
import com.dontflymoney.api.InternalRequest
import com.dontflymoney.api.Step
import com.dontflymoney.baseactivity.IYesNoDialogAnswer
import com.dontflymoney.baseactivity.SmartActivity
import com.dontflymoney.layout.MoveLine
import com.dontflymoney.listeners.IDatePickerActivity
import com.dontflymoney.listeners.PickDate

import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject

import java.lang.reflect.Field
import java.text.SimpleDateFormat
import java.util.Calendar

class ExtractActivity : SmartActivity(), IYesNoDialogAnswer, IDatePickerActivity {
    internal var main: ListView
    internal var empty: TextView

    internal var accountUrl: String

    internal var dialog: DatePickerDialog? = null


    override fun contentView(): Int {
        return R.layout.extract
    }

    override fun optionsMenuResource(): Int {
        return R.menu.extract
    }

    override fun contextMenuResource(): Int {
        return R.menu.move_options
    }

    override fun viewWithContext(): Int {
        return R.id.main_table
    }


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setCurrentInfo()

        if (rotated && SmartActivity.succeded) {
            setDateFromLast()

            try {
                fillMoves()
            } catch (e: JSONException) {
                message.alertError(R.string.error_activity_json, e)
            }

        } else {
            setDateFromCaller()
            getExtract()
        }
    }

    private fun setCurrentInfo() {
        main = findViewById(R.id.main_table) as ListView
        empty = findViewById(R.id.empty_list) as TextView

        accountUrl = intent.getStringExtra("accountUrl")
    }

    private fun setDateFromLast() {
        setDate(month, year)
    }

    private fun setDateFromCaller() {
        val today = Calendar.getInstance()
        val startMonth = intent.getIntExtra("month", today.get(Calendar.MONTH))
        val startYear = intent.getIntExtra("year", today.get(Calendar.YEAR))
        setDate(startMonth, startYear)
    }

    @SuppressLint("SimpleDateFormat")
    private fun setDate(month: Int, year: Int) {
        ExtractActivity.month = month
        ExtractActivity.year = year

        val date = Calendar.getInstance()
        date.set(Calendar.MONTH, month)
        date.set(Calendar.YEAR, year)

        val formatter = SimpleDateFormat("MMM/yyyy")
        val dateInFull = formatter.format(date.time)

        form.setValue(R.id.reportDate, dateInFull)
    }

    fun changeDate(v: View) {
        if (dialog == null) {
            dialog = DatePickerDialog(this, PickDate(this), year, month, 1)

            try {
                val pickerField = dialog!!.javaClass.getDeclaredField("mDatePicker")
                pickerField.setAccessible(true)

                val field = pickerField.get(dialog).javaClass.getDeclaredField("mDaySpinner")
                field.setAccessible(true)
                val dayPicker = field.get(pickerField.get(dialog))
                (dayPicker as View).visibility = View.GONE
            } catch (ignored: Exception) {
            }

        }

        dialog!!.show()
    }

    override fun setResult(year: Int, month: Int, day: Int) {
        setDate(month, year)
        getExtract()
    }

    private fun getExtract() {
        request = InternalRequest(this, "Moves/Extract")

        request.AddParameter("ticket", Authentication.Get())
        request.AddParameter("accountUrl", accountUrl)
        request.AddParameter("id", year * 100 + month + 1)

        request.Post(Step.Populate)
    }

    override fun getDialog(): DatePickerDialog {
        return dialog
    }

    @Throws(JSONException::class)
    override fun HandleSuccess(data: JSONObject, step: Step) {
        when (step) {
            Step.Populate -> {
                moveList = data.getJSONArray("MoveList")
                name = data.getString("Name")
                total = data.getDouble("Total")
                canCheck = data.getBoolean("CanCheck")

                fillMoves()
            }
            Step.Recording -> {
                refresh()
            }
            else -> {
                message.alertError(R.string.this_is_not_happening)
            }
        }
    }

    @Throws(JSONException::class)
    private fun fillMoves() {
        form.setValue(R.id.totalTitle, name)
        form.setValueColored(R.id.totalValue, total)

        if (moveList.length() == 0) {
            main.visibility = View.GONE
            empty.visibility = View.VISIBLE
        } else {
            main.visibility = View.VISIBLE
            empty.visibility = View.GONE

            val accountAdapter = MoveAdapter(this, moveList, canCheck)
            main.adapter = accountAdapter
        }
    }


    fun goToSummary(item: MenuItem) {
        val intent = Intent(this, SummaryActivity::class.java)

        intent.putExtra("accountUrl", accountUrl)
        intent.putExtra("year", year)

        startActivity(intent)
    }

    fun goToMove(item: MenuItem) {
        goToMove(0)
    }

    private fun goToMove(moveId: Int) {
        val intent = Intent(this, MovesCreateActivity::class.java)

        intent.putExtra("id", moveId)
        intent.putExtra("accountUrl", accountUrl)
        intent.putExtra("year", year)
        intent.putExtra("month", month)

        startActivity(intent)
    }


    override fun onContextItemSelected(item: MenuItem): Boolean {
        when (item.itemId) {
            R.id.edit_move -> {
                edit()
                return true
            }
            R.id.delete_move -> {
                askDelete()
                return true
            }
            R.id.check_move -> {
                check()
                return true
            }
            R.id.uncheck_move -> {
                uncheck()
                return true
            }
            else -> return super.onContextItemSelected(item)
        }
    }

    private fun edit() {
        val moveLine = clickedView as MoveLine
        goToMove(moveLine.id)
    }


    fun askDelete(): Boolean {
        var messageText = getString(R.string.sure_to_delete)
        val moveRow = clickedView as MoveLine

        messageText = String.format(messageText, moveRow.description)

        message.alertYesNo(messageText, this)

        return false
    }

    override fun YesAction() {
        submitMoveAction("Delete")
    }

    override fun NoAction() {}


    private fun check() {
        submitMoveAction("Check")
    }

    private fun uncheck() {
        submitMoveAction("Uncheck")
    }


    private fun submitMoveAction(action: String) {
        request = InternalRequest(this, "Moves/" + action)

        val view = clickedView as MoveLine

        request.AddParameter("ticket", Authentication.Get())
        request.AddParameter("accountUrl", accountUrl)
        request.AddParameter("id", view.id)

        request.Post(Step.Recording)
    }


    public override fun changeContextMenu(view: View, menu: ContextMenu) {
        val move = clickedView as MoveLine

        try {
            if (canCheck) {
                hideMenuItem(menu, if (move.isChecked) R.id.check_move else R.id.uncheck_move)
                showMenuItem(menu, if (move.isChecked) R.id.uncheck_move else R.id.check_move)
            } else {
                hideMenuItem(menu, R.id.check_move)
                hideMenuItem(menu, R.id.uncheck_move)
            }
        } catch (e: Exception) {
            e.printStackTrace()
        }

    }

    fun hideMenuItem(menu: ContextMenu, id: Int) {
        toggleMenuItem(menu, id, false)
    }

    fun showMenuItem(menu: ContextMenu, id: Int) {
        toggleMenuItem(menu, id, true)
    }

    fun toggleMenuItem(menu: ContextMenu, id: Int, show: Boolean) {
        val buttonToHide = menu.findItem(id)
        buttonToHide.isVisible = show
    }

    companion object {

        internal var moveList: JSONArray
        internal var name: String
        internal var total: Double = 0.toDouble()
        internal var canCheck: Boolean = false
        private var month: Int = 0
        private var year: Int = 0
    }

}

