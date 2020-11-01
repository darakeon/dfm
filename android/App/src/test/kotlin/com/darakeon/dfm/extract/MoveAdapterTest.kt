package com.darakeon.dfm.extract

import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.api.guid
import com.darakeon.dfm.utils.api.ActivityMock
import kotlinx.android.synthetic.main.move_details.view.name
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
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
		val adapter = MoveAdapter(activity, list, false)
		val line = activity.layoutInflater
			.inflate(R.layout.move_details, null)
			as MoveLine

		adapter.populateView(line, 0)

		assertThat(line.name.text.toString(), `is`("name"))
	}
}
