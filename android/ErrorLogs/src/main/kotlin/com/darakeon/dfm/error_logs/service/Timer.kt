package com.darakeon.dfm.error_logs.service

import android.os.CountDownTimer

class Timer(
	private val caller: Caller,
	private val interval: Int,
	private val times: Int?,
	private val call: (Long) -> Unit,
): CountDownTimer(
	(times ?: 20) * interval * 1000L,
	interval * 1000L
) {
	constructor(caller: Caller, interval: Int, call: (Long) -> Unit)
		: this(caller, interval, null, call)

	override fun onTick(millisUntilFinished: Long) {
		call(millisUntilFinished / interval / 1000)
	}

	override fun onFinish() {
		if (times == null) {
			caller.timer = Timer(caller, interval, times, call)
			caller.timer.start()
		}
	}

	interface Caller {
		var timer: Timer
	}
}
