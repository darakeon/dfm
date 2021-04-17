package com.darakeon.dfm.offlineFallback

import android.content.Context
import android.content.Intent
import android.os.SystemClock
import androidx.core.app.JobIntentService
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.Log
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.ApiCaller
import com.darakeon.dfm.lib.auth.Authentication
import com.google.gson.Gson
import java.io.Serializable
import kotlin.reflect.KClass

abstract class Service<S: Service<S, O>, O: Serializable>(
	private val typeService: KClass<S>,
	private val typeObj: KClass<O>,
) : JobIntentService(), ApiCaller {
	private val intervalMin
		get() = resources.getInteger(
			R.integer.service_seconds_interval
		) / 60.0

	private val intervalMs
		get() = (intervalMin * 60 * 1000).toLong()

	private lateinit var obj: O
	private lateinit var manager: Manager<O>

	override fun onHandleWork(intent: Intent) {
		manager = manager(this, typeObj)

		val json = intent.getStringExtra(objKey)
		val noObj = intent.getBooleanExtra(noObjKey, false)

		if (noObj) {
			next()
			return
		}

		if (json == null) {
			Log.record("No object received")
			next()
			return
		}

		val obj = Gson().fromJson(json, typeObj.java)

		this.obj = obj

		manager.add(obj)

		callApi(Api(this, null), obj) {
			manager.success(obj)
			next()
		}
	}

	private val auth
		get() = Authentication(this)

	override var ticket: String
		get() = auth.ticket
		set(value) { auth.ticket = value }

	override fun logout() {
		auth.clear()
	}

	override fun checkTFA() {
		manager.error(obj)
	}

	override fun error(text: String) {
		manager.error(obj, text)
		next()
	}

	override fun error(resId: Int) {
		Log.record("Error thrown: ${getString(resId)}")
		restart()
	}

	override fun error(resMessage: Int, resButton: Int, action: () -> Unit) {
		manager.error(obj, resMessage)
		action()
		next()
	}

	override fun error(url: String, error: Throwable) {
		Log.record("Error thrown: $url $error")
		restart()
	}

	private fun next() {
		val next = manager.next
		if (next != null) {
			restart(next.obj)
		} else {
			Log.record("end of jobs")
		}
		stopSelf()
	}

	private fun restart() {
		Log.record("...${intervalMin}min...")
		SystemClock.sleep(intervalMs)

		if (isStopped) return

		restart(obj)
	}

	private fun restart(obj: O) {
		start(this, typeService, obj)
	}

	protected abstract fun callApi(
		api: Api<*>, obj: O, success: () -> Unit
	)

	companion object {
		private const val objKey = "object"
		private const val noObjKey = "no_object"

		fun <O : Any> manager(context: Context, type: KClass<O>) =
			Manager(context, type)

		fun <O: Serializable, S: Service<S, O>> start(
			context: Context, typeService: KClass<S>, obj: O
		) {
			start<O, S>(context, typeService) {
				it.putExtra(objKey, Gson().toJson(obj))
			}
		}

		fun <O: Serializable, S: Service<S, O>> start(
			context: Context, typeService: KClass<S>, typeObj: KClass<O>
		) {
			val manager = manager(context, typeObj)
			if (manager.run) {
				start<O, S>(context, typeService) {
					it.putExtra(noObjKey, true)
				}
			} else {
				manager.clearSucceeded()
			}
		}

		private fun <O : Any, S: Service<S, O>> start(
			context: Context,
			type: KClass<S>,
			extra: (Intent) -> Unit
		) {
			val intent = Intent()
			extra(intent)
			enqueueWork(context, type.java, 27, intent)
		}
	}
}
