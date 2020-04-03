package com.darakeon.dfm.utils.robolectric

import android.graphics.drawable.Drawable
import android.view.ContextMenu
import android.view.View
import org.robolectric.fakes.RoboMenu

class RoboContextMenu: RoboMenu(), ContextMenu {
	override fun setHeaderIcon(iconRes: Int) = this
	override fun setHeaderIcon(icon: Drawable?) = this
	override fun setHeaderView(view: View?) = this
	override fun setHeaderTitle(titleRes: Int) = this
	override fun setHeaderTitle(title: CharSequence?) = this
	override fun clearHeader() { }
}
