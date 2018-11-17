package com.darakeon.dfm.settings

import com.darakeon.dfm.api.entities.settings.Settings
import com.darakeon.dfm.base.SmartStatic

object SettingsStatic : SmartStatic
{
	override var succeeded: Boolean = false

	var settings: Settings = Settings(false, false)
}

