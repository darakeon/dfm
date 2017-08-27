package com.dontflymoney.viewhelper;

import java.text.DecimalFormat;

import android.annotation.SuppressLint;
import android.content.Context;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.dontflymoney.entities.Move;
import com.dontflymoney.view.R;

@SuppressLint("ViewConstructor")
public class LinearLayoutDetail extends LinearLayout 
{
	Move move;
	String description;
	int amount;
	double value;
	
	public LinearLayoutDetail(Context context, Move move, String description, int amount, double value)
	{
		super(context);
		
		this.move = move;
		this.description = description;
		this.amount = amount;
		this.value = value;

		TextView descriptionField = new TextView(context);
   	    descriptionField.setText(description);
   	    
   	    addView(descriptionField);
   	    
   	    TextView amountField = new TextView(context); 
   	    amountField.setText(Integer.toString(amount));
   	    addView(amountField);
   	    
   	    TextView valueField = new TextView(context); 
   	    DecimalFormat formatter = new DecimalFormat("0.00");
   	    valueField.setText(formatter.format(value));
   	    addView(valueField);
   	    
   	    Button buttonField = new Button(context); 
   	    buttonField.setText(R.string.remove_detail);
   	    buttonField.setOnClickListener(new RemoveDetail());
   	    addView(buttonField);
	}
	
	private class RemoveDetail implements OnClickListener
	{
		@Override
		public void onClick(View button)
		{
			move.Remove(description, amount, value);
			
			LinearLayout item = (LinearLayout)button.getParent();
			((LinearLayout)item.getParent()).removeView(item);
		}
	}

}
