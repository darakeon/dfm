package com.darakeon.dfm.moves

import android.content.Context
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.offlineFallback.Service
import kotlin.reflect.KClass

class MovesService: Service<MovesService, Move>(MovesService::class, Move::class) {
	override fun callApi(
		api: Api<*>, obj: Move, success: () -> Unit
	) = api.saveMove(obj, success)

	companion object {
		private val typeObj: KClass<Move>
			get() = Move::class

		private val typeService: KClass<MovesService>
			get() = MovesService::class

		fun manager(context: Context) =
			manager(context, typeObj)

		fun start(context: Context, move: Move) =
			start(context, typeService, move)

		fun start(context: Context) =
			start(context, typeService, typeObj)
	}
}
