package com.darakeon.dfm.extract

import android.view.View
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.api.guid
import com.darakeon.dfm.testutils.getDecimal
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.api.ActivityMock
import kotlinx.android.synthetic.main.extract.main_table
import kotlinx.android.synthetic.main.move_details.view.date
import kotlinx.android.synthetic.main.move_details.view.move_status
import kotlinx.android.synthetic.main.move_details.view.name
import kotlinx.android.synthetic.main.move_details.view.value
import kotlinx.android.synthetic.main.move_line.view.action_check
import kotlinx.android.synthetic.main.move_line.view.action_delete
import kotlinx.android.synthetic.main.move_line.view.action_edit
import kotlinx.android.synthetic.main.move_line.view.action_uncheck
import kotlinx.android.synthetic.main.move_line.view.actions
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
		activity.waitTasks(mocker.server)

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

		moveLine.setMove(move, false, {}, {}, {}, {})

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

		moveLine.setMove(move, false, {}, {}, {}, {})

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

		moveLine.setMove(move, false, {}, {}, {}, {})

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

		moveLine.setMove(move, true, {}, {}, {}, {})

		assertThat(moveLine.move_status.visibility, `is`(View.VISIBLE))
	}

	@Test
	fun setMoveCannotCheck() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, false, {}, {}, {}, {})

		assertThat(moveLine.move_status.visibility, `is`(View.GONE))
	}

	@Test
	fun setMoveChecked() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, true, guid
		)

		moveLine.setMove(move, true, {}, {}, {}, {})

		assertThat(moveLine.move_status.text.toString(), `is`("\uE101"))
		assertTrue(moveLine.isChecked)

		val color = activity.getColor(R.color.checked_dark)
		assertThat(moveLine.move_status.currentTextColor, `is`(color))

		assertNotNull(moveLine.move_status.typeface)
	}

	@Test
	fun setMoveUnchecked() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, true, {}, {}, {}, {})

		assertThat(moveLine.move_status.text.toString(), `is`("\uE085"))
		assertFalse(moveLine.isChecked)

		val color = activity.getColor(R.color.unchecked_dark)
		assertThat(moveLine.move_status.currentTextColor, `is`(color))

		assertNotNull(moveLine.move_status.typeface)
	}

	@Test
	@Config(qualifiers = "port")
	fun setMoveDatePortrait() {
		val move = Move(
			"zelda",
			1986, 2, 1,
			34.0, false, guid
		)

		moveLine.setMove(move, false, {}, {}, {}, {})

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

		moveLine.setMove(move, false, {}, {}, {}, {})

		assertNotNull(moveLine.date)
		assertThat(moveLine.date?.text.toString(), `is`("1986-02-21"))
	}

	@Test
	fun setMoveLongClick() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		// to show context menu, moveLine must be
		// added to something that is inside activity
		val root = activity.main_table.parent as LinearLayout
		root.addView(moveLine)

		val shadow = shadowOf(moveLine)
		assertNull(shadow.onLongClickListener)

		var edit = false
		var delete = false
		var check = false
		var uncheck = false
		moveLine.setMove(move, true,
			{ edit = true },
			{ delete = true },
			{ check = true },
			{ uncheck = true }
		)

		assertNotNull(shadow.onLongClickListener)

		assertThat(moveLine.actions.visibility, `is`(View.GONE))
		moveLine.performLongClick()
		assertThat(moveLine.actions.visibility, `is`(View.VISIBLE))

		moveLine.action_edit.performClick()
		assertTrue(edit)

		moveLine.action_delete.performClick()
		assertTrue(delete)

		moveLine.action_check.performClick()
		assertTrue(check)

		moveLine.action_uncheck.performClick()
		assertTrue(uncheck)
	}

	@Test
	fun getId() {
		val guid = UUID.randomUUID()

		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, false, {}, {}, {}, {})

		assertThat(moveLine.guid, `is`(guid))
	}

	@Test
	fun check() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, true, {}, {}, {}, {})

		assertFalse(moveLine.isChecked)

		moveLine.check()

		assertTrue(moveLine.isChecked)
		assertThat(moveLine.move_status.text.toString(), `is`("\uE101"))

		val color = activity.getColor(R.color.checked_dark)
		assertThat(moveLine.move_status.currentTextColor, `is`(color))
	}

	@Test
	fun uncheck() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, true, guid
		)

		moveLine.setMove(move, true, {}, {}, {}, {})

		assertTrue(moveLine.isChecked)
		moveLine.uncheck()

		assertFalse(moveLine.isChecked)
		assertThat(moveLine.move_status.text.toString(), `is`("\uE085"))

		val color = activity.getColor(R.color.unchecked_dark)
		assertThat(moveLine.move_status.currentTextColor, `is`(color))
	}
}
