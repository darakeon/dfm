package com.darakeon.dfm.api.entities.moves

import com.darakeon.dfm.api.entities.ComboItem

interface MoveForm {
	val isUsingCategories: Boolean

	val categoryList: Array<ComboItem>
	val natureList: Array<ComboItem>
	val accountList: Array<ComboItem>

	fun blockedByAccounts(): Boolean
	fun blockedByCategories(): Boolean
}
