package com.dontflymoney.view

import android.app.AlertDialog
import android.content.DialogInterface
import android.content.DialogInterface.OnClickListener
import android.os.Bundle
import android.view.View
import android.widget.CheckBox

import com.dontflymoney.api.InternalRequest
import com.dontflymoney.api.Step
import com.dontflymoney.baseactivity.SmartActivity

import org.json.JSONException
import org.json.JSONObject

class SettingsActivity : SmartActivity() {
    internal var useCategories: Boolean = false
    internal var useCategoriesField: CheckBox

    internal var moveCheck: Boolean = false
    internal var moveCheckField: CheckBox


    override fun contentView(): Int {
        return R.layout.settings
    }


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        getFields()

        if (rotated && SmartActivity.succeded) {
            useCategoriesField.isChecked = useCategories
            moveCheckField.isChecked = moveCheck
        } else {
            getCurrentSettings()
        }
    }

    private fun getFields() {
        useCategoriesField = findViewById(R.id.use_categories) as CheckBox
        moveCheckField = findViewById(R.id.move_check) as CheckBox
    }

    private fun getCurrentSettings() {
        request = InternalRequest(this, "Users/GetConfig")
        request.AddParameter("ticket", Authentication.Get())
        request.Get(Step.Populate)
    }

    fun saveSettings(view: View) {
        request = InternalRequest(this, "Users/SaveConfig")
        request.AddParameter("ticket", Authentication.Get())
        request.AddParameter("UseCategories", useCategoriesField.isChecked)
        request.AddParameter("MoveCheck", moveCheckField.isChecked)
        request.Post(Step.Recording)
    }


    @Throws(JSONException::class)
    override fun HandleSuccess(data: JSONObject, step: Step) {
        when (step) {
            Step.Populate -> {
                populateScreen(data)
            }
            Step.Recording -> {
                back()
            }
            else -> {
                message.alertError(R.string.this_is_not_happening)
            }
        }

    }


    @Throws(JSONException::class)
    private fun populateScreen(data: JSONObject) {
        useCategories = data.getBoolean("UseCategories")
        useCategoriesField.isChecked = useCategories

        moveCheck = data.getBoolean("MoveCheck")
        moveCheckField.isChecked = moveCheck
    }


    private fun back() {
        AlertDialog.Builder(this)
                .setTitle(R.string.title_activity_settings)
                .setMessage(R.string.settings_saved)
                .setPositiveButton(R.string.ok_button) { dialog, which -> navigation.redirectWithExtras() }
                .show()
    }


}
