package com.darakeon.dfm.extract

import android.view.View
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.api.guid
import com.darakeon.dfm.testutils.getDecimal
import com.darakeon.dfm.testutils.robolectric.waitTasksFinish
import com.darakeon.dfm.utils.api.ActivityMock
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
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.annotation.Config
import java.util.UUID

@RunWith(RobolectricTestRunner::class)
class MoveLineTest {
	@get:Rule
	val log = LogRule()

	private lateinit var mocker: ActivityMock<ExtractActivity>
	private lateinit var activity: ExtractActivity
	private lateinit var moveLine: MoveLine

	@Before
	fun setup() {
		mocker = ActivityMock(ExtractActivity::class)

		activity = mocker.get()
		activity.onCreate(null, null)
		waitTasksFinish()

		moveLine = activity.layoutInflater
			.inflate(R.layout.move_line, activity.main_table, false)
			as MoveLine
	}

	@Test
	fun setMoveDescription() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, false)

		assertThat(moveLine.description, `is`("zelda"))
		assertThat(moveLine.name.text.toString(), `is`("zelda"))
	}

	@Test
	fun setMovePositive() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, false)

		assertThat(moveLine.value.text.toString(), `is`("+34.00".getDecimal()))
		val color = activity.getColor(R.color.positive_dark)
		assertThat(moveLine.value.currentTextColor, `is`(color))

		assertThat(moveLine.nature, `is`(Nature.In))
		assertThat(
			moveLine.getPrivate("checkNature"),
			`is`(Nature.In)
		)
	}

	@Test
	fun setMoveNegative() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			-34.0, false, guid
		)

		moveLine.setMove(move, false)

		assertThat(moveLine.value.text.toString(), `is`("-34.00".getDecimal()))
		val color = activity.getColor(R.color.negative_dark)
		assertThat(moveLine.value.currentTextColor, `is`(color))

		assertThat(moveLine.nature, `is`(Nature.Out))
		assertThat(
			moveLine.getPrivate("checkNature"),
			`is`(Nature.Out)
		)
	}

	@Test
	fun setMoveCanCheck() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, true)

		assertThat(moveLine.check_move.visibility, `is`(View.VISIBLE))
	}

	@Test
	fun setMoveCannotCheck() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, false)

		assertThat(moveLine.check_move.visibility, `is`(View.GONE))
	}

	@Test
	fun setMoveChecked() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, true, guid
		)

		moveLine.setMove(move, true)

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
			34.0, false, guid
		)

		moveLine.setMove(move, true)

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
			1986, 2, 1,
			34.0, false, guid
		)

		moveLine.setMove(move, false)

		assertNotNull(moveLine.date)
		assertThat(moveLine.date?.text.toString(), `is`("01"))
	}

	@Test
	@Config(qualifiers = "land")
	fun setMoveDateLandscape() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, false)

		assertNotNull(moveLine.date)
		assertThat(moveLine.date?.text.toString(), `is`("1986-02-21"))
	}

	@Test
	fun setMoveClick() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		// to show context menu, moveLine must be
		// added to something that is inside activity
		val root = activity.main_table.parent as LinearLayout
		root.addView(moveLine)

		val menu = RoboContextMenu()
		activity.menuInflater.inflate(R.menu.move_options, menu)
		moveLine.menu = menu

		val shadow = shadowOf(moveLine)
		assertNull(shadow.onClickListener)

		moveLine.setMove(move, true)

		assertNotNull(shadow.onClickListener)

		var showingMenu = false
		moveLine.setOnCreateContextMenuListener { _, _, _ ->
			showingMenu = true
		}

		moveLine.performClick()

		assertTrue(showingMenu)
	}

	@Test
	fun getId() {
		val guid = UUID.randomUUID()

		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, false)

		assertThat(moveLine.guid, `is`(guid))
	}

	@Test
	fun check() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, true)

		assertFalse(moveLine.isChecked)

		moveLine.check()

		assertTrue(moveLine.isChecked)
		assertThat(moveLine.check_move.text.toString(), `is`("\uE101"))

		val color = activity.getColor(R.color.checked_dark)
		assertThat(moveLine.check_move.currentTextColor, `is`(color))
	}

	@Test
	fun uncheck() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, true, guid
		)

		moveLine.setMove(move, true)

		assertTrue(moveLine.isChecked)

		moveLine.uncheck()

		assertFalse(moveLine.isChecked)
		assertThat(moveLine.check_move.text.toString(), `is`("\uE085"))

		val color = activity.getColor(R.color.unchecked_dark)
		assertThat(moveLine.check_move.currentTextColor, `is`(color))
	}
}
