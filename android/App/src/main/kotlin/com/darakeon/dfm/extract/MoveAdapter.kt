package com.darakeon.dfm.extract

import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.lib.ui.Adapter

class MoveAdapter(
	activity: ExtractActivity,
	list: List<Move>,
	private val canCheck: Boolean
) : Adapter<ExtractActivity, Move, MoveLine>(activity, list) {
	override val lineLayoutId: Int
		get() = R.layout.move_details

	override fun populateView(view: MoveLine, position: Int) =
		view.setMove(list[position], canCheck)
}
