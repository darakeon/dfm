package com.dontflymoney.entities;

public enum Nature
{
    Out,
    In,
    Transfer;

    public int GetNumber()
    {
        switch (this)
        {
            case Out: return 0;
            case In: return 1;
            case Transfer: return 2;
            default: throw new UnsupportedOperationException();
        }
    }

    public static Nature GetNature(int number)
    {
        switch (number)
        {
            case 0: return Out;
            case 1: return In;
            case 2: return Transfer;
            default: throw new UnsupportedOperationException();
        }
    }
}
