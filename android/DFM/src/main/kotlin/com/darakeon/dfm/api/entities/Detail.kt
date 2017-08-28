package com.darakeon.dfm.api.entities

class Detail {
    var Description: String? = null
    var Amount: Int = 0
    var Value: Double = 0.toDouble()

    internal fun Equals(description: String?, amount: Int, value: Double): Boolean {
        return Description == description
                && Amount == amount
                && Value == value
    }

}
