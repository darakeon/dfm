package com.darakeon.dfm.terms

import android.os.Bundle
import android.os.PersistableBundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.TermsBinding
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.terms.Clause
import com.darakeon.dfm.lib.api.entities.terms.Terms
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.api.readBundle
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.api.ActivityMock
import com.darakeon.dfm.welcome.WelcomeActivity
import com.google.gson.Gson
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Assert.assertNotNull
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class TermsActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<TermsActivity>
	private lateinit var activity: TermsActivity

	@Before
	fun setup() {
		mocker = ActivityMock(TermsActivity::class)
		activity = mocker.get()
	}

	@After
	fun tearDown() {
		mocker.server.shutdown()
	}

	@Test
	fun structure() {
		val saved = Bundle()
		saved.putJson("terms", Terms())
		activity.onCreate(saved, null)

		assertNotNull(activity.findViewById(R.id.content))
		assertNotNull(activity.findViewById(R.id.date))
		assertNotNull(activity.findViewById(R.id.ok_button))
	}

	@Test
	fun onCreateFromApi() {
		activity.simulateNetwork()
		mocker.server.enqueue("terms_get")

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val binding = TermsBinding.bind(
			shadowOf(activity).contentView
		)

		val content = Clause(
			"main",
			listOf(
				Clause(
					"clause 1",
					listOf(
						Clause("clause 1a"),
						Clause("clause 1b"),
						Clause("clause 1c"),
					)
				),
				Clause("clause 2"),
				Clause(
					"clause 3",
					listOf(
						Clause(
							"clause 3a",
							listOf(
								Clause("clause 3aI"),
								Clause("clause 3aII"),
							)
						),
						Clause("clause 3b"),
					)
				),
			)
		)

		val terms = activity.getPrivate<Terms>("terms")
		assertThat(terms, `is`(Terms(2023, 11, 20, content)))

		val expected = terms.content.format()

		assertThat(binding.content.adapter.count, `is`(expected.size))

		for(i in expected.indices) {
			val line = binding.content.adapter.getItem(i)
			assertThat(line, `is`(expected[i]))
		}

		val effectiveDate = activity.getString(R.string.effective_date)
		val expectedDateText = effectiveDate.format(terms.date.format())
		assertThat(binding.date.text.toString(), `is`(expectedDateText))
	}

	@Test
	fun onCreateFromSavedState() {
		val saved = Bundle()
		saved.putString("terms", readBundle("terms"))

		activity.onCreate(saved, null)

		val binding = TermsBinding.bind(
			shadowOf(activity).contentView
		)

		val content = Clause(
			"main",
			listOf(
				Clause(
					"clause 1",
					listOf(
						Clause("clause 1a"),
						Clause("clause 1b"),
						Clause("clause 1c"),
					)
				),
				Clause("clause 2"),
				Clause(
					"clause 3",
					listOf(
						Clause(
							"clause 3a",
							listOf(
								Clause("clause 3aI"),
								Clause("clause 3aII"),
							)
						),
						Clause("clause 3b"),
					)
				),
			)
		)

		val terms = activity.getPrivate<Terms>("terms")
		assertThat(terms, `is`(Terms(2023, 11, 20, content)))

		val expectedClauses = terms.content.format()

		assertThat(binding.content.adapter.count, `is`(expectedClauses.size))

		for(i in expectedClauses.indices) {
			assertThat(binding.content.adapter.getItem(i), `is`(expectedClauses[i]))
		}

		val effectiveDate = activity.getString(R.string.effective_date)
		val expectedDateText = effectiveDate.format(terms.date.format())
		assertThat(binding.date.text.toString(), `is`(expectedDateText))
	}

	@Test
	fun onSaveInstance() {
		val originalTerms = Gson().fromJson(
			readBundle("terms"),
			Terms::class.java
		)

		val originalState = Bundle()
		originalState.putJson("terms", originalTerms)

		activity.onCreate(originalState, null)

		val newState = Bundle()
		activity.onSaveInstanceState(newState, PersistableBundle())

		val newTerms = newState.getFromJson("terms", Terms())
		assertThat(newTerms, `is`(originalTerms))
	}

	@Test
	fun exit() {
		activity.simulateNetwork()

		val saved = Bundle()
		saved.putJson("terms", Terms())
		activity.intent.putExtra("__parent", WelcomeActivity::class.java)

		activity.onCreate(saved, null)

		mocker.server.enqueue("empty")
		activity.exit(View(activity))

		val shadowActivity = shadowOf(activity)
		val intent = shadowActivity.peekNextStartedActivity()

		assertThat(intent.getCalledName(), `is`("WelcomeActivity"))
	}
}
