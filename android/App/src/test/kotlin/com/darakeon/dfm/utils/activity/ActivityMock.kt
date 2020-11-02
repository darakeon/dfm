package com.darakeon.dfm.utils.activity

import android.app.Activity
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.ApiCaller
import com.darakeon.dfm.lib.api.RequestService
import com.darakeon.dfm.lib.api.Retrofit
import com.darakeon.dfm.testutils.setPrivate
import com.darakeon.dfm.testutils.robolectric.ActivityMock
import kotlin.reflect.KClass

class ActivityMock<A>(
	activityClass: KClass<A>
) : ActivityMock<RequestService, A, Api<A>> (
	RequestService::class,
	Retrofit::build,
	activityClass,
	{ obj, prop, value -> obj.setPrivate(prop) { value }},
	{ act, url -> Api(act, url) }
) where A: Activity, A: ApiCaller
