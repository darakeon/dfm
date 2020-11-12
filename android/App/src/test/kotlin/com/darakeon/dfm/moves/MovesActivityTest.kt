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
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.ComboItem
import com.darakeon.dfm.lib.api.entities.Date
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.api.entities.moves.MoveCreation
import com.darakeon.dfm.lib.api.entities.moves.MoveForm
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.LogRule
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
import kotlinx.android.synthetic.main.moves.account_in
import kotlinx.android.synthetic.main.moves.account_out
import kotlinx.android.synthetic.main.moves.category
import kotlinx.android.synthetic.main.moves.category_picker
import kotlinx.android.synthetic.main.moves.date
import kotlinx.android.synthetic.main.moves.date_picker
import kotlinx.android.synthetic.main.moves.description
import kotlinx.android.synthetic.main.moves.detail_amount
import kotlinx.android.synthetic.main.moves.detail_description
import kotlinx.android.synthetic.main.moves.detail_value
import kotlinx.android.synthetic.main.moves.detailed_value
import kotlinx.android.synthetic.main.moves.details
import kotlinx.android.synthetic.main.moves.form
import kotlinx.android.synthetic.main.moves.nature_in
import kotlinx.android.synthetic.main.moves.nature_out
import kotlinx.android.synthetic.main.moves.nature_transfer
import kotlinx.android.synthetic.main.moves.no_accounts
import kotlinx.android.synthetic.main.moves.no_categories
import kotlinx.android.synthetic.main.moves.remove_check
import kotlinx.android.synthetic.main.moves.simple_value
import kotlinx.android.synthetic.main.moves.value
import kotlinx.android.synthetic.main.moves.warnings
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
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
	}

	@Test
	fun structure() {
		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

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
		assertNotNull(activity.findViewById(R.id.detailed_value))
		assertNotNull(activity.findViewById(R.id.detail_description))
		assertNotNull(activity.findViewById(R.id.detail_amount))
		assertNotNull(activity.findViewById(R.id.detail_value))
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
	fun onCreateFromApiForm() {
		activity.intent.putExtra("accountUrl", "url")
		activity.simulateNetwork()
		mocker.server.enqueue("move_get")

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val form = activity
			.getPrivate<MoveForm>("moveForm")

		assertTrue(form.isUsingCategories)

		assertThat(form.categoryList.size, `is`(1))
		assertThat(form.categoryList[0], `is`(ComboItem("Category", "category")))

		assertThat(form.natureList.size, `is`(3))
		assertThat(form.natureList[0], `is`(ComboItem("Out", "0")))
		assertThat(form.natureList[1], `is`(ComboItem("In", "1")))
		assertThat(form.natureList[2], `is`(ComboItem("Transfer", "2")))

		assertThat(form.accountList.size, `is`(1))
		assertThat(form.accountList[0], `is`(ComboItem("Account", "account")))
	}

	@Test
	fun onCreateFromApiMoveNew() {
		activity.intent.putExtra("accountUrl", "url")
		activity.simulateNetwork()
		mocker.server.enqueue("move_get_new")

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val move = activity.getPrivate<Move>("move")
		assertNull(move.guid)
		assertThat(move.date, `is`(Date()))
		assertNull(move.inUrl)
		assertThat(move.outUrl, `is`("url"))

		assertTrue(activity.nature_out.isChecked)
		assertFalse(activity.nature_in.isChecked)
		assertFalse(activity.nature_transfer.isChecked)
	}

	@Test
	fun onCreateFromApiMoveEdit() {
		activity.intent.putExtra("accountUrl", "url")
		activity.intent.putExtra("id", 27)
		activity.simulateNetwork()
		mocker.server.enqueue("move_get_edit")

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
		saved.putString("moveForm", readBundle("move_form"))

		activity.onCreate(saved, null)

		val form = activity.getPrivate<MoveForm>("moveForm")

		assertTrue(form.isUsingCategories)

		assertThat(form.categoryList.size, `is`(1))
		assertThat(form.categoryList[0], `is`(ComboItem("My Category", "category")))

		assertThat(form.natureList.size, `is`(3))
		assertThat(form.natureList[0], `is`(ComboItem("Out", "0")))
		assertThat(form.natureList[1], `is`(ComboItem("In", "1")))
		assertThat(form.natureList[2], `is`(ComboItem("Transfer", "2")))

		assertThat(form.accountList.size, `is`(2))
		assertThat(form.accountList[0], `is`(ComboItem("My Out", "out")))
		assertThat(form.accountList[1], `is`(ComboItem("My In", "in")))

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
		saved.putString("moveForm", readBundle("move_form_no_accounts"))

		activity.onCreate(saved, null)

		val form = activity.getPrivate<MoveForm>("moveForm")

		assertTrue(form.blockedByAccounts())
		assertThat(activity.no_accounts.visibility, `is`(VISIBLE))

		assertThat(activity.form.visibility, `is`(GONE))
		assertThat(activity.warnings.visibility, `is`(VISIBLE))
	}

	@Test
	fun populateResponseMoveNotAllowedByCategories() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form_no_categories"))

		activity.onCreate(saved, null)

		val form = activity.getPrivate<MoveForm>("moveForm")

		assertTrue(form.blockedByCategories())
		assertThat(activity.no_categories.visibility, `is`(VISIBLE))

		assertThat(activity.form.visibility, `is`(GONE))
		assertThat(activity.warnings.visibility, `is`(VISIBLE))
	}

	@Test
	fun populateResponseMoveCheckedWarning() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move_checked"))

		activity.onCreate(saved, null)

		assertThat(activity.remove_check.visibility, `is`(VISIBLE))
	}

	@Test
	fun populateResponseDescription() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move"))

		activity.onCreate(saved, null)

		assertThat(activity.description.text.toString(), `is`("move"))

		val move = activity.getPrivate<Move>("move")

		activity.description.setText("another")
		assertThat(move.description, `is`("another"))
	}

	@Test
	fun populateResponseDate() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move"))

		activity.onCreate(saved, null)

		assertThat(activity.date.text.toString(), `is`("2020-03-08"))

		val pickerButton = shadowOf(activity.date_picker)
		assertNotNull(pickerButton.onClickListener)
	}

	@Test
	fun populateResponseUsingCategories() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move_with_category"))

		activity.onCreate(saved, null)

		assertThat(activity.category.text.toString(), `is`("My Category"))

		val categoryButton = shadowOf(
			activity.category_picker
		)
		assertNotNull(categoryButton.onClickListener)
	}

	@Test
	fun populateResponseNotUsingCategories() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form_not_using_categories"))
		saved.putString("move", readBundle("move_without_category"))

		activity.onCreate(saved, null)

		assertThat(activity.category.visibility, `is`(GONE))
		assertThat(activity.category_picker.visibility, `is`(GONE))
	}

	@Test
	fun populateResponseWarnCategoryLose() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form_not_using_categories"))
		saved.putString("move", readBundle("move_with_category"))

		activity.onCreate(saved, null)

		assertThat(activity.category.visibility, `is`(GONE))
		assertThat(activity.category_picker.visibility, `is`(VISIBLE))

		val fieldFlags = activity.category_picker.paintFlags
		val strikeLine = Paint.STRIKE_THRU_TEXT_FLAG
		assertThat(fieldFlags.and(strikeLine), `is`(strikeLine))
	}

	@Test
	fun populateResponseNatureOut() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move_out"))

		activity.onCreate(saved, null)

		assertTrue(activity.nature_out.isChecked)

		val move = activity.getPrivate<Move>("move")

		assertThat(move.natureEnum, `is`(Nature.Out))

		assertNotNull(move.outUrl)
		assertNull(move.inUrl)
	}

	@Test
	fun populateResponseNatureTransfer() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move"))

		activity.onCreate(saved, null)

		assertTrue(activity.nature_transfer.isChecked)

		val move = activity.getPrivate<Move>("move")

		assertThat(move.natureEnum, `is`(Nature.Transfer))

		assertNotNull(move.outUrl)
		assertNotNull(move.inUrl)

		assertThat(activity.account_out.text.toString(), not(`is`("account out")))
		assertThat(activity.account_in.text.toString(), not(`is`("account in")))
	}

	@Test
	fun populateResponseNatureIn() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move_in"))

		activity.onCreate(saved, null)

		assertTrue(activity.nature_in.isChecked)

		val move = activity.getPrivate<Move>("move")

		assertThat(move.natureEnum, `is`(Nature.In))

		assertNull(move.outUrl)
		assertNotNull(move.inUrl)
	}

	@Test
	fun populateResponseAccounts() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move"))

		activity.onCreate(saved, null)

		assertThat(activity.account_out.text.toString(), `is`("My Out"))
		assertThat(activity.account_in.text.toString(), `is`("My In"))
	}

	@Test
	fun populateResponseDetails() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move_detailed"))

		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		assertTrue(move.isDetailed)

		assertThat(activity.simple_value.visibility, `is`(GONE))
		assertThat(activity.detailed_value.visibility, `is`(VISIBLE))

		assertThat(activity.details.childCount, `is`(2))

		val detail1 = activity.details.getChildAt(0) as DetailBox
		val getText1 = { id: Int -> detail1.findViewById<TextView>(id).text.toString() }
		assertThat(getText1(R.id.detail_description), `is`("detail 1"))
		assertThat(getText1(R.id.detail_amount), `is`("1"))
		assertThat(getText1(R.id.detail_value), `is`("27.00".getDecimal()))

		val detail2 = activity.details.getChildAt(1) as DetailBox
		val getText2 = { id: Int -> detail2.findViewById<TextView>(id).text.toString() }
		assertThat(getText2(R.id.detail_description), `is`("detail 2"))
		assertThat(getText2(R.id.detail_amount), `is`("2"))
		assertThat(getText2(R.id.detail_value), `is`("54.00".getDecimal()))
	}

	@Test
	fun populateResponseValue() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move_single_value"))

		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		assertFalse(move.isDetailed)

		assertThat(activity.simple_value.visibility, `is`(VISIBLE))
		assertThat(activity.detailed_value.visibility, `is`(GONE))

		assertThat(activity.value.text.toString(), `is`("1.00".getDecimal()))

		activity.value.setText("2")
		assertThat(move.value, `is`(2.0))

		assertThat(activity.details.childCount, `is`(0))
	}

	@Test
	fun onSaveInstance() {
		val originalMove = Gson().fromJson(
			readBundle("move"),
			Move::class.java
		)

		val originalForm = Gson().fromJson(
			readBundle("move_form"),
			MoveCreation::class.java
		)

		val originalState = Bundle()
		originalState.putJson("move", originalMove)
		originalState.putJson("moveForm", originalForm)

		activity.onCreate(originalState, null)

		val newState = Bundle()
		activity.onSaveInstanceState(newState, PersistableBundle())

		val newMove = newState.getFromJson("move", Move())
		assertThat(newMove, `is`(originalMove))

		val newForm = newState.getFromJson("moveForm", MoveCreation())
		assertThat(newForm, `is`(originalForm))
	}

	@Test
	fun dateDialog() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		assertThat(activity.date.text.toString(), `is`(""))

		activity.showDatePicker()

		val dialog = getLastDatePicker()
		dialog.updateDate(1986, Calendar.MARCH, 27)
		dialog.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		assertThat(activity.date.text.toString(), `is`("1986-03-27"))
	}

	@Test
	fun dateTyping() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		assertThat(activity.date.text.toString(), `is`(""))

		activity.date.append("1986-03-27")

		val move = activity.getPrivate<Move>("move")
		assertThat(move.date, `is`(Date(1986, 3, 27)))
	}

	@Test
	fun categoryDialog() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		assertThat(activity.category.text.toString(), `is`(""))
		assertNull(move.categoryName)

		activity.changeCategory()
		shadowOf(getLatestAlertDialog()).clickOnItem(1)

		assertThat(activity.category.text.toString(), `is`("My Category"))
		assertThat(move.categoryName, `is`("category"))
	}

	@Test
	fun categoryTyping() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val suggestions = activity.category.adapter
		assertNotNull(suggestions)
		assertThat(suggestions.count, `is`(1))

		val move = activity.getPrivate<Move>("move")

		assertThat(activity.category.text.toString(), `is`(""))
		assertNull(move.categoryName)

		activity.category.append("My Category")

		assertThat(move.categoryName, `is`("category"))
	}

	@Test
	fun warnLoseCategory() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form_not_using_categories"))
		saved.putString("move", readBundle("move_with_category"))
		activity.onCreate(saved, null)

		activity.category_picker.performClick()

		val alert = getLatestAlertDialog()
		val shadow = shadowOf(alert)

		assertThat(shadow.title.toString(), `is`("Ops!"))
		val message = activity.getString(R.string.losingCategory)
		assertThat(shadow.message.toString(), `is`(message))
	}

	@Test
	fun accountOutDialog() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		assertThat(activity.account_out.text.toString(), `is`(""))
		assert(move.outUrl.isNullOrEmpty())

		activity.changeAccountOut()
		shadowOf(getLatestAlertDialog()).clickOnItem(1)

		assertThat(activity.account_out.text.toString(), `is`("My Out"))
		assertThat(move.outUrl, `is`("out"))

		assertTrue(activity.nature_out.isChecked)
		assertFalse(activity.nature_transfer.isChecked)
		assertFalse(activity.nature_in.isChecked)
	}

	@Test
	fun accountOutTyping() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		assertThat(activity.account_out.text.toString(), `is`(""))
		assert(move.outUrl.isNullOrEmpty())

		activity.account_out.append("My Out")

		assertThat(move.outUrl, `is`("out"))

		assertTrue(activity.nature_out.isChecked)
		assertFalse(activity.nature_transfer.isChecked)
		assertFalse(activity.nature_in.isChecked)
	}

	@Test
	fun accountInDialog() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		assertThat(activity.account_in.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		activity.changeAccountIn()
		shadowOf(getLatestAlertDialog()).clickOnItem(2)

		assertThat(activity.account_in.text.toString(), `is`("My In"))
		assertThat(move.inUrl, `is`("in"))

		assertFalse(activity.nature_out.isChecked)
		assertFalse(activity.nature_transfer.isChecked)
		assertTrue(activity.nature_in.isChecked)
	}

	@Test
	fun accountInTyping() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		assertThat(activity.account_in.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		activity.account_in.append("My In")

		assertThat(move.inUrl, `is`("in"))

		assertFalse(activity.nature_out.isChecked)
		assertFalse(activity.nature_transfer.isChecked)
		assertTrue(activity.nature_in.isChecked)
	}

	@Test
	fun accountOutAndInDialog() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		assertThat(activity.account_in.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		activity.changeAccountIn()
		shadowOf(getLatestAlertDialog()).clickOnItem(2)

		activity.changeAccountOut()
		shadowOf(getLatestAlertDialog()).clickOnItem(1)

		assertThat(activity.account_in.text.toString(), `is`("My In"))
		assertThat(move.inUrl, `is`("in"))

		assertThat(activity.account_out.text.toString(), `is`("My Out"))
		assertThat(move.outUrl, `is`("out"))

		assertFalse(activity.nature_out.isChecked)
		assertTrue(activity.nature_transfer.isChecked)
		assertFalse(activity.nature_in.isChecked)
	}

	@Test
	fun accountOutAndInTyping() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		assertThat(activity.account_in.text.toString(), `is`(""))
		assert(move.inUrl.isNullOrEmpty())

		activity.account_in.append("My In")
		activity.account_out.append("My Out")

		assertThat(move.inUrl, `is`("in"))
		assertThat(move.outUrl, `is`("out"))

		assertFalse(activity.nature_out.isChecked)
		assertTrue(activity.nature_transfer.isChecked)
		assertFalse(activity.nature_in.isChecked)
	}

	@Test
	fun useDetailed() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		activity.useDetailed(View(activity))

		assertTrue(move.isDetailed)

		assertThat(activity.detail_amount.text.toString(), `is`("1"))
	}

	@Test
	fun useSimple() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		activity.useSimple(View(activity))

		assertFalse(move.isDetailed)
	}

	@Test
	fun addDetail() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		activity.detail_description.setText("cat")
		activity.detail_amount.setText("18")
		activity.detail_value.setText("3.14".getDecimal())

		activity.addDetail(View(activity))

		assertThat(activity.detail_description.text.toString(), `is`(""))
		assertThat(activity.detail_amount.text.toString(), `is`("1"))
		assertThat(activity.detail_value.text.toString(), `is`(""))

		assertThat(move.detailList.size, `is`(1))
		assertThat(move.detailList[0].description, `is`("cat"))
		assertThat(move.detailList[0].amount, `is`(18))
		assertThat(move.detailList[0].value, `is`(3.14))

		assertThat(activity.details.childCount, `is`(1))
		val detail = activity.details.getChildAt(0) as DetailBox
		val getText = { id: Int -> detail.findViewById<TextView>(id).text.toString() }
		assertThat(getText(R.id.detail_description), `is`("cat"))
		assertThat(getText(R.id.detail_amount), `is`("18"))
		assertThat(getText(R.id.detail_value), `is`("3.14".getDecimal()))
	}

	@Test
	fun addDetailNoDescription() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		activity.detail_description.setText("")
		activity.detail_amount.setText("18")
		activity.detail_value.setText("3.14".getDecimal())

		activity.addDetail(View(activity))

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.title.toString(), `is`("Ops!"))
		val message = activity.getString(R.string.fill_all)
		assertThat(shadow.message.toString(), `is`(message))

		assertThat(move.detailList.size, `is`(0))
		assertThat(activity.details.childCount, `is`(0))
	}

	@Test
	fun addDetailNoAmount() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		activity.detail_description.setText("cat")
		activity.detail_amount.setText("")
		activity.detail_value.setText("3.14".getDecimal())

		activity.addDetail(View(activity))

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.title.toString(), `is`("Ops!"))
		val message = activity.getString(R.string.fill_all)
		assertThat(shadow.message.toString(), `is`(message))

		assertThat(move.detailList.size, `is`(0))
		assertThat(activity.details.childCount, `is`(0))
	}

	@Test
	fun addDetailNoValue() {
		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")

		activity.detail_description.setText("cat")
		activity.detail_amount.setText("18")
		activity.detail_value.setText("")

		activity.addDetail(View(activity))

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.title.toString(), `is`("Ops!"))
		val message = activity.getString(R.string.fill_all)
		assertThat(shadow.message.toString(), `is`(message))

		assertThat(move.detailList.size, `is`(0))
		assertThat(activity.details.childCount, `is`(0))
	}

	@Test
	fun save() {
		activity.simulateNetwork()
		mocker.server.enqueue("empty")

		activity.intent.putExtra(
			"__parent",
			WelcomeActivity::class.java
		)
		activity.intent.putExtra("id", guid)

		val saved = Bundle()
		saved.putString("moveForm", readBundle("move_form"))
		saved.putString("move", readBundle("move"))
		activity.onCreate(saved, null)

		val move = activity.getPrivate<Move>("move")
		assertNotNull(move.value)

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

		mocker.server.enqueue("move_get")
		activity.onCreate(null, null)

		mocker.server.enqueue("empty")
		activity.save(View(activity))

		activity.waitTasks(mocker.server)

		val requestPath = mocker.server.lastPath()
		val urlGuid = requestPath.split('/').last()
		assertThat(urlGuid, `is`(guid.toString()))
	}
}
