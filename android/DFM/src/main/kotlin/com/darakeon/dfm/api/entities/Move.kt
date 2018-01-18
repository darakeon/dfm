package com.darakeon.dfm.api.entities

import com.darakeon.dfm.activities.objects.MovesCreateStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.api.getCalendar
import org.json.JSONObject
import java.text.SimpleDateFormat
import java.util.*

class Move {

	private var id: Int = 0
	var description: String? = null
	var date: Calendar = Calendar.getInstance()

	var nature: Nature? = null

	var warnCategory: Boolean = false
	var category: String? = null
	var accountOut: String? = null
	var accountIn: String? = null

	var isDetailed: Boolean = false
	var value: Double = 0.toDouble()
	var details: MutableList<Detail> = ArrayList()


	private fun getNature(number: Int?): Nature =
		com.darakeon.dfm.api.entities.Nature.getNature(number)


	fun setNature(number: String) {
		setNature(number.toInt())
	}

	fun setNature(number: Int?) {
		nature = getNature(number)
	}

	fun add(description: String, amount: Int, value: Double) {
		val detail = Detail()

		detail.description = description
		detail.amount = amount
		detail.value = value

		details.add(detail)
	}

	fun remove(description: String?, amount: Int, value: Double) {
		for (detail in details) {
			if (detail.equals(description, amount, value)) {
				details.remove(detail)
				return
			}
		}
	}


	val day: Int
		get() = date.get(Calendar.DAY_OF_MONTH)

	val month: Int
		get() = date.get(Calendar.MONTH)

	val year: Int
		get() = date.get(Calendar.YEAR)

	fun setParameters(request: InternalRequest<MovesCreateStatic>) {
		request.addParameter("ID", id)
		request.addParameter("Description", description)

		request.addParameter("Date.Year", date.get(Calendar.YEAR))
		request.addParameter("Date.Month", date.get(Calendar.MONTH) + 1)
		request.addParameter("Date.Day", date.get(Calendar.DAY_OF_MONTH))

		request.addParameter("Nature", nature)

		request.addParameter("Category", category)
		request.addParameter("AccountOutUrl", accountOut)
		request.addParameter("AccountInUrl", accountIn)

		if (isDetailed) {
			for (detail in details) {
				val position = details.lastIndexOf(detail)

				request.addParameter("DetailList[$position].Description", detail.description!!)
				request.addParameter("DetailList[$position].Amount", detail.amount)
				request.addParameter("DetailList[$position].Value", detail.value)
			}
		} else {
			request.addParameter("Value", value)
		}
	}

	fun dateString(): String {
		val formatter = SimpleDateFormat("dd/MM/yyyy", Locale.getDefault())

		return formatter.format(date.time)
	}


	fun setData(move: JSONObject, activityAccountUrl: String?, useCategories: Boolean) {
		id = move.getInt("ID")

		if (id == 0) {
			accountOut = activityAccountUrl
			accountIn = activityAccountUrl
		} else {
			setEditMoveData(move, useCategories)
		}
	}

	private fun setEditMoveData(move: JSONObject, useCategories: Boolean) {
		description = move.getString("Description")
		date = move.getCalendar("Date")

		if (useCategories) {
			category = move.getString("Category")
		} else if (move.has("Category")) {
			warnCategory = true
		}

		accountOut = move.getString("AccountOutUrl")
		accountIn = move.getString("AccountInUrl")
		nature = getNature(move.getInt("Nature"))

		if (move.has("Value") && !move.isNull("Value")) {
			value = move.getDouble("Value")
		}

		if (move.has("DetailList")) {
			val detailList = move.getJSONArray("DetailList")

			isDetailed = detailList.length() > 0

			(0 until detailList.length())
				.map { detailList.getJSONObject(it) }
				.forEach { add(
					it.getString("Description"),
					it.getInt("Amount"),
					it.getDouble("Value")
				) }
		}
	}

	// Kotlin, shame on you
	operator fun component1(): Int = year
	operator fun component2(): Int = month
	operator fun component3(): Int = day
}
