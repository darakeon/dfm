package com.darakeon.dfm.extract

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.extract.Move
import com.darakeon.dfm.base.Adapter

class MoveAdapter(
	private val activity: ExtractActivity,
	list: List<Move>,
	private val canCheck: Boolean
) : Adapter<ExtractActivity, Move, MoveLine>(activity, list) {
	override val id: Int
		get() = R.layout.move_line

	override fun populateView(view: MoveLine, position: Int) =
		view.setMove(activity, list[position], canCheck)
}
