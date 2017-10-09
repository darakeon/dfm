package com.darakeon.dfm.user

import android.content.Context
import com.google.android.gms.iid.InstanceID

fun Context.GetId(): String {
	return InstanceID.getInstance(this).id
}
