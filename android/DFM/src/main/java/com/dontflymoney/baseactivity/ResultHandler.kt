package com.dontflymoney.baseactivity

import com.dontflymoney.activityObjects.SmartStatic
import com.dontflymoney.api.Step
import com.dontflymoney.userdata.Language
import com.dontflymoney.view.R

import org.json.JSONException
import org.json.JSONObject

internal class ResultHandler<T : SmartStatic>(private val activity: SmartActivity<T>, private val navigation: Navigation<T>) {

    fun HandlePostResult(result: JSONObject, step: Step) {
        try {
            if (result.has("error")) {
                val error = result.getString("error")

                activity.message.alertError(error)

                if (error.contains(activity.getString(R.string.uninvited)) || error.contains("uninvited")) {
                    navigation.logout()
                }
            } else {
                val data = result.getJSONObject("data")

                if (data.has("Language"))
                    Language.ChangeAndSave(activity, data.getString("Language"))

                activity.HandleSuccess(data, step)
            }
        } catch (e: JSONException) {
            activity.message.alertError(R.string.error_activity_json, e)
        }

    }

    fun HandlePostError(error: String, step: Step) {
        activity.message.alertError(error)
    }


}
