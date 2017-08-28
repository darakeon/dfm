package com.dontflymoney.entities

import com.dontflymoney.activityObjects.MovesCreateStatic
import com.dontflymoney.api.InternalRequest
import com.dontflymoney.viewhelper.DateTime
import org.json.JSONException
import org.json.JSONObject
import java.text.SimpleDateFormat
import java.util.*

class Move {

    private var Id: Int = 0
    var Description: String? = null
    var Date: Calendar = Calendar.getInstance()

    var Nature: Nature? = null

    var Category: String? = null
    var AccountOut: String? = null
    var AccountIn: String? = null

    var isDetailed: Boolean = false
    var Value: Double = 0.toDouble()
    var Details: MutableList<Detail> = ArrayList<Detail>()


    private fun getNature(number: Int?): Nature {
        return com.dontflymoney.entities.Nature.GetNature(number)
    }


    fun SetNature(number: String) {
        SetNature(Integer.parseInt(number))
    }

    fun SetNature(number: Int?) {
        Nature = getNature(number)
    }

    fun Add(description: String, amount: Int, value: Double) {
        val detail = Detail()

        detail.Description = description
        detail.Amount = amount
        detail.Value = value

        Details.add(detail)
    }

    fun Remove(description: String?, amount: Int, value: Double) {
        for (detail in Details) {
            if (detail.Equals(description, amount, value)) {
                Details.remove(detail)
                return
            }
        }
    }


    val day: Int
        get() = Date.get(Calendar.DAY_OF_MONTH)

    val month: Int
        get() = Date.get(Calendar.MONTH)

    val year: Int
        get() = Date.get(Calendar.YEAR)

    fun setParameters(request: InternalRequest<MovesCreateStatic>) {
        request.AddParameter("ID", Id)
        request.AddParameter("Description", Description)

        request.AddParameter("Date.Year", Date.get(Calendar.YEAR))
        request.AddParameter("Date.Month", Date.get(Calendar.MONTH) + 1)
        request.AddParameter("Date.Day", Date.get(Calendar.DAY_OF_MONTH))

        request.AddParameter("Nature", Nature)

        request.AddParameter("Category", Category)
        request.AddParameter("AccountOutUrl", AccountOut)
        request.AddParameter("AccountInUrl", AccountIn)

        if (isDetailed) {
            for (detail in Details) {
                val position = Details.lastIndexOf(detail)

                request.AddParameter("DetailList[$position].Description", detail.Description!!)
                request.AddParameter("DetailList[$position].Amount", detail.Amount)
                request.AddParameter("DetailList[$position].Value", detail.Value)
            }
        } else {
            request.AddParameter("Value", Value)
        }
    }

    fun DateString(): String {
        val formatter = SimpleDateFormat("dd/MM/yyyy", Locale.getDefault())

        return formatter.format(Date.time)
    }


    @Throws(JSONException::class)
    fun SetData(move: JSONObject, activityAccountUrl: String) {
        Id = move.getInt("ID")

        if (Id == 0) {
            AccountOut = activityAccountUrl
        } else {
            setEditMoveData(move)
        }
    }

    @Throws(JSONException::class)
    private fun setEditMoveData(move: JSONObject) {
        Description = move.getString("Description")
        Date = DateTime.getCalendar(move.getJSONObject("Date"))
        Category = move.getString("Category")
        AccountOut = move.getString("AccountOutUrl")
        AccountIn = move.getString("AccountInUrl")
        Nature = getNature(move.getInt("Nature"))

        if (move.has("Value") && !move.isNull("Value")) {
            Value = move.getDouble("Value")
        }

        if (move.has("DetailList")) {
            val detailList = move.getJSONArray("DetailList")

            isDetailed = detailList.length() > 0

            for (d in 0..detailList.length() - 1) {
                val detail = detailList.getJSONObject(d)
                Add(detail.getString("Description"), detail.getInt("Amount"), detail.getDouble("Value"))
            }
        }
    }


}
