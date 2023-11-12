package com.darakeon.dfm.extract

import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.MoveLineBinding
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.api.guid
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class MoveAdapterTest: BaseTest() {
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

		val binding = MoveLineBinding.bind(line)

		assertThat(binding.details.name.text.toString(), `is`("name"))

		binding.actionEdit.performClick()
		assertTrue(edit)

		binding.actionDelete.performClick()
		assertTrue(delete)

		binding.actionCheck.performClick()
		assertTrue(check)

		binding.actionUncheck.performClick()
		assertTrue(uncheck)
	}
}
