package com.dontflymoney.viewhelper;

import android.text.Editable;
import android.text.TextWatcher;

public abstract class AfterTextWatcher implements TextWatcher {
	
	public abstract void textChanged(String text);
	
	public void afterTextChanged(Editable s) {
    	textChanged(s.toString());
    }
    public void beforeTextChanged(CharSequence s, int start, int count, int after){}
    public void onTextChanged(CharSequence s, int start, int before, int count){}
}
