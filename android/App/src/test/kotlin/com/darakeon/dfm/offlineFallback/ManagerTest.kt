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
	private lateinit var manager: Manager<Int>

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

		manager = Manager(activity, Int::class)
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

		manager.add(1)

		assertThat(manager.next?.obj, `is`(1))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)
    }

    @Test
    fun success() {
		manager.add(2)

		assertThat(manager.next?.obj, `is`( 2))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.success(2)

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))
		assertNull(manager.error)
    }

    @Test
    fun error_onlyKey() {
		manager.add(3)

		assertThat(manager.next?.obj, `is`(3))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.error(3)

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))

		assertNotNull(manager.error)
		assertThat(manager.error?.obj, `is`(3))
		assertThat(manager.error?.error, `is`("fail_at_offline_insert"))
    }

    @Test
    fun error_keyAndErrorCode() {
		manager.add(4)

		assertThat(manager.next?.obj, `is`(4))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.error(4, R.string.error_not_identified)

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))

		assertNotNull(manager.error)
		assertThat(manager.error?.obj, `is`(4))
		assertThat(manager.error?.error, `is`("error_not_identified"))
    }

    @Test
    fun error_keyAndErrorText() {
		manager.add(5)

		assertThat(manager.next?.obj, `is`(5))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.error(5, "21")

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))

		assertNotNull(manager.error)
		assertThat(manager.error?.obj, `is`(5))
		assertThat(manager.error?.error, `is`("21"))
    }

    @Test
    fun remove() {
		manager.add(6)

		val item = manager.next!!

		assertThat(item.obj, `is`(6))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.remove(item.hashCode())

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))
		assertNull(manager.error)
    }

    @Test
    fun clearSucceeded() {
		manager.add(7)

		assertThat(manager.next?.obj, `is`(7))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.clearSucceeded()

		assertThat(manager.next?.obj, `is`(7))
		assertThat(manager.run, `is`(true))
		assertNull(manager.error)

		manager.success(7)

		assertNull(manager.next)
		assertThat(manager.run, `is`(false))
		assertNull(manager.error)
    }
}
