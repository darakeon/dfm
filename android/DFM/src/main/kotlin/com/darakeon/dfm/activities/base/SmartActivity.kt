package com.darakeon.dfm.activities.base

import android.content.Context
import android.os.Bundle
import android.view.*
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.AccountsActivity
import com.darakeon.dfm.activities.objects.SmartStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.api.Step
import com.darakeon.dfm.user.Authentication
import com.darakeon.dfm.user.Language
import com.darakeon.dfm.user.Theme
import org.json.JSONException
import org.json.JSONObject

abstract class SmartActivity<T : SmartStatic>(var static : T) : FixOrientationActivity() {

    var clickedView: View? = null

    var succeeded: Boolean
        get() = static.succeeded
        set(value) { static.succeeded = value }

    protected abstract fun contentView(): Int
    protected open fun optionsMenuResource(): Int {
        return 0
    }

    protected open fun contextMenuResource(): Int {
        return 0
    }

    protected open fun viewWithContext(): Int {
        return 0
    }

    protected open val isLoggedIn: Boolean
        get() = true

    protected fun hasParent(): Boolean {
        return false
    }

    protected open fun changeContextMenu(view: View, menuInfo: ContextMenu) {}

    protected val Authentication: Authentication get() = Authentication(this)

    protected val form: Form get() = Form(this)
    val message: Message<T> get() = Message(this)
    protected val navigation: Navigation<T> get() = Navigation(this, Authentication)
    internal val resultHandler: ResultHandler<T> get() = ResultHandler(this, navigation)

    var request: InternalRequest<T>? = null


    override fun onCreate(savedInstanceState: Bundle?) {
        Language.ChangeFromSaved(this)
        Theme.ChangeFromSaved(this)

        super.onCreate(savedInstanceState)

        static.inflater = getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

        setContentView(contentView())
        setupActionBar()

        if (viewWithContext() != 0) {
            val contextView = findViewById(viewWithContext())
            registerForContextMenu(contextView)
        }
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        super.onCreateOptionsMenu(menu)

        if (optionsMenuResource() != 0)
            menuInflater.inflate(optionsMenuResource(), menu)

        if (isLoggedIn)
            menuInflater.inflate(R.menu.common, menu)

        return true
    }

    override fun onCreateContextMenu(menu: ContextMenu, v: View, menuInfo: ContextMenu.ContextMenuInfo) {
        super.onCreateContextMenu(menu, v, menuInfo)

        if (contextMenuResource() != 0) {
            menuInflater.inflate(contextMenuResource(), menu)
            changeContextMenu(v, menu)
        }
    }

    override fun onDestroy() {
        super.onDestroy()
        request?.Cancel()
    }

    private fun setupActionBar() {
        if (hasParent()) {
            val actionBar = actionBar

            actionBar?.setDisplayHomeAsUpEnabled(true)
        }
    }


    fun back(view: View) {
        navigation.back()
    }

    fun logout(menuItem: MenuItem) {
        navigation.logout()
    }

    fun close(menuItem: MenuItem) {
        navigation.close()
    }

    fun refresh(menuItem: MenuItem) {
        refresh()
    }

    fun goToAccounts(menuItem: MenuItem) {
        navigation.redirect(AccountsActivity::class.java)
    }

    fun goToSettings(menuItem: MenuItem) {
        navigation.goToSettings()
    }

    fun refresh() {
        finish()
        startActivity(intent)
    }


    @Throws(JSONException::class)
    abstract fun HandleSuccess(data: JSONObject, step: Step)

    fun HandlePostResult(result: JSONObject, step: Step) {
        resultHandler.HandlePostResult(result, step)
        succeeded = true
    }

    fun HandlePostError(error: String) {
        succeeded = false
        resultHandler.HandlePostError(error)
    }


    open fun EnableScreen() {
        succeeded = true
    }

    fun Reset() {
        succeeded = false
    }



}