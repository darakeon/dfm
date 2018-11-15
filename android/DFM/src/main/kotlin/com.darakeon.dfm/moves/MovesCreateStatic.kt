package com.darakeon.dfm.moves

import com.darakeon.dfm.api.entities.moves.MoveCreation
import com.darakeon.dfm.base.SmartStatic

object MovesCreateStatic : SmartStatic
{
	override var succeeded: Boolean = false

	var moveCreation: MoveCreation = MoveCreation()
}

