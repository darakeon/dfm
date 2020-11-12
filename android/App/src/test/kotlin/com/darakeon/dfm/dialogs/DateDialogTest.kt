package com.darakeon.dfm.dialogs

import android.app.AlertDialog
import android.app.Dialog
import android.content.res.Resources
import android.view.View
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.activity.TestActivity
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog
import java.util.Calendar.DECEMBER
import java.util.Calendar.MARCH

@RunWith(RobolectricTestRunner::class)
class DateDialogTest: BaseTest() {
	private val yearId = getResId("year")
	private val monthId = getResId("month")
	private val dayId = getResId("day")
	private lateinit var mocker: ActivityMock<TestActivity>

	@Before
	fun setup() {
		mocker = ActivityMock(TestActivity::class)
	}

	@Test
	fun getDateDialogWithDayMonthYear() {
		var year = 1986
		var month = MARCH
		var day = 27

		val activity = mocker.create()
		activity.getDateDialog(2013, DECEMBER, 23) {
			y, m, d ->
			year = y
			month = m
			day = d
		}.show()

		val dialog = getLatestAlertDialog()

		assertTrue(isVisible(dialog, dayId))
		assertTrue(isVisible(dialog, monthId))
		assertTrue(isVisible(dialog, yearId))

		dialog.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		assertThat(day, `is`(23))
		assertThat(month, `is`(DECEMBER))
		assertThat(year, `is`(2013))
	}

	@Test
	fun getDateDialogWithMonthYear() {
		var year = 1986
		var month = MARCH

		val activity = mocker.create()
		activity.getDateDialog(2013, DECEMBER) {
			y, m ->
			year = y
			month = m
		}.show()

		val dialog = getLatestAlertDialog()

		assertFalse(isVisible(dialog, dayId))
		assertTrue(isVisible(dialog, monthId))
		assertTrue(isVisible(dialog, yearId))

		dialog.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		assertThat(month, `is`(DECEMBER))
		assertThat(year, `is`(2013))
	}

	@Test
	fun getDateDialogWithYear() {
		var year = 1986

		val activity = mocker.create()
		activity.getDateDialog(2013) {
			y -> year = y
		}.show()

		val dialog = getLatestAlertDialog()

		assertFalse(isVisible(dialog, dayId))
		assertFalse(isVisible(dialog, monthId))
		assertTrue(isVisible(dialog, yearId))

		dialog.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		assertThat(year, `is`(2013))
	}

	private fun isVisible(dialog: AlertDialog, resId: Int) =
		dialog.findViewById<View>(resId)
			.visibility == View.VISIBLE

	private fun getResId(name: String) =
		Resources.getSystem()
			.getIdentifier(name, "id", "android")
}
