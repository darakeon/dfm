package com.darakeon.dfm.api.entities.moves

import com.darakeon.dfm.api.entities.Date
import com.darakeon.dfm.api.old.InternalRequest
import com.darakeon.dfm.extensions.toDoubleByCulture
import com.darakeon.dfm.moves.MovesCreateStatic
import com.google.gson.annotations.SerializedName
import java.util.ArrayList

class Move {
	@SerializedName("ID")
	var id: Int = 0

	@SerializedName("Description")
	var description: String? = null

	@SerializedName("Date")
	var date: Date = Date()

	@SerializedName("Nature")
	var natureValue: Int? = null
	var nature
		get() = Nature.get(natureValue)
		set(value) { natureValue = value?.value }

	@SerializedName("WarnCategory")
	var warnCategory: Boolean = false

	@SerializedName("Category")
	var category: String? = null

	@SerializedName("AccountOutUrl")
	var accountOut: String? = null

	@SerializedName("AccountInUrl")
	var accountIn: String? = null

	@SerializedName("Value")
	var value: Double = 0.0

	@SerializedName("DetailList")
	var details: MutableList<Detail> = ArrayList()

	var isDetailed: Boolean = false

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

	fun setValue(value: String) {
		this.value = value.toDoubleByCulture() ?: 0.0
	}

	fun setDefaultData(activityAccountUrl: String?, useCategories: Boolean) {
		if (id == 0) {
			accountOut = activityAccountUrl
			accountIn = activityAccountUrl
		} else {
			warnCategory = !useCategories && category != null
		}
	}

	fun setParameters(request: InternalRequest<MovesCreateStatic>) {
		request.addParameter("ID", id)
		request.addParameter("Description", description)

		request.addParameter("Date.Year", date.year)
		request.addParameter("Date.Month", date.month)
		request.addParameter("Date.Day", date.day)

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
}
