package com.darakeon.dfm.extract

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.extract.Move
import com.darakeon.dfm.base.Adapter

class MoveAdapter(
	private val activity: ExtractActivity,
	moveJsonList: Array<Move>,
	private val canCheck: Boolean
) : Adapter<ExtractActivity, Move, MoveLine>(activity, moveJsonList) {
	private val moveList: MutableList<Move> =
		moveJsonList.toMutableList()

	override val id: Int
		get() = R.layout.extract_line

	override fun populateView(view: MoveLine, position: Int) =
		view.setMove(activity, moveList[position], canCheck)
}
