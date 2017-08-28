package com.dontflymoney.baseactivity

import android.app.ActionBar
import android.content.Context
import android.os.Bundle
import android.view.ContextMenu
import android.view.LayoutInflater
import android.view.Menu
import android.view.MenuItem
import android.view.View

import com.dontflymoney.api.InternalRequest
import com.dontflymoney.api.Step
import com.dontflymoney.userdata.Authentication
import com.dontflymoney.userdata.Language
import com.dontflymoney.view.AccountsActivity
import com.dontflymoney.view.R

import org.json.JSONException
import org.json.JSONObject

abstract class SmartActivity : FixOrientationActivity() {

    var clickedView: View? = null

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

    protected val isLoggedIn: Boolean
        get() = true

    protected fun hasParent(): Boolean {
        return false
    }

    protected open fun changeContextMenu(view: View, menuInfo: ContextMenu) {}

    protected var Authentication: Authentication

    protected var form: Form
    var message: Message
    protected var navigation: Navigation
    protected var resultHandler: ResultHandler
    protected var license: License? = null

    protected var request: InternalRequest? = null


    override fun onCreate(savedInstanceState: Bundle?) {
        Language.ChangeFromSaved(this)

        super.onCreate(savedInstanceState)

        inflater = getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

        setContentView(contentView())
        setupActionBar()

        if (viewWithContext() != 0) {
            val contextView = findViewById(viewWithContext())
            registerForContextMenu(contextView)
        }

        Authentication = Authentication(this)

        form = Form(this)
        message = Message(this)
        navigation = Navigation(this, Authentication)
        resultHandler = ResultHandler(this, navigation)
        license = License(this)
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

        if (license != null)
            license!!.Destroy()

        if (request != null)
            request!!.Cancel()
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
        navigation.redirect(AccountsActivity::class.java!!)
    }

    fun goToSettings(menuItem: MenuItem) {
        navigation.goToSettings()
    }

    fun refresh() {
        finish()
        startActivity(intent)
    }


    @Throws(JSONException::class)
    protected abstract fun HandleSuccess(data: JSONObject, step: Step)

    fun HandlePostResult(result: JSONObject, step: Step) {
        resultHandler.HandlePostResult(result, step)
        succeded = true
    }

    fun HandlePostError(error: String, step: Step) {
        succeded = false
        resultHandler.HandlePostError(error, step)
    }


    open fun EnableScreen() {
        succeded = true
    }

    fun Reset() {
        succeded = false
    }

    companion object {
        protected var inflater: LayoutInflater? = null

        protected var succeded = false
    }


}