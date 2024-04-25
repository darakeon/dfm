package com.darakeon.dfm.moves

import android.app.Dialog
import android.graphics.Paint
import android.net.Uri
import android.os.Bundle
import android.os.PersistableBundle
import android.view.View
import android.view.View.GONE
import android.view.View.VISIBLE
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.MovesBinding
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.AccountComboItem
import com.darakeon.dfm.lib.api.entities.ComboItem
import com.darakeon.dfm.lib.api.entities.Date
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.auth.setValueTyped
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.api.guid
import com.darakeon.dfm.testutils.api.readBundle
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.getDecimal
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.activity.getLastDatePicker
import com.darakeon.dfm.utils.api.ActivityMock
import com.darakeon.dfm.welcome.WelcomeActivity
import com.google.gson.Gson
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog
import java.util.Calendar
import java.util.UUID

@RunWith(RobolectricTestRunner::class)
class MovesActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<MovesActivity>
	private lateinit var activity: MovesActivity

	@Before
	fun setup() {
		mocker = ActivityMock(MovesActivity::class)
		activity = mocker.get()

		activity.setValueTyped("isUsingCategories", true)

		val categoryCombo = arrayListOf(
			ComboItem("My Category", "category"),
		)
		activity.setValueTyped("categoryCombo", categoryCombo)

		val accountCombo = arrayListOf(
			AccountComboItem("My Out", "out", null),
			AccountComboItem("My In", "in", null),
			AccountComboItem("My In BRL", "in_brl", "BRL"),
			AccountComboItem("My Out EUR", "out_eur", "EUR"),
		)

		activity.setValueTyped("accountCombo", accountCombo)
	}

	@After
	fun tearDown() {
		mocker.server.shutdown()
	}

	@Test
	fun structure() {
		activity.onCreate(Bundle(), null)

		assertNotNull(activity.findViewById(R.id.warnings))
		assertNotNull(activity.findViewById(R.id.no_accounts))
		assertNotNull(activity.findViewById(R.id.no_categories))
		assertNotNull(activity.findViewById(R.id.remove_check))
		assertNotNull(activity.findViewById(R.id.form))
		assertNotNull(activity.findViewById(R.id.description))
		assertNotNull(activity.findViewById(R.id.date))
		assertNotNull(activity.findViewById(R.id.date_picker))
		assertNotNull(activity.findViewById(R.id.category))
		assertNotNull(activity.findViewById(R.id.category_picker))
		assertNotNull(activity.findViewById(R.id.nature_out))
		assertNotNull(activity.findViewById(R.id.nature_transfer))
		assertNotNull(activity.findViewById(R.id.nature_in))
		assertNotNull(activity.findViewById(R.id.account_out))
		assertNotNull(activity.findViewById(R.id.account_out_picker))
		assertNotNull(activity.findViewById(R.id.account_in))
		assertNotNull(activity.findViewById(R.id.account_in_picker))
		assertNotNull(activity.findViewById(R.id.simple_value))
		assertNotNull(activity.findViewById(R.id.value))
		assertNotNull(activity.findViewById(R.id.conversion))
		assertNotNull(activity.findViewById(R.id.detailed_value))
		assertNotNull(activity.findViewById(R.id.detail_description))
		assertNotNull(activity.findViewById(R.id.detail_amount))
		assertNotNull(activity.findViewById(R.id.detail_value))
		assertNotNull(activity.findViewById(R.id.detail_conversion))
		assertNotNull(activity.findViewById(R.id.details))
	}

	@Test
	fun onCreateAccountAndIdByIntent() {
		activity.intent.putExtra("accountUrl", "url")
		activity.intent.putExtra("id", guid)

		activity.onCreate(Bundle(), null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val id = activity.getPrivate<UUID?>("id")
		assertThat(id, `is`(guid))
	}

	@Test
	fun onCreateAccountAndIdByQuery() {
		activity.intent.data = Uri.parse("?accountUrl=url&id=$guid")

		activity.onCreate(Bundle(), null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val id = activity.getPrivate<UUID?>("id")
		assertThat(id, `is`(guid))
	}

	@Test
	fun onCreateAccountAndIdByQueryInvalidId() {
		activity.intent.data = Uri.parse("?accountUrl=url&id=X")

		activity.onCreate(Bundle(), null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val id = activity.getPrivate<UUID?>("id")
		assertNull(id)
	}

	@Test
	fun onCreateFromApiMoveNew() {
		activity.intent.putExtra("accountUrl", "out")

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")
		assertNull(move.guid)
		assertThat(move.date, `is`(Date()))
		assertNull(move.inUrl)
		assertThat(move.outUrl, `is`("out"))

		assertTrue(binding.natureOut.isChecked)
		assertFalse(binding.natureIn.isChecked)
		assertFalse(binding.natureTransfer.isChecked)
	}

	@Test
	fun onCreateFromApiMoveEdit() {
		activity.intent.putExtra("accountUrl", "url")
		activity.intent.putExtra("id", guid)
		activity.simulateNetwork()
		mocker.server.enqueue("move_get")

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val move = activity.getPrivate<Move>("move")
		assertThat(move.guid, `is`(guid))
		assertThat(move.description, `is`("move"))
		assertThat(move.date, `is`(Date(2020, 3, 8)))
		assertThat(move.natureEnum, `is`(Nature.Transfer))
		assertThat(move.categoryName, `is`("category"))
		assertThat(move.outUrl, `is`("out"))
		assertThat(move.inUrl, `is`("in"))
		assertThat(move.value, `is`(1.0))
		assertThat(move.detailList.size, `is`(1))
		assertThat(move.detailList[0].description, `is`("detail"))
		assertThat(move.detailList[0].amount, `is`(1))
		assertThat(move.detailList[0].value, `is`(27.0))
		assertTrue(move.checked)

		assertFalse(move.warnCategory)
		assertTrue(move.isDetailed)
	}

	@Test
	fun onCreateWithSavedState() {
		val saved = Bundle()
		saved.putString("move", readBundle("move"))

		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		assertThat(move.guid, `is`(guid))
		assertThat(move.description, `is`("move"))
		assertThat(move.date, `is`(Date(2020, 3, 8)))
		assertThat(move.natureEnum, `is`(Nature.Transfer))
		assertThat(move.categoryName, `is`("category"))
		assertThat(move.outUrl, `is`("out"))
		assertThat(move.inUrl, `is`("in"))
		assertThat(move.value, `is`(1.0))
		assertThat(move.detailList.size, `is`(1))
		assertThat(move.detailList[0].description, `is`("detail"))
		assertThat(move.detailList[0].amount, `is`(1))
		assertThat(move.detailList[0].value, `is`(27.0))
		assertTrue(move.checked)

		assertFalse(move.warnCategory)
		assertTrue(move.isDetailed)
	}

	@Test
	fun populateResponseMoveNotAllowedByAccounts() {
		val saved = Bundle()

		activity.setValueTyped("accountCombo", emptyArray<ComboItem>())

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.noAccounts.visibility, `is`(VISIBLE))

		assertThat(binding.form.visibility, `is`(GONE))
		assertThat(binding.warnings.visibility, `is`(VISIBLE))
	}

	@Test
	fun populateResponseMoveNotAllowedByCategories() {
		val saved = Bundle()

		activity.setValueTyped("categoryCombo", emptyArray<ComboItem>())

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.noCategories.visibility, `is`(VISIBLE))

		assertThat(binding.form.visibility, `is`(GONE))
		assertThat(binding.warnings.visibility, `is`(VISIBLE))
	}

	@Test
	fun populateResponseMoveCheckedWarning() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_checked"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.removeCheck.visibility, `is`(VISIBLE))
	}

	@Test
	fun populateResponseDescription() {
		val saved = Bundle()
		saved.putString("move", readBundle("move"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.description.text.toString(), `is`("move"))

		val move = activity.getPrivate<Move>("move")

		binding.description.setText("another")
		assertThat(move.description, `is`("another"))
	}

	@Test
	fun populateResponseDate() {
		val saved = Bundle()
		saved.putString("move", readBundle("move"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.date.text.toString(), `is`("2020-03-08"))

		val pickerButton = shadowOf(binding.datePicker)
		assertNotNull(pickerButton.onClickListener)
	}

	@Test
	fun populateResponseUsingCategories() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_with_category"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val categoryButton = shadowOf(
			binding.categoryPicker
		)
		assertNotNull(categoryButton.onClickListener)
	}

	@Test
	fun populateResponseNotUsingCategories() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_without_category"))

		activity.setValueTyped("isUsingCategories", false)

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.category.visibility, `is`(GONE))
		assertThat(binding.categoryPicker.visibility, `is`(GONE))
	}

	@Test
	fun populateResponseWarnCategoryLose() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_with_category"))

		activity.setValueTyped("isUsingCategories", false)

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.category.visibility, `is`(GONE))
		assertThat(binding.categoryPicker.visibility, `is`(VISIBLE))

		val fieldFlags = binding.categoryPicker.paintFlags
		val strikeLine = Paint.STRIKE_THRU_TEXT_FLAG
		assertThat(fieldFlags.and(strikeLine), `is`(strikeLine))
	}

	@Test
	fun populateResponseNatureOut() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_out"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertTrue(binding.natureOut.isChecked)

		val move = activity.getPrivate<Move>("move")

		assertThat(move.natureEnum, `is`(Nature.Out))

		assertNotNull(move.outUrl)
		assertNull(move.inUrl)
	}

	@Test
	fun populateResponseNatureTransfer() {
		val saved = Bundle()
		saved.putString("move", readBundle("move"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertTrue(binding.natureTransfer.isChecked)

		val move = activity.getPrivate<Move>("move")

		assertThat(move.natureEnum, `is`(Nature.Transfer))

		assertNotNull(move.outUrl)
		assertNotNull(move.inUrl)

		assertThat(binding.accountOut.text.toString(), not(`is`("account out")))
		assertThat(binding.accountIn.text.toString(), not(`is`("account in")))
	}

	@Test
	fun populateResponseNatureIn() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_in"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertTrue(binding.natureIn.isChecked)

		val move = activity.getPrivate<Move>("move")

		assertThat(move.natureEnum, `is`(Nature.In))

		assertNull(move.outUrl)
		assertNotNull(move.inUrl)
	}

	@Test
	fun populateResponseAccounts() {
		val saved = Bundle()
		saved.putString("move", readBundle("move"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.accountOut.text.toString(), `is`("My Out"))
		assertThat(binding.accountIn.text.toString(), `is`("My In"))
	}

	@Test
	fun populateResponseDetails() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_detailed"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertTrue(move.isDetailed)

		assertThat(binding.simpleValue.visibility, `is`(GONE))
		assertThat(binding.detailedValue.visibility, `is`(VISIBLE))

		assertThat(binding.details.childCount, `is`(2))

		val detail1 = binding.details.getChildAt(0) as DetailBox
		val getText1 = { id: Int -> detail1.findViewById<TextView>(id).text.toString() }
		assertThat(getText1(R.id.detail_description), `is`("detail 1"))
		assertThat(getText1(R.id.detail_amount), `is`("1"))
		assertThat(getText1(R.id.detail_value), `is`("27.00".getDecimal()))

		val detail2 = binding.details.getChildAt(1) as DetailBox
		val getText2 = { id: Int -> detail2.findViewById<TextView>(id).text.toString() }
		assertThat(getText2(R.id.detail_description), `is`("detail 2"))
		assertThat(getText2(R.id.detail_amount), `is`("2"))
		assertThat(getText2(R.id.detail_value), `is`("54.00".getDecimal()))
	}

	@Test
	fun populateResponseDetailsConversion() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_detailed_conversion"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertTrue(move.isDetailed)

		assertThat(binding.simpleValue.visibility, `is`(GONE))
		assertThat(binding.detailedValue.visibility, `is`(VISIBLE))

		assertThat(binding.details.childCount, `is`(2))

		val detail1 = binding.details.getChildAt(0) as DetailBox
		val getText1 = { id: Int -> detail1.findViewById<TextView>(id).text.toString() }
		assertThat(getText1(R.id.detail_description), `is`("detail 1"))
		assertThat(getText1(R.id.detail_amount), `is`("1"))
		assertThat(getText1(R.id.detail_value), `is`("27.00".getDecimal()))
		assertThat(getText1(R.id.detail_conversion), `is`("135.00".getDecimal()))

		val detail2 = binding.details.getChildAt(1) as DetailBox
		val getText2 = { id: Int -> detail2.findViewById<TextView>(id).text.toString() }
		assertThat(getText2(R.id.detail_description), `is`("detail 2"))
		assertThat(getText2(R.id.detail_amount), `is`("2"))
		assertThat(getText2(R.id.detail_value), `is`("54.00".getDecimal()))
		assertThat(getText2(R.id.detail_conversion), `is`("270.00".getDecimal()))
	}

	@Test
	fun populateResponseValue() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_single_value"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertFalse(move.isDetailed)

		assertThat(binding.simpleValue.visibility, `is`(VISIBLE))
		assertThat(binding.detailedValue.visibility, `is`(GONE))

		assertThat(binding.value.text.toString(), `is`("1.00".getDecimal()))

		binding.value.setText("2")
		assertThat(move.value, `is`(2.0))

		assertThat(binding.details.childCount, `is`(0))
	}

	@Test
	fun populateResponseConversion() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_conversion"))

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertFalse(move.isDetailed)

		assertThat(binding.simpleValue.visibility, `is`(VISIBLE))
		assertThat(binding.detailedValue.visibility, `is`(GONE))

		assertThat(binding.value.text.toString(), `is`("1.00".getDecimal()))
		assertThat(binding.conversion.text.toString(), `is`("5.00".getDecimal()))

		binding.value.setText("2")
		assertThat(move.value, `is`(2.0))

		binding.conversion.setText("10")
		assertThat(move.conversion, `is`(10.0))

		assertThat(binding.details.childCount, `is`(0))
	}

	@Test
	fun onSaveInstance() {
		val originalMove = Gson().fromJson(
			readBundle("move"),
			Move::class.java
		)

		val originalState = Bundle()
		originalState.putJson("move", originalMove)

		activity.onCreate(originalState, null)

		val newState = Bundle()
		activity.onSaveInstanceState(newState, PersistableBundle())

		val newMove = newState.getFromJson("move", Move())
		assertThat(newMove, `is`(originalMove))
	}

	@Test
	fun dateDialog() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.date.text.toString(), `is`(""))

		activity.showDatePicker()

		val dialog = getLastDatePicker()
		dialog.updateDate(1986, Calendar.MARCH, 27)
		dialog.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		assertThat(binding.date.text.toString(), `is`("1986-03-27"))
	}

	@Test
	fun dateTyping() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.date.text.toString(), `is`(""))

		binding.date.append("1986-03-27")

		val move = activity.getPrivate<Move>("move")
		assertThat(move.date, `is`(Date(1986, 3, 27)))
	}

	@Test
	fun categoryDialog() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.category.text.toString(), `is`(""))
		assertNull(move.categoryName)

		activity.changeCategory()
		shadowOf(getLatestAlertDialog()).clickOnItem(1)

		assertThat(binding.category.text.toString(), `is`("My Category"))
		assertThat(move.categoryName, `is`("category"))
	}

	@Test
	fun categoryTyping() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val suggestions = binding.category.adapter
		assertNotNull(suggestions)
		assertThat(suggestions.count, `is`(1))

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.category.text.toString(), `is`(""))
		assertNull(move.categoryName)

		binding.category.append("My Category")

		assertThat(move.categoryName, `is`("category"))
	}

	@Test
	fun warnLoseCategory() {
		val saved = Bundle()
		saved.putString("move", readBundle("move_with_category"))

		activity.setValueTyped("isUsingCategories", false)

		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		binding.categoryPicker.performClick()

		val alert = getLatestAlertDialog()
		val shadow = shadowOf(alert)

		assertThat(shadow.title.toString(), `is`("Ops!"))
		val message = activity.getString(R.string.losingCategory)
		assertThat(shadow.message.toString(), `is`(message))
	}

	@Test
	fun accountOutDialog() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.accountOut.text.toString(), `is`(""))
		assert(move.outUrl.isNullOrEmpty())

		activity.changeAccountOut()
		shadowOf(getLatestAlertDialog()).clickOnItem(1)

		assertThat(binding.accountOut.text.toString(), `is`("My Out"))
		assertThat(move.outUrl, `is`("out"))

		assertTrue(binding.natureOut.isChecked)
		assertFalse(binding.natureTransfer.isChecked)
		assertFalse(binding.natureIn.isChecked)
	}

	@Test
	fun accountOutTyping() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.accountOut.text.toString(), `is`(""))
		assert(move.outUrl.isNullOrEmpty())

		binding.accountOut.append("My Out")

		assertThat(move.outUrl, `is`("out"))

		assertTrue(binding.natureOut.isChecked)
		assertFalse(binding.natureTransfer.isChecked)
		assertFalse(binding.natureIn.isChecked)
	}

	@Test
	fun accountInDialog() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.accountIn.text.toString(), `is`(""))
		assertNull(move.inUrl)

		activity.changeAccountIn()
		shadowOf(getLatestAlertDialog()).clickOnItem(2)

		assertThat(binding.accountIn.text.toString(), `is`("My In"))
		assertThat(move.inUrl, `is`("in"))

		assertFalse(binding.natureOut.isChecked)
		assertFalse(binding.natureTransfer.isChecked)
		assertTrue(binding.natureIn.isChecked)
	}

	@Test
	fun accountInTyping() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.accountIn.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		binding.accountIn.append("My In")

		assertThat(move.inUrl, `is`("in"))

		assertFalse(binding.natureOut.isChecked)
		assertFalse(binding.natureTransfer.isChecked)
		assertTrue(binding.natureIn.isChecked)
	}

	@Test
	fun accountOutAndInDialog() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.accountIn.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		activity.changeAccountIn()
		shadowOf(getLatestAlertDialog()).clickOnItem(2)

		activity.changeAccountOut()
		shadowOf(getLatestAlertDialog()).clickOnItem(1)

		assertThat(binding.accountIn.text.toString(), `is`("My In"))
		assertThat(move.inUrl, `is`("in"))

		assertThat(binding.accountOut.text.toString(), `is`("My Out"))
		assertThat(move.outUrl, `is`("out"))

		assertFalse(binding.natureOut.isChecked)
		assertTrue(binding.natureTransfer.isChecked)
		assertFalse(binding.natureIn.isChecked)
	}

	@Test
	fun accountOutAndInTyping() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.accountIn.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		binding.accountIn.append("My In")
		binding.accountOut.append("My Out")

		assertThat(move.inUrl, `is`("in"))
		assertThat(move.outUrl, `is`("out"))

		assertFalse(binding.natureOut.isChecked)
		assertTrue(binding.natureTransfer.isChecked)
		assertFalse(binding.natureIn.isChecked)
	}

	@Test
	fun useDetailed() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		activity.useDetailed(View(activity))

		assertTrue(move.isDetailed)

		assertThat(binding.detailAmount.text.toString(), `is`("1"))
	}

	@Test
	fun useSimple() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		activity.useSimple(View(activity))

		assertFalse(move.isDetailed)
	}

	@Test
	fun addDetail() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		binding.detailDescription.setText("cat")
		binding.detailAmount.setText("18")
		binding.detailValue.setText("3.14".getDecimal())

		activity.addDetail(View(activity))

		assertThat(binding.detailDescription.text.toString(), `is`(""))
		assertThat(binding.detailAmount.text.toString(), `is`("1"))
		assertThat(binding.detailValue.text.toString(), `is`(""))

		assertThat(move.detailList.size, `is`(1))
		assertThat(move.detailList[0].description, `is`("cat"))
		assertThat(move.detailList[0].amount, `is`(18))
		assertThat(move.detailList[0].value, `is`(3.14))

		assertThat(binding.details.childCount, `is`(1))
		val detail = binding.details.getChildAt(0) as DetailBox
		val getText = { id: Int -> detail.findViewById<TextView>(id).text.toString() }
		assertThat(getText(R.id.detail_description), `is`("cat"))
		assertThat(getText(R.id.detail_amount), `is`("18"))
		assertThat(getText(R.id.detail_value), `is`("3.14".getDecimal()))
	}

	@Test
	fun addDetailConversion() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		binding.detailDescription.setText("cat")
		binding.detailAmount.setText("18")
		binding.detailValue.setText("3.14".getDecimal())
		binding.detailConversion.setText("15.70".getDecimal())

		activity.addDetail(View(activity))

		assertThat(binding.detailDescription.text.toString(), `is`(""))
		assertThat(binding.detailAmount.text.toString(), `is`("1"))
		assertThat(binding.detailValue.text.toString(), `is`(""))
		assertThat(binding.detailConversion.text.toString(), `is`(""))

		assertThat(move.detailList.size, `is`(1))
		assertThat(move.detailList[0].description, `is`("cat"))
		assertThat(move.detailList[0].amount, `is`(18))
		assertThat(move.detailList[0].value, `is`(3.14))
		assertThat(move.detailList[0].conversion, `is`(15.7))

		assertThat(binding.details.childCount, `is`(1))
		val detail = binding.details.getChildAt(0) as DetailBox
		val getText = { id: Int -> detail.findViewById<TextView>(id).text.toString() }
		assertThat(getText(R.id.detail_description), `is`("cat"))
		assertThat(getText(R.id.detail_amount), `is`("18"))
		assertThat(getText(R.id.detail_value), `is`("3.14".getDecimal()))
		assertThat(getText(R.id.detail_conversion), `is`("15.70".getDecimal()))
	}

	@Test
	fun addDetailNoDescription() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		binding.detailDescription.setText("")
		binding.detailAmount.setText("18")
		binding.detailValue.setText("3.14".getDecimal())

		activity.addDetail(View(activity))

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.title.toString(), `is`("Ops!"))
		val message = activity.getString(R.string.fill_all)
		assertThat(shadow.message.toString(), `is`(message))

		assertThat(move.detailList.size, `is`(0))
		assertThat(binding.details.childCount, `is`(0))
	}

	@Test
	fun addDetailNoAmount() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		binding.detailDescription.setText("cat")
		binding.detailAmount.setText("")
		binding.detailValue.setText("3.14".getDecimal())

		activity.addDetail(View(activity))

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.title.toString(), `is`("Ops!"))
		val message = activity.getString(R.string.fill_all)
		assertThat(shadow.message.toString(), `is`(message))

		assertThat(move.detailList.size, `is`(0))
		assertThat(binding.details.childCount, `is`(0))
	}

	@Test
	fun addDetailNoValue() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		binding.detailDescription.setText("cat")
		binding.detailAmount.setText("18")
		binding.detailValue.setText("")

		activity.addDetail(View(activity))

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.title.toString(), `is`("Ops!"))
		val message = activity.getString(R.string.fill_all)
		assertThat(shadow.message.toString(), `is`(message))

		assertThat(move.detailList.size, `is`(0))
		assertThat(binding.details.childCount, `is`(0))
	}

	@Test
	fun save() {
		activity.simulateNetwork()

		activity.intent.putExtra(
			"__parent",
			WelcomeActivity::class.java
		)
		activity.intent.putExtra("id", guid)

		val saved = Bundle()
		saved.putString("move", readBundle("move"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")
		assertNotNull(move.value)

		mocker.server.enqueue("empty")
		activity.save(View(activity))
		activity.waitTasks(mocker.server)

		assertNull(move.value)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()
		assertNull(intent.extras?.get("id"))
		assertThat(intent.getCalledName(), `is`("WelcomeActivity"))
	}

	@Test
	fun edit() {
		activity.simulateNetwork()

		activity.intent.putExtra("accountUrl", "url")
		activity.intent.putExtra("id", guid)
		activity.intent.putExtra(
			"__parent",
			WelcomeActivity::class.java
		)

		activity.onCreate(null, null)

		mocker.server.enqueue("empty")
		activity.save(View(activity))

		activity.waitTasks(mocker.server)

		val requestPath = mocker.server.lastPath()
		val urlGuid = requestPath.split('/').last()
		assertThat(urlGuid, `is`(guid.toString()))
	}

	@Test
	fun accountWithoutConversion() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.accountIn.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		binding.accountIn.append("My In")
		binding.accountOut.append("My Out")

		assertThat(move.inUrl, `is`("in"))
		assertThat(move.outUrl, `is`("out"))
		assertThat(binding.conversion.visibility, `is`(GONE))

		activity.useDetailed(binding.detailedValue)

		assertThat(binding.detailConversion.visibility, `is`(GONE))
	}

	@Test
	fun accountWithConversion() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.accountIn.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		binding.accountIn.append("My In BRL")
		binding.accountOut.append("My Out EUR")

		assertThat(move.inUrl, `is`("in_brl"))
		assertThat(move.outUrl, `is`("out_eur"))
		assertThat(binding.conversion.visibility, `is`(VISIBLE))

		activity.useDetailed(binding.detailedValue)

		assertThat(binding.detailConversion.visibility, `is`(VISIBLE))
	}

	@Test
	fun accountWithoutConversionBecauseJustOut() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.accountIn.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		binding.accountOut.append("My Out EUR")

		assertNull(move.inUrl)
		assertThat(move.outUrl, `is`("out_eur"))
		assertThat(binding.conversion.visibility, `is`(GONE))

		activity.useDetailed(binding.detailedValue)

		assertThat(binding.detailConversion.visibility, `is`(GONE))
	}

	@Test
	fun accountWithoutConversionBecauseJustIn() {
		val saved = Bundle()
		activity.onCreate(saved, null)

		val binding = MovesBinding.bind(
			shadowOf(activity).contentView
		)

		val move = activity.getPrivate<Move>("move")

		assertThat(binding.accountIn.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		binding.accountIn.append("My In BRL")

		assertThat(move.inUrl, `is`("in_brl"))
		assertNull(move.outUrl)
		assertThat(binding.conversion.visibility, `is`(GONE))

		activity.useDetailed(binding.detailedValue)

		assertThat(binding.detailConversion.visibility, `is`(GONE))
	}
}
