package com.darakeon.dfm.extract

import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.api.guid
import com.darakeon.dfm.utils.api.ActivityMock
import kotlinx.android.synthetic.main.move_details.view.name
import kotlinx.android.synthetic.main.move_line.view.action_check
import kotlinx.android.synthetic.main.move_line.view.action_delete
import kotlinx.android.synthetic.main.move_line.view.action_edit
import kotlinx.android.synthetic.main.move_line.view.action_uncheck
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class MoveAdapterTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun populateView() {
		val activity = ActivityMock(ExtractActivity::class).create()

		val list = listOf(Move("name", 2019, 5, 4, 0.0, false, guid))

		var edit = false
		var delete = false
		var check = false
		var uncheck = false

		val adapter = MoveAdapter(activity, list, false,
			{ edit = true },
			{ delete = true },
			{ check = true },
			{ uncheck = true }
		)

		val line = activity.layoutInflater
			.inflate(R.layout.move_line, null)
			as MoveLine

		adapter.populateView(line, 0)

		assertThat(line.name.text.toString(), `is`("name"))

		line.action_edit.performClick()
		assertTrue(edit)

		line.action_delete.performClick()
		assertTrue(delete)

		line.action_check.performClick()
		assertTrue(check)

		line.action_uncheck.performClick()
		assertTrue(uncheck)
	}
}
