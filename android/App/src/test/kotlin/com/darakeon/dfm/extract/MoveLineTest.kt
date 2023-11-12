package com.darakeon.dfm.extract

import android.os.Bundle
import android.view.View
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.ExtractBinding
import com.darakeon.dfm.databinding.MoveLineBinding
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.api.guid
import com.darakeon.dfm.testutils.getDecimal
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.api.ActivityMock
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
import java.util.UUID

@RunWith(RobolectricTestRunner::class)
class MoveLineTest: BaseTest() {
	private lateinit var mocker: ActivityMock<ExtractActivity>
	private lateinit var activity: ExtractActivity
	private lateinit var parentBinding: ExtractBinding
	private lateinit var moveLine: MoveLine
	private lateinit var binding: MoveLineBinding

	@Before
	fun setup() {
		mocker = ActivityMock(ExtractActivity::class)

		activity = mocker.get()
		activity.onCreate(Bundle(), null)
		activity.waitTasks(mocker.server)

		parentBinding = ExtractBinding.bind(
			shadowOf(activity).contentView
		)

		moveLine = activity.layoutInflater
			.inflate(R.layout.move_line, parentBinding.mainTable, false)
			as MoveLine

		binding = MoveLineBinding.bind(moveLine)
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
		assertThat(binding.details.name.text.toString(), `is`("zelda"))
	}

	@Test
	fun setMovePositive() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, false, {}, {}, {}, {})

		assertThat(binding.details.value.text.toString(), `is`("+34.00".getDecimal()))
		val color = activity.getColor(android.R.color.holo_blue_dark)
		assertThat(binding.details.value.currentTextColor, `is`(color))

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

		assertThat(binding.details.value.text.toString(), `is`("-34.00".getDecimal()))
		val color = activity.getColor(android.R.color.holo_red_dark)
		assertThat(binding.details.value.currentTextColor, `is`(color))

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

		assertThat(binding.details.moveStatus.visibility, `is`(View.VISIBLE))
	}

	@Test
	fun setMoveCannotCheck() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, false, {}, {}, {}, {})

		assertThat(binding.details.moveStatus.visibility, `is`(View.GONE))
	}

	@Test
	fun setMoveChecked() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, true, guid
		)

		moveLine.setMove(move, true, {}, {}, {}, {})

		assertThat(binding.details.moveStatus.text.toString(), `is`("\uE101"))
		assertTrue(moveLine.isChecked)

		val color = activity.getColor(android.R.color.holo_blue_light)
		assertThat(binding.details.moveStatus.currentTextColor, `is`(color))

		assertNotNull(binding.details.moveStatus.typeface)
	}

	@Test
	fun setMoveUnchecked() {
		val move = Move(
			"zelda",
			1986, 2, 21,
			34.0, false, guid
		)

		moveLine.setMove(move, true, {}, {}, {}, {})

		assertThat(binding.details.moveStatus.text.toString(), `is`("\uE085"))
		assertFalse(moveLine.isChecked)

		val color = activity.getColor(android.R.color.holo_red_light)
		assertThat(binding.details.moveStatus.currentTextColor, `is`(color))

		assertNotNull(binding.details.moveStatus.typeface)
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

		assertNotNull(binding.details.date)
		assertThat(binding.details.date?.text.toString(), `is`("01"))
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

		assertNotNull(binding.details.date)
		assertThat(binding.details.date?.text.toString(), `is`("1986-02-21"))
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
		val root = parentBinding.mainTable.parent as LinearLayout
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

		assertThat(binding.actions.visibility, `is`(View.GONE))
		moveLine.performLongClick()
		assertThat(binding.actions.visibility, `is`(View.VISIBLE))

		binding.actionEdit.performClick()
		assertTrue(edit)

		binding.actionDelete.performClick()
		assertTrue(delete)

		binding.actionCheck.performClick()
		assertTrue(check)

		binding.actionUncheck.performClick()
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
		assertThat(binding.details.moveStatus.text.toString(), `is`("\uE101"))

		val color = activity.getColor(android.R.color.holo_blue_light)
		assertThat(binding.details.moveStatus.currentTextColor, `is`(color))
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
		assertThat(binding.details.moveStatus.text.toString(), `is`("\uE085"))

		val color = activity.getColor(android.R.color.holo_red_light)
		assertThat(binding.details.moveStatus.currentTextColor, `is`(color))
	}
}
