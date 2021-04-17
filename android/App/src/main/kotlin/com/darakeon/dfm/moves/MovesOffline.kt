package com.darakeon.dfm.offlineFallback

import android.content.Context
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.Log
import com.darakeon.dfm.lib.auth.getValue
import com.darakeon.dfm.lib.auth.setValue
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import kotlin.reflect.KClass

class Manager<O : Any>(
	private val context: Context,
	private val type: KClass<O>,
) {
	private val completeSPKey = "offline_fallback_${type.simpleName}"

	private var queue: String
		get() = context.getValue(completeSPKey)
		set(value) = context.setValue(completeSPKey, value)

	private var stati: Array<ObjStatus<O>>
		get() = if (queue.isEmpty()) {
			arrayOf()
		} else {
			val arrayType = object : TypeToken<Array<ObjStatus<O>>>() {}.type
			Gson().fromJson<Array<ObjStatus<O>>>(
				queue, arrayType
			).map {
				ObjStatus(
					Gson().fromJson(Gson().toJson(it.obj), type.java),
					it.status,
					it.error
				)
			}.toTypedArray()
		}
		set(value) {
			queue = Gson().toJson(value)
		}

	private val pending: Array<ObjStatus<O>>
		get() = stati.filter {
			m -> m.status == Status.Pending
		}.toTypedArray()

	val next: ObjStatus<O>?
		get() {
			printCounting()
			return pending.firstOrNull()
		}

	val run: Boolean
		get() = pending.any()

	val error: ObjStatus<O>?
		get() = stati.firstOrNull {
			m -> m.status == Status.Error
		}

	fun printCounting() =
		Log.record("Pending: ${pending.size}/${stati.size}")

	fun add(obj: O) {
		if (pending.any { m -> m.has(obj) }) {
			Log.record("Already added")
		} else {
			stati += ObjStatus(obj)
		}
	}

	fun success(obj: O) {
		Log.record("[SUCCEEDED]: $obj")
		update(obj) {
			it.success()
		}
	}

	fun error(obj: O) =
		error(obj, R.string.fail_at_offline_insert)

	fun error(obj: O, error: Int) =
		error(obj, context.getString(error))

	fun error(obj: O, error: String) {
		Log.record("[$error]: $obj")
		update(obj) {
			it.error(error)
		}
	}

	private fun update(obj: O, action: (ObjStatus<O>) -> Unit) {
		val list = pending
		action(list.first { m -> m.has(obj) })
		stati = list
	}

	fun remove(objStatusHash: Int) {
		stati = stati.filter {
			it.hashCode() != objStatusHash
		}.toTypedArray()
		printCounting()
	}

	fun clearSucceeded() {
		stati = stati.filter {
			it.status != Status.Success
		}.toTypedArray()
		Log.record("Stack: ${stati.size}")
	}
}
