package com.neronov.aleksei.mcxbasic;

import java.io.Serializable;
import java.util.ArrayList;

/**
 * Created by newuser on 15.03.16.
 */
public class ArraySet implements Serializable {
    public String name;
    public ArrayList value;
    public int size;

    public ArraySet() {
        name = "";
        value = new ArrayList();
        size = 0;
    }

    public String getName() {
        return this.name;
    }

    public void setName(String value) {
        name = value;
    }

    public ArrayList getValue() {
        return this.value;
    }

    public void setValue(ArrayList value1) {
        value = value1;
    }

    public int getSize() {
        return this.size;
    }

    public void setSize(int value) {
        size = value;
    }

}
