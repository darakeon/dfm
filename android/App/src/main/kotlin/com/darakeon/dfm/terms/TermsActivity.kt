package com.darakeon.dfm.terms

import android.os.Bundle
import android.view.View
import android.widget.ArrayAdapter
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.TermsBinding
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.backWithExtras
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.terms.Terms

class TermsActivity : BaseActivity<TermsBinding>() {
	override fun inflateBinding(): TermsBinding {
		return TermsBinding.inflate(layoutInflater)
	}

	override val title = R.string.title_activity_terms

	private var terms = Terms()
	private val termsKey = "terms"

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (savedInstanceState == null) {
			callApi { api ->
				api.getTerms {
					this.populateScreen(it)
				}
			}
		} else {
			this.populateScreen(
				savedInstanceState.getFromJson(termsKey, Terms())
			)
		}
	}

	private fun populateScreen(data: Terms) {
		terms = data

		val clauses = data.content.format()

		binding.content.adapter =
			ArrayAdapter(this,android.R.layout.simple_list_item_1,clauses);

		binding.date.text =
			getString(R.string.effective_date).format(data.date.format())
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)
		outState.putJson(termsKey, terms)
	}

	fun exit(@Suppress(ON_CLICK) view: View) {
		backWithExtras()
	}
}
