package com.darakeon.dfm.extract

import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.lib.ui.Adapter

class MoveAdapter(
	activity: ExtractActivity,
	list: List<Move>,
	private val canCheck: Boolean,
	private val edit: (MoveLine) -> Unit,
	private val delete: (MoveLine) -> Unit,
	private val check: (MoveLine) -> Unit,
	private val uncheck: (MoveLine) -> Unit,
) : Adapter<ExtractActivity, Move, MoveLine>(activity, list) {
	override val lineLayoutId: Int
		get() = R.layout.move_line

	override fun populateView(view: MoveLine, position: Int) =
		view.setMove(list[position], canCheck, edit, delete, check, uncheck)
}
