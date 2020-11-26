package com.darakeon.dfm.lib.extensions

import android.view.MotionEvent
import android.view.MotionEvent.ACTION_DOWN
import android.view.MotionEvent.ACTION_MOVE
import android.view.MotionEvent.ACTION_UP
import android.view.View
import android.widget.GridLayout
import kotlin.math.abs

fun View.changeColSpan(size: Int) {
	val layoutParams = layoutParams as GridLayout.LayoutParams
	layoutParams.columnSpec = GridLayout.spec(GridLayout.UNDEFINED, size, size.toFloat())
}

fun View.swipe(direction: Direction, action: () -> Unit) {
	swipe(direction, 1, action)
}

fun View.swipe(direction: Direction, fingers: Int, action: () -> Unit) {
	val actions = touches
		.getOrAdd(this, mutableMapOf())

	actions.getOrAdd(
		direction, mutableMapOf()
	)[fingers] = action

	setOnTouchListener { view, event ->
		if (event.action == ACTION_UP && event.pointerCount == 1)
			view.performClick()

		touch(view, event, actions)
	}
}

private fun <K, V> MutableMap<K, V>.getOrAdd(key: K, defaultValue: V): V {
	if (this[key] == null)
		this[key] = defaultValue

	return this[key]!!
}

private var x = mutableMapOf<View, Float>()
private var y = mutableMapOf<View, Float>()
private var finger = mutableMapOf<View, Int>()
private var touches = mutableMapOf<
	View, MutableMap<Direction, MutableMap<Int, () -> Unit>>
>()

enum class Direction {
	None, Up, Left, Down, Right
}

private fun touch(view: View, event: MotionEvent, moves: MutableMap<Direction, MutableMap<Int, () -> Unit>>): Boolean {
	when(event.action) {
		ACTION_DOWN -> {
			x[view] = event.rawX
			y[view] = event.rawY
			finger[view] = event.pointerCount
		}

		ACTION_MOVE -> {
			if (!x.containsKey(view)) x[view] = event.rawX
			if (!y.containsKey(view)) y[view] = event.rawY
			if (!finger.containsKey(view)) finger[view] = event.pointerCount
		}

		ACTION_UP -> {
			val diffX = event.rawX - (x[view]?:0f)
			val diffY = event.rawY - (y[view]?:0f)

			val direction = getDirection(diffX, diffY)
			log(direction.toString())
			log(finger[view].toString())

			moves[direction]
				?.get(finger[view])
				?.invoke()

			x.remove(view)
			y.remove(view)
			finger.remove(view)
		}
	}

	return false
}

private fun getDirection(diffX: Float, diffY: Float): Direction {
	val absX = abs(diffX)
	val absY = abs(diffY)

	return when {
		absX > absY -> when {
			diffX > 0 -> Direction.Right
			diffX < 0 -> Direction.Left
			else -> Direction.None
		}
		absX < absY -> when {
			diffY > 0 -> Direction.Down
			diffY < 0 -> Direction.Up
			else -> Direction.None
		}
		else -> Direction.None
	}
}
