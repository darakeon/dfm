package com.darakeon.dfm.extensions

import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.ComboItem
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.log.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog

@RunWith(RobolectricTestRunner::class)
class FormTest {
	@get:Rule
	val log = LogRule()

	lateinit var activity: BaseActivity

	@Before
	fun setup() {
		activity = ActivityMock().create()
	}

	@Test
	fun setColorByAttrPositive() {
		val textView = TextView(activity)

		textView.setColorByAttr(R.attr.positive)
		val color = activity.getColor(R.color.positive_dark)
		assertThat(textView.currentTextColor, `is`(color))
	}

	@Test
	fun setColorByAttrNegative() {
		val textView = TextView(activity)

		textView.setColorByAttr(R.attr.negative)
		val color = activity.getColor(R.color.negative_dark)
		assertThat(textView.currentTextColor, `is`(color))
	}

	@Test
	fun applyGlyphiconWithoutText() {
		val textView = TextView(activity)
		textView.text = ""

		textView.applyGlyphicon()

		assertNull(textView.typeface)
	}

	@Test
	fun applyGlyphiconWith1Character() {
		val textView = TextView(activity)
		textView.setText(R.string.action_settings)

		textView.applyGlyphicon()

		assertNotNull(textView.typeface)
	}

	@Test
	fun applyGlyphiconWithMoreThan1Character() {
		val textView = TextView(activity)
		textView.text = "normal text"

		textView.applyGlyphicon()

		assertNull(textView.typeface)
	}

	@Test
	fun setValueColoredPositive() {
		val textView = TextView(activity)

		textView.setValueColored(50.00)
		assertThat(textView.text.toString(), `is`("+50.00"))
		val color = activity.getColor(R.color.positive_dark)
		assertThat(textView.currentTextColor, `is`(color))
	}

	@Test
	fun setValueColoredNegative() {
		val textView = TextView(activity)

		textView.setValueColored(-50.00)
		assertThat(textView.text.toString(), `is`("-50.00"))
		val color = activity.getColor(R.color.negative_dark)
		assertThat(textView.currentTextColor, `is`(color))
	}

	@Test
	fun setValueColoredNeutral() {
		val textView = TextView(activity)

		textView.setValueColored(0.00)
		assertThat(textView.text.toString(), `is`("0.00"))
		val color = activity.getColor(R.color.neutral_dark)
		assertThat(textView.currentTextColor, `is`(color))
	}

	@Test
	fun showChangeList() {
		var chosenText = ""
		var chosenValue = ""

		val options = listOf(
			ComboItem("A", "a"), ComboItem("B", "b")
		).toTypedArray()

		activity.showChangeList(options, R.string.app_short_name) {
			t: String, v: String ->
				chosenText = t
				chosenValue = v
		}

		val alert = getLatestAlertDialog()

		shadowOf(alert).clickOnItem(1)

		assertThat(chosenText, `is`("B"))
		assertThat(chosenValue, `is`("b"))
	}
}
