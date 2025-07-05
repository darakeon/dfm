package com.darakeon.dfm.offlineFallback

import android.app.Activity
import com.darakeon.dfm.R
import com.darakeon.dfm.utils.activity.mockContext
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Before
import org.junit.Test


class ManagerTest {
	private lateinit var manager: Manager<Fake>

	private lateinit var activity: Activity

    @Before
    fun setUp() {
		val mockContext = mockContext()
			.mockSharedPreferences()
			.mockResources()
			.addStringResource(R.string.error_not_identified, "error_not_identified")
			.addStringResource(R.string.fail_at_offline_insert, "fail_at_offline_insert")
			.mockTheme()

		activity = mockContext.activity
		manager = Manager(activity, Fake::class)
    }

    @After
    fun tearDown() {
    }

    @Test
    fun printCounting() {
		manager.printCounting()
    }

    @Test
    fun add() {
		assertNull(manager.next)
		assertThat(manager.run, `is`(false))
		assertNull(manager.error)

		val fake = Fake(1)
		manager.add(fake)

		assertThat(manager.next?.obj, `is`(fake))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)
    }

    @Test
    fun success() {
		val fake = Fake(2)
		manager.add(fake)

		assertThat(manager.next?.obj, `is`(fake))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.success(fake)

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))
		assertNull(manager.error)
    }

    @Test
    fun error_onlyKey() {
		val fake = Fake(3)
		manager.add(fake)

		assertThat(manager.next?.obj, `is`(fake))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.error(fake)

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))

		assertNotNull(manager.error)
		assertThat(manager.error?.obj, `is`(fake))
		assertThat(manager.error?.error, `is`("fail_at_offline_insert"))
    }

    @Test
    fun error_keyAndErrorCode() {
		val fake = Fake(4)
		manager.add(fake)

		assertThat(manager.next?.obj, `is`(fake))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.error(fake, R.string.error_not_identified)

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))

		assertNotNull(manager.error)
		assertThat(manager.error?.obj, `is`(fake))
		assertThat(manager.error?.error, `is`("error_not_identified"))
    }

    @Test
    fun error_keyAndErrorText() {
		val fake = Fake(5)
		manager.add(fake)

		assertThat(manager.next?.obj, `is`(fake))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.error(fake, "21")

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))

		assertNotNull(manager.error)
		assertThat(manager.error?.obj, `is`(fake))
		assertThat(manager.error?.error, `is`("21"))
    }

    @Test
    fun remove() {
		val fake = Fake(6)
		manager.add(fake)

		val item = manager.next!!

		assertThat(item.obj, `is`(fake))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.remove(item.hashCode())

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))
		assertNull(manager.error)
    }

    @Test
    fun clearSucceeded() {
		val fake = Fake(7)
		manager.add(fake)

		assertThat(manager.next?.obj, `is`(fake))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.clearSucceeded()

		assertThat(manager.next?.obj, `is`(fake))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.success(fake)

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))
		assertNull(manager.error)
    }

	data class Fake(val number: Int)
}
