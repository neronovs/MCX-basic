package com.neronov.aleksei.mcxbasic;

import java.io.Serializable;

/**
 * Created by newuser on 15.03.16.
 */
public class VariableSet implements Serializable {
    public String var;
    public String name;
    public boolean stringType;

    private VariableSet() {
        var = "";
        name = "";
        stringType = false;
    }

    public String getVar() {
        return this.var;
    }

    public void setVar(String value) {
        var = value;
    }

    public String getName() {
        return this.name;
    }

    public void setName(String value) {
        name = value;
    }

    public boolean getStringType() {
        return this.stringType;
    }

    public void setStringType(boolean value) {
        stringType = value;
    }

}
