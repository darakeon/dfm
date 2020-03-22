package com.darakeon.dfm.extract

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.extract.Move
import com.darakeon.dfm.utils.activity.ActivityMock
import kotlinx.android.synthetic.main.move_line.view.name
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class MoveAdapterTest {
	@Test
	fun populateView() {
		val list = arrayOf(Move("name", 2019, 5, 4, 0.0, false, 4))
		val activity = ActivityMock.create<ExtractActivity>()
		val adapter = MoveAdapter(activity, list, false)
		val line = activity.layoutInflater
			.inflate(R.layout.move_line, null)
			as MoveLine

		adapter.populateView(line, 0)

		assertThat(line.name.text.toString(), `is`("name"))
	}
}
