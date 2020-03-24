package com.darakeon.dfm.extract

import android.view.View
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.extract.Move
import com.darakeon.dfm.api.entities.moves.Nature
import com.darakeon.dfm.extensions.getPrivate
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.getDecimal
import com.darakeon.dfm.utils.robolectric.RoboContextMenu
import kotlinx.android.synthetic.main.extract.main_table
import kotlinx.android.synthetic.main.move_line.view.check_move
import kotlinx.android.synthetic.main.move_line.view.date
import kotlinx.android.synthetic.main.move_line.view.name
import kotlinx.android.synthetic.main.move_line.view.value
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.annotation.Config

@RunWith(RobolectricTestRunner::class)
class MoveLineTest {
	private lateinit var activity: ExtractActivity
	private lateinit var moveLine: MoveLine

	@Before
	fun setup() {
		activity = ActivityMock().create<ExtractActivity>()
		moveLine = activity.layoutInflater
			.inflate(R.layout.move_line, activity.main_table, false)
			as MoveLine
	}

	@Test
	fun setMoveDescription() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, 1
		)

		moveLine.setMove(activity, move, false)

		assertThat(moveLine.description, `is`("zelda"))
		assertThat(moveLine.name.text.toString(), `is`("zelda"))
	}

	@Test
	fun setMovePositive() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, 1
		)

		moveLine.setMove(activity, move, false)

		assertThat(moveLine.value.text.toString(), `is`("34.00".getDecimal()))
		val color = activity.getColor(R.color.positive_dark)
		assertThat(moveLine.value.currentTextColor, `is`(color))

		assertThat(moveLine.nature, `is`(Nature.In))
		assertThat(
			moveLine.getPrivate("checkNature") as Nature,
			`is`(Nature.In)
		)
	}

	@Test
	fun setMoveNegative() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			-34.0, false, 1
		)

		moveLine.setMove(activity, move, false)

		assertThat(moveLine.value.text.toString(), `is`("34.00".getDecimal()))
		val color = activity.getColor(R.color.negative_dark)
		assertThat(moveLine.value.currentTextColor, `is`(color))

		assertThat(moveLine.nature, `is`(Nature.Out))
		assertThat(
			moveLine.getPrivate("checkNature") as Nature,
			`is`(Nature.Out)
		)
	}

	@Test
	fun setMoveCanCheck() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, 1
		)

		moveLine.setMove(activity, move, true)

		assertThat(moveLine.check_move.visibility, `is`(View.VISIBLE))
	}

	@Test
	fun setMoveCannotCheck() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, 1
		)

		moveLine.setMove(activity, move, false)

		assertThat(moveLine.check_move.visibility, `is`(View.GONE))
	}

	@Test
	fun setMoveChecked() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, true, 1
		)

		moveLine.setMove(activity, move, true)

		assertThat(moveLine.check_move.text.toString(), `is`("\uE101"))
		assertTrue(moveLine.isChecked)

		val color = activity.getColor(R.color.checked_dark)
		assertThat(moveLine.check_move.currentTextColor, `is`(color))

		assertNotNull(moveLine.check_move.typeface)
	}

	@Test
	fun setMoveUnchecked() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, 1
		)

		moveLine.setMove(activity, move, true)

		assertThat(moveLine.check_move.text.toString(), `is`("\uE085"))
		assertFalse(moveLine.isChecked)

		val color = activity.getColor(R.color.unchecked_dark)
		assertThat(moveLine.check_move.currentTextColor, `is`(color))

		assertNotNull(moveLine.check_move.typeface)
	}

	@Test
	@Config(qualifiers = "port")
	fun setMoveDatePortrait() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, 1
		)

		moveLine.setMove(activity, move, false)

		assertNull(moveLine.date)
	}

	@Test
	@Config(qualifiers = "land")
	fun setMoveDateLandscape() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, 1
		)

		moveLine.setMove(activity, move, false)

		assertNotNull(moveLine.date)
		assertThat(moveLine.date?.text.toString(), `is`("1986-02-21"))
	}

	@Test
	fun setMoveClick() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, 1
		)

		// to show context menu, moveLine must be added to something
		val root = activity.main_table
		val linearLayout = root.parent as LinearLayout
		linearLayout.addView(moveLine)

		val menu = RoboContextMenu()
		activity.menuInflater.inflate(R.menu.move_options, menu)
		moveLine.menu = menu

		moveLine.setMove(activity, move, true)

		val shadow = shadowOf(moveLine)
		assertNotNull(shadow.onClickListener)

		moveLine.performClick()

		assertNotNull(activity.clickedView)
		assertThat(activity.clickedView as View, `is`(moveLine as View))
	}

	@Test
	fun getId() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, 1
		)

		moveLine.setMove(activity, move, false)

		assertThat(moveLine.id, `is`(1))
	}

	@Test
	fun reverseCheck() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, true, 1
		)

		moveLine.setMove(activity, move, true)

		assertTrue(moveLine.isChecked)

		moveLine.reverseCheck()

		assertFalse(moveLine.isChecked)
		assertThat(moveLine.check_move.text.toString(), `is`("\uE085"))

		val color = activity.getColor(R.color.unchecked_dark)
		assertThat(moveLine.check_move.currentTextColor, `is`(color))
	}
}
