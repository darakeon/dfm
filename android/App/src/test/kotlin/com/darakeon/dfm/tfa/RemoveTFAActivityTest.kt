package com.darakeon.dfm.tfa

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.RemoveTfaBinding
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.api.ActivityMock
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
class RemoveTFAActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<RemoveTFAActivity>

	@Before
	fun setup() {
		mocker = ActivityMock(RemoveTFAActivity::class)
	}

	@After
	fun tearDown() {
		mocker.server.shutdown()
	}

	@Test
	fun structure() {
		val activity = mocker.create()

		assertNotNull(activity.findViewById(R.id.password))
	}

	@Test
	fun remove() {
		mocker.server.enqueue("empty")

		val activity = mocker.create()
		activity.simulateNetwork()

		val view = View(activity)

		activity.remove(view)
		activity.waitTasks(mocker.server)

		val binding = RemoveTfaBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.password.text.length, `is`(0))

		assertThat(
			binding.successMessage.text,
			`is`("Você receberá por e-mail as instruções para prosseguir com a remoção do Login mais Seguro")
		)
	}
}
